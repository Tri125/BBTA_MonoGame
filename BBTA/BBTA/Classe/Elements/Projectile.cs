using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using BBTA.Partie_De_Jeu;
using BBTA.Outils;

namespace BBTA.Elements
{
    /// <summary>
    /// La classe projectile sert de base à tous les projectiles du jeu.
    /// Il s'agit en fait d'un objet physique avec une vitesse initiale et qui peut déclancher un événement lorsqu'il explose.
    /// La composante "IsBullet" de son corps physique est activée pour améliorer la détection des collisions.
    /// </summary>
    public abstract class Projectile : ObjetPhysique
    {
        //Informations quant à l'explosion du projectile------------------------------------------------------------------------
        private readonly int rayonExplosion;
        protected bool explose = false; //Indique si le processus d'explosion est enclanché.

        //Variables reliées à l'affichage du projectile-------------------------------------------------------------------------
        protected SpriteEffects retourner = SpriteEffects.None;
        protected Rectangle positionSpriteSheet;

        //Variables issues du moteur Farseer------------------------------------------------------------------------------------
        private World mondePhysique;

        //Événements et éléments reliés-----------------------------------------------------------------------------------------
        public delegate void DelegateExplosion(Projectile proejctileExplosant, Vector2 position, int rayonExplosion);
        public event DelegateExplosion Explosion;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel le projectile évoluera</param>
        /// <param name="forme">Sa forme géométrique</param>
        /// <param name="texture">La spritesheet conteant le projectile en question</param>
        /// <param name="positionSpriteSheet">La position et la taille de l'image représentant le projectile dans la spritesheet</param>
        /// <param name="positionDepart">La position de départ du projectile lors de son lancement</param>
        /// <param name="vitesse">La vitesse initiale du projectile</param>
        /// <param name="rayonExplosion">Le rayon de l'explosion qu'il créera</param>
        public Projectile(World mondePhysique, Shape forme, Texture2D texture, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 vitesse, int rayonExplosion)
            : base(texture, mondePhysique, forme)
        {
            this.mondePhysique = mondePhysique;
            this.rayonExplosion = rayonExplosion;
            this.positionSpriteSheet = positionSpriteSheet;
            corpsPhysique.IsBullet = true; //Améliore la détection des collisions avec les objets à grande vitesse comme celui-ci
            corpsPhysique.Position = positionDepart;
            corpsPhysique.BodyType = BodyType.Dynamic;
            corpsPhysique.LinearVelocity = vitesse;
        }

        /// <summary>
        /// Met à jour le projectile.
        /// Détecte si le projectile issue de cette classe explose (explose == true), un événement est déclanché et le corps physique est supprimé.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            if (explose == true && Explosion != null)
            {
                Explosion(this, Conversion.MetreAuPixel(corpsPhysique.Position), Conversion.MetreAuPixel(rayonExplosion));
                corpsPhysique.Dispose();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Affiche le projectile à l'écran
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Conversion.MetreAuPixel(corpsPhysique.Position), positionSpriteSheet, Color.White,
                             corpsPhysique.Rotation, new Vector2(positionSpriteSheet.Width / 2, positionSpriteSheet.Height / 2), 1, retourner, 0);
        }

    }
}