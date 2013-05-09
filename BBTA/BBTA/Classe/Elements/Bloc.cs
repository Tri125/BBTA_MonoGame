using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using BBTA.Classe.Outils;
using BBTA.Carte;

namespace BBTA.Classe.Elements
{
    /// <summary>
    /// Les objets issus de la classe Bloc agissent comme blocs au sein de la carte.
    /// -----------------------------------------------------------------------------------------------
    /// Instancie ledit bloc au sein de l'environnement physique Farseer.
    /// Détermine, en vertu des informations provenent d'une explosion, si le bloc existe toujours
    /// -----------------------------------------------------------------------------------------------
    /// </summary>
    public class Bloc : ObjetPhysique
    {
        //Variables dont la velur ne peut être changée en cours de route ----------------------------------------
        private readonly TypeBloc type; //S'il y a du gazon ou non; s'il a les coins ronds, etc.
        private readonly float taille; //D'un côté, en mètre.

        //Variables reliées au processus de destruction et à son affichage---------------------------------------
        private bool enDestruction = false;
        private int etapeDestruction = 0;
        private int tempsDepuisEtapePrecedente = 0;

        //Constantes---------------------------------------------------------------------------------------------
        private const float DENSITE = 0; //Le bloc étant statique, il n'a pas besoin de masse.
        private const float ECHELLE = 1.01f;
        private const int TEMPS_ENTRE_ETAPES = 50;

        //Propriétés---------------------------------------------------------------------------------------------
        public TypeBloc Type { get { return type; } }
        public float Taille { get { return taille; } }

        //Événements
        public event EventHandler AnimationDestructionTerminee;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">World Farseer</param>
        /// <param name="position">Position du bloc à l'écran (Coordonnées)</param>
        /// <param name="texture">Texture du bloc</param>
        /// <param name="tailleCote">Taille d'un côté du bloc (en mètre pour Farseer)</param>
        public Bloc(World mondePhysique, Vector2 position, Texture2D texture, float tailleCote, TypeBloc type)
            : base(texture, mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(tailleCote/2f, tailleCote/2f), DENSITE))
        {
            this.type = type;
            this.taille = tailleCote;
            corpsPhysique.Position = position;
            corpsPhysique.CollisionCategories = Category.All; //Un bloc entre en collision avec tous les objets du jeu
            corpsPhysique.CollidesWith = Category.All;
            corpsPhysique.IsStatic = true;
            corpsPhysique.Friction = 10f;
            corpsPhysique.UserData = this; //Le corps physique contient un définition du bloc.  De cette manières, à partir du monde physique, on peut accèder à la largeur du bloc.
        }

        /// <summary>
        /// Met à jour le bloc.  Anime correctement le bloc s'il est en voie d'être détruit.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //S'il est en voie d'être détruit
            if (enDestruction)
            {
                tempsDepuisEtapePrecedente += gameTime.ElapsedGameTime.Milliseconds;
                //Si le délai entre l'affichage de chacune des images est dépassé,
                if (tempsDepuisEtapePrecedente > TEMPS_ENTRE_ETAPES) 
                {
                    tempsDepuisEtapePrecedente -= TEMPS_ENTRE_ETAPES;
                    //La destruction est plus prononcée
                    etapeDestruction++;
                }
                //Si la destruction est complète, un événement est déclanché pour annoncer la destruction
                if (etapeDestruction == 5 && AnimationDestructionTerminee != null)
                {
                    AnimationDestructionTerminee(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Détermine si le bloc continue d'exister en vertue des informations provenant d'une explosion
        /// </summary>
        /// <param name="puissance">Puissance de l'explosion</param>
        /// <param name="rayon">Rayon de l'explosion</param>
        /// <param name="lieu">Lieu d'origine de l'explosion</param>
        /// <returns>Si le bloc doit être détruit</returns>
        public void Explose(Vector2 lieu, int rayonExplosion)
        {
            //Si le bloc se situe dans le rayon d'explosion, il est automatiquement détruit.
            if (Vector2.Distance(lieu, ObtenirPosition()) < rayonExplosion)
            {
                corpsPhysique.Dispose();
                enDestruction = true;
            }
        }

        /// <summary>
        /// Dessine le bloc correctement en fonction de sa destruction ou non, et des blocs environnants.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle selection = new Rectangle(Conversion.MetreAuPixel((int)type), Conversion.MetreAuPixel(etapeDestruction), 40, 40);
            Vector2 pointCentral = new Vector2(20, 20);
            spriteBatch.Draw(texture, ObtenirPosition(), selection, Color.White, corpsPhysique.Rotation, pointCentral, ECHELLE, SpriteEffects.None, 0);
        }
    }
}