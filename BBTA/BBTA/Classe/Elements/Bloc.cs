using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
namespace BBTA.Elements
{
    /// <summary>
    /// Les objets issus de la classe Bloc agissent comme blocs au sein de la carte.
    /// -----------------------------------------------------------------------------------------------
    /// Instancie ledit bloc au sein de l'environnement physique Farseer.
    /// Détermine, en vertu des informations provenent d'une explosion, si le bloc existe toujours
    /// -----------------------------------------------------------------------------------------------
    /// </summary>
    public class Bloc : Sprite
    {
        //Variables-----------------------------------------------------------------------------------------------
        public Body corpsPhysique { get; set; }
        private float metrePixel;
        private TypeBloc type;
        private bool enDestruction = false;
        private int etapeDestruction = 0;
        private int tempsDepuisEtapePrecedente = 0;
        private const int TEMPS_ENTRE_ETAPES = 50;
        //Constantes----------------------------------------------------------------------------------------------
        private const float DENSITE = 0;
        private const float seuilResistance = 5;

        public event EventHandler AnimationDestructionTerminee;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">World Farseer</param>
        /// <param name="position">Position du bloc à l'écran (Coordonnées)</param>
        /// <param name="texture">Texture du bloc</param>
        /// <param name="tailleCote">Taille d'un côté du bloc (en mètre pour Farseer)</param>
        public Bloc(World mondePhysique, Vector2 position, Texture2D texture, float tailleCote, float metrePixel, TypeBloc type)
            : base(texture, position * metrePixel)
        {
            this.type = type;
            this.metrePixel = metrePixel;
            corpsPhysique = BodyFactory.CreateRectangle(mondePhysique, tailleCote, tailleCote, DENSITE, position);
            corpsPhysique.CollisionCategories = Category.All;
            corpsPhysique.CollidesWith = Category.All;
            corpsPhysique.IsStatic = true;
            corpsPhysique.Friction = 0.3f;
            echelle = 1.01f;
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
            /*Les dégâts causés par une explosion à une certaine distance du centre de l'explosion
             * sont déterminés par le biais d'une équation linéaire(ax+b).  Au centre de l'explosion, 
             * les dégâts causés sont maximals alors qu'au bout du rayon d'effet, ils sont nuls*/
            if (Vector2.Distance(lieu, Position) < rayonExplosion)
            {
                corpsPhysique.Dispose();
                enDestruction = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (enDestruction)
            {
                tempsDepuisEtapePrecedente += gameTime.ElapsedGameTime.Milliseconds;
                if (tempsDepuisEtapePrecedente > TEMPS_ENTRE_ETAPES)
                {
                    tempsDepuisEtapePrecedente -= TEMPS_ENTRE_ETAPES;
                    etapeDestruction++;
                }
                if (etapeDestruction == 5 && AnimationDestructionTerminee != null)
                {
                    AnimationDestructionTerminee(this, new EventArgs());
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle selection = new Rectangle((int)type * 40, etapeDestruction*40, 40, 40);
            Vector2 pointCentral = new Vector2(20, 20);
            spriteBatch.Draw(texture, corpsPhysique.Position * metrePixel, selection,
                             Color.White, angleRotation, pointCentral, echelle,
                             SpriteEffects.None, 0);
        }
    }
}