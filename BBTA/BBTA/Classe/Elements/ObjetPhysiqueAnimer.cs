using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using BBTA.Classe.Outils;

namespace BBTA.Classe.Elements
{
    public abstract class ObjetPhysiqueAnimer : ObjetPhysique
    {
        //Variables reliées à l'affichage de la bonne image dans la SpriteSheet---------------------------------------
        protected int largeur; //Largeur d'une image individuelle
        protected int hauteur; //Hauteur d'une image individuelle
        protected int nbColonnes; //Nombres d'images sur l'axe horizontal
        protected int nbRangees; //Nombre d'images sur l'axe vertical
        protected int imageEnCours; //Image présentement affichée dans la spritesheet
        protected readonly int nbImagesSequence; //Nombre d'images contenues dans la séquence à afficher en boucle.
        public SpriteEffects effet { get; set; }

        //Variables servant au délai entre l'affichage de chaque image------------------------------------------------
        protected int milliSecParImage;
        protected int tempsdepuisDerniereImage;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel l'objet évoluera</param>
        /// <param name="forme">La forme géométrique</param>
        /// <param name="texture">La spritesheet de l'objet</param>
        /// <param name="nbColonnes">Le nombre d'images sur l'axe horizontal</param>
        /// <param name="nbRangees">Le nombre d'images sur l'axe vertical</param>
        /// <param name="milliSecParImage">Délai entre chaque image (en millisecondes)</param>
        public ObjetPhysiqueAnimer(World mondePhysique, Shape forme, Texture2D texture, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(texture, mondePhysique, forme)
        {
            this.largeur = texture.Width / nbColonnes;
            this.hauteur = texture.Height / nbRangees;
            this.nbColonnes = nbColonnes;
            this.nbRangees = nbRangees;
            this.nbImagesSequence = nbColonnes * nbRangees;
            this.milliSecParImage = milliSecParImage;
            effet = SpriteEffects.None;
        }

        /// <summary>
        /// Permet d'animer l'objet en modifiant l'image de la spritesheet qui est affichée.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        /// <param name="posDebut">Index de l'image initiale de la séquence (1 étant le premier index)</param>
        /// <param name="posFin">Index de l'image finale de la séquence</param>
        protected void Animer(GameTime gameTime, int posDebut, int posFin)
        {
            tempsdepuisDerniereImage += gameTime.ElapsedGameTime.Milliseconds;
            //Si le délai pour l'affichage est expiré, on change l'image affichée.
            if (tempsdepuisDerniereImage > milliSecParImage)
            {
                tempsdepuisDerniereImage -= milliSecParImage;
                imageEnCours++;
                //Si c'est la dernière image de la séquence, on se repositionne à la première
                if (imageEnCours == posFin)
                {
                    imageEnCours = posDebut;
                }
            }
        }

        /// <summary>
        /// Dessine le sprite à l'écran lors de la mise à jour
        /// </summary>
        /// <param name="spriteBatch">Objet dessinant le sprite</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //À partir des informations sur l'image actuelles et sur la taille de la spritesheet, on se positionne pour afficher la bonne portion de la spritesheet.
            int rangeeActuelle = imageEnCours / nbColonnes;
            int colonneActuelle = imageEnCours % nbColonnes;
            Rectangle selection = new Rectangle(colonneActuelle * largeur, rangeeActuelle * hauteur, largeur, hauteur);
            //Omn dessine le tout.
            spriteBatch.Draw(texture, new Vector2((int)Conversion.MetreAuPixel(corpsPhysique.Position.X), (int)Conversion.MetreAuPixel(corpsPhysique.Position.Y)),
                             selection, Color.White, corpsPhysique.Rotation, new Vector2(largeur / 2, hauteur /2), 1, effet, 0);
        }
    }
}