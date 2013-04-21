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
namespace BBTA.Elements
{
    /// <summary>
    /// Les objets issus de la classe Bloc agissent comme blocs au sein de la carte.
    /// -----------------------------------------------------------------------------------------------
    /// Instancie ledit bloc au sein de l'environnement physique Farseer.
    /// Détermine, en vertu des informations provenent d'une explosion, si le bloc existe toujours
    /// -----------------------------------------------------------------------------------------------
    /// </summary>
    public class Bloc:ObjetPhysique
    {
        //Variables-----------------------------------------------------------------------------------------------
        private TypeBloc type;
        private float echelle;
        //Constantes----------------------------------------------------------------------------------------------
        private const float DENSITE = 1;
        private const float seuilResistance = 45;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">World Farseer</param>
        /// <param name="position">Position du bloc à l'écran (Coordonnées)</param>
        /// <param name="texture">Texture du bloc</param>
        /// <param name="tailleCote">Taille d'un côté du bloc (en mètre pour Farseer)</param>
        public Bloc(World mondePhysique, Vector2 position, Texture2D texture, float tailleCote, TypeBloc type)
            : base(texture, mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(tailleCote, tailleCote), DENSITE))
        {
            this.type = type;
            corpsPhysique.Position = position;
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
        public bool ExplosetIl(float energie, Vector2 lieu)
        {
            /*Les dégâts causés par une explosion à une certaine distance du centre de l'explosion
             * sont déterminés par le biais d'une équation linéaire(ax+b).  Au centre de l'explosion, 
             * les dégâts causés sont maximals alors qu'au bout du rayon d'effet, ils sont nuls*/
            float distance = Vector2.Distance(lieu, Conversion.MetreAuPixel(corpsPhysique.Position));
            if (energie / distance > seuilResistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool MeDetruire()
        {
            if (corpsPhysique == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle selection = new Rectangle((int)type * texture.Width / 5, 0, texture.Width / 5, texture.Height);
            Vector2 pointCentral = new Vector2(texture.Width / 5 / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, Conversion.MetreAuPixel(corpsPhysique.Position), selection,
                             Color.White, 0, pointCentral, echelle,
                             SpriteEffects.None, 0);
        }
    }
}