using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTA.Elements
{
    public class SpriteAnimer:Sprite
    {
        protected int nbColonnes;
        protected int nbRangees;
        
        protected int imageEnCours;
        protected readonly int nbImagesSequence;

        protected int milliSecParImage;
        protected int tempsdepuisDerniereImage;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture pour un affichage en 2 dimmensions</param>
        /// <param name="position">Position initiale du sprite</param>
        /// <param name="vitesse">Vitesse initiale de l'objet</param>        
        /// <param name="nbColonnes">Nombre de colonnes dans la sprite sheet</param>
        /// <param name="nbRangees">Nombre de rangee dans la sprite sheet</param>
        /// <param name="milliSecParImage">Délais désiré entre chaque image de la sprite sheet</param>
        public SpriteAnimer(Texture2D texture, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(texture)
        {
            this.nbColonnes = nbColonnes;
            this.nbRangees = nbRangees;
            this.largeur = texture.Width / nbColonnes;
            this.hauteur = texture.Height / nbRangees;
            this.nbImagesSequence = nbColonnes * nbRangees;
            this.milliSecParImage = milliSecParImage;
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture pour un affichage en 2 dimmensions</param>
        /// <param name="position">Position initiale du sprite</param>
        /// <param name="vitesse">Vitesse initiale de l'objet</param>        
        /// <param name="nbColonnes">Nombre de colonnes dans la sprite sheet</param>
        /// <param name="nbRangees">Nombre de rangee dans la sprite sheet</param>
        /// <param name="milliSecParImage">Délais désiré entre chaque image de la sprite sheet</param>
        public SpriteAnimer(Texture2D texture, Vector2 position, Vector2 vitesse, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(texture, position, vitesse)
        {
            this.nbColonnes = nbColonnes;
            this.nbRangees = nbRangees;
            this.largeur = texture.Width / nbColonnes;
            this.hauteur = texture.Height / nbRangees;
            this.nbImagesSequence = nbColonnes * nbRangees;
            this.milliSecParImage = milliSecParImage;
        }

        /// <summary>
        /// Met à jour le sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="borduresDeFenetre">Rectangle délimitant la région où peut se déplacer le sprite</param>
        public override void Update(GameTime gameTime)
        {
            tempsdepuisDerniereImage += gameTime.ElapsedGameTime.Milliseconds;
            if (tempsdepuisDerniereImage > milliSecParImage)
            {
                tempsdepuisDerniereImage -= milliSecParImage;
                imageEnCours++;
                if (imageEnCours == nbImagesSequence)
                {
                    imageEnCours = 0;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Dessine le sprite à l'écran lors de la mise à jour
        /// </summary>
        /// <param name="spriteBatch">Objet dessinant le sprite</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            int rangeeActuelle = imageEnCours / nbColonnes;
            int colonneActuelle = imageEnCours % nbColonnes;
            Rectangle selection = new Rectangle(colonneActuelle*largeur, rangeeActuelle*hauteur, largeur, hauteur);           
            spriteBatch.Draw(texture, Position, selection, Color.White*opacite, angleRotation, new Vector2((float)largeur/2, (float)hauteur/2), echelle, SpriteEffects.None, 0);
        }
    }
}
