using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Elements;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using System.Timers;
using BBTA.Classe.Outils;
using FarseerPhysics.Collision;

namespace BBTA.Classe.Elements
{
    /// <summary>
    /// Les mines dans BBTA sont des projectiles qui explosent lorsqu'un joueur est trop près de celle-ci et en aucun autre temps.
    /// </summary>
    public class Mine:Projectile
    {
        //Variables issues du moteur physique Farseer-----------------------------------------------------------------------------
        private World mondePhysique; //Servira à détecter les joueurs trop près

        //Position du bloc au-dessous de la mine----------------------------------------------------------------------------------
        private Vector2 blocAuDessous;

        //Variables reliées au compte à rebours-----------------------------------------------------------------------------------
        private Timer compteRebours = new Timer(200);
        private int tempsDepuisLumierePrecedente = 0;

        //Cosntantes--------------------------------------------------------------------------------------------------------------
        public const int RAYON_EXPLOSION = 4;
        private const int DELAI_LUMIERE = 200; //Délai pour l'intermitance du petit indicateur lumineux de la mine

        //Événements--------------------------------------------------------------------------------------------------------------
        public event EventHandler FixationAuSol;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel évoluera la mine</param>
        /// <param name="positionSpriteSheet">Position de la texture de la mine dans la spritesheet</param>
        /// <param name="positionDepart">Position initiale de la mine lors de son lancement</param>
        /// <param name="direction">Direction du lancer</param>
        /// <param name="vitesse">Vitesse initiale</param>
        /// <param name="texture">Texture de la mine qui sera affichée à l'écran</param>
        public Mine(ref World mondePhysique, Texture2D texture, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 vitesse)
            : base(mondePhysique, new CircleShape(Conversion.PixelAuMetre(7), 5), texture, positionSpriteSheet, positionDepart, vitesse, RAYON_EXPLOSION)

        {
            this.mondePhysique = mondePhysique;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            compteRebours.Elapsed += new ElapsedEventHandler(compteRebours_Elapsed);
        }

        /// <summary>
        /// Met à jour la mine.
        /// Anime le témoin lumineux.
        /// Détecte les objets à proximité et explose si c'est un joueur.
        /// Détecte s'il reste toujours un bloc derrière elle.
        /// Lorsqu'elle se colle, la mine s'oriente correctement;
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            //L'image de la mine est modifiée si le délai de temps est expiré.
            tempsDepuisLumierePrecedente += gameTime.ElapsedGameTime.Milliseconds;
            if (tempsDepuisLumierePrecedente > DELAI_LUMIERE)
            {
                tempsDepuisLumierePrecedente -= DELAI_LUMIERE;
                //Le rectangle de sélection de l'image de la spritesheet est décalé en fonction de l'état actuel du témoin lumineux.
                if (positionSpriteSheet.X == 3)
                {
                    positionSpriteSheet.Offset(texture.Width/2, 0);
                }
                else
                {
                    positionSpriteSheet.Offset(-texture.Width / 2, 0);
                }
            }

            //Si la mine est en mouvement dans les airs, son orientation suit celle de sa vitesse.
            if (corpsPhysique.LinearVelocity != Vector2.Zero)
            {
                corpsPhysique.Rotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X);
            }

            /* Si la mine n'obéit plus aux lois de la gravité, cela signifie qu'elle est fixée au sol. 
             * Ainsi, à partir de ce moment, on peut détecter les objets aux alentours pour savoir si elle doit exploser.
             * De même, la détection du bloc derrière elle est effectuée
             */
            if (corpsPhysique.IgnoreGravity)
            {
                //Aire où la détection est effectuée.  Carré 3x3 blocs centré sur la mine.
                AABB detectionAutourMine = new AABB(corpsPhysique.Position - new Vector2(1.5f), corpsPhysique.Position + new Vector2(1.5f));
                bool blocAuDessousEstDetecter = false; //Si à la fin du processus de détection cette variable est toujours fausse, c'est qu'il n'y a plus de bloc sous la mine.
                mondePhysique.QueryAABB(Fixture =>
                                        {
                                            //S'il n'y a plus de blocs au dessous, la mine disparaît.
                                            if (Fixture.Body.Position == blocAuDessous)
                                            {
                                                blocAuDessousEstDetecter = true;
                                                return false;
                                            }

                                            //Si c'est un objet qui se déplacer et que ce n'est pas la mine elle-même, le processus d'explosion est démarré.
                                            //Note : les projectiles sont aussi pris en compte.
                                            if (Fixture.Body.BodyType == BodyType.Dynamic && Fixture.Body != corpsPhysique)
                                            {
                                                compteRebours.Start();
                                                return false;
                                            }
                                            else
                                            {
                                                return true; //On continue de chercher
                                            }

                                        },
                                        ref detectionAutourMine);
                //Explosion de la mine s'il n'y a plus de bloc en-dessous.
                if (blocAuDessousEstDetecter == false)
                {
                    explose = true;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Lorsque la mine entre en collison, elle ne subit plus la gravité.
        /// De plus, elle s'oriente de manière à apparaître collée au mur.
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            //Elle est désormais immobile.
            corpsPhysique.IgnoreGravity = true;
            corpsPhysique.LinearVelocity = Vector2.Zero; 

            //On retient en mémoire la position du bloc en-dessous pour vérifier dans le Update s'il est toujours là.
            blocAuDessous = fixtureB.Body.Position;

            //La mine s'oriente en fonction de l'angle de la surface qu'elle frappe et du côté qu'elle le fait.
            if (contact.Manifold.LocalPoint.Y < 0)
            {
                corpsPhysique.Rotation = MathHelper.PiOver2 * 3;
            }
            else if (contact.Manifold.LocalPoint.Y > 0)
            {
                corpsPhysique.Rotation = MathHelper.PiOver2;
            }
            else if (contact.Manifold.LocalPoint.X > 0)
            {
                corpsPhysique.Rotation = 0;
            }
            else
            {
                corpsPhysique.Rotation = MathHelper.Pi;
            }

            //Un événement est déclanché pour indiquer que la mine est désormais positionnée et qu'elle n'est plus en vol.
            if (FixationAuSol != null)
            {
                FixationAuSol(this, new EventArgs());
            }

            return true;
        }

        /// <summary>
        /// Expiration du compte à rebours.  Le processus d'explosion démarre
        /// </summary>
        /// <param name="sender">Mine</param>
        /// <param name="e"></param>
        void compteRebours_Elapsed(object sender, ElapsedEventArgs e)
        {
            explose = true;
        }

        /// <summary>
        /// Dessine la mine correctement à l'écran
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ObtenirPosition(), positionSpriteSheet,
                             Color.White, corpsPhysique.Rotation, new Vector2(positionSpriteSheet.Width / 2, positionSpriteSheet.Height / 2), 1, retourner, 0);
        }
    }
}
