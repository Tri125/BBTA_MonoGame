using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Classe.Elements;

namespace BBTA.Classe.Interface
{
    /// <summary>
    /// Il s'agit d'un bouton qui dispose de l'image d'une arme spécifique et qui affiche le nombre de munitions qu'il reste de cette arme.
    /// Elle ne fait qu'afficher certaines informations et ne gère pas la modification du nombre de munitions.
    /// </summary>
    public class IndicateurArmeRestante:Bouton
    {
        //Variables reliées à l'affichage du bouton-----------------------------------------------------------------------------------------
        private SpriteFont police;
        private Armes type;

        //Propriétés------------------------------------------------------------------------------------------------------------------------
        public int nbArmeRestantes { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Spritesheet dans laquelle se trouve la texture du bouton.</param>
        /// <param name="boutonDansSpriteSheet">Position de la texture du bouton dans la spritesheet.</param>
        /// <param name="position">Position du bouton à l'écran</param>
        /// <param name="type">Type de l'arme</param>
        /// <param name="police">Police d'écriture pour le nombre de munitions restantes dans la pastille.</param>
        public IndicateurArmeRestante(Texture2D texture, Rectangle? boutonDansSpriteSheet, Vector2 position, Armes type, SpriteFont police)
            : base(texture, position, boutonDansSpriteSheet)
        {
            this.type = type;
            this.police = police;
        }

        /// <summary>
        /// Affiche le bouton à l'écran
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Transformation du nombre de munitions restantes en chaîne de caractère.
            StringBuilder nbArmes = new StringBuilder(nbArmeRestantes.ToString());
            //Pour s'assurer d'un affichage standardisé, un "zéro" est ajouté devant les nombres inférieurs à 10.
            if (nbArmeRestantes < 10)
            {
                nbArmes.Insert(0, "0");
            }
            spriteBatch.DrawString(police, nbArmes.ToString(), new Vector2(Position.X + 60, Position.Y+5), Color.White);
        }

        /// <summary>
        /// Permet d'obtenir le type de l'arme qui est affichée par le bouton.
        /// </summary>
        /// <returns>Type de l'arme</returns>
        public Armes ObtenirType()
        {
            return type;
        }
    }
}
