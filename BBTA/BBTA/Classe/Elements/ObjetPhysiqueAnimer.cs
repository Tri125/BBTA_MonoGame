using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using BBTA.Interfaces;
using IndependentResolutionRendering;

namespace BBTA.Elements
{
    public abstract class ObjetPhysiqueAnimer:ObjetPhysique, IUtiliseMatriceCamera
    {
        protected int largeur;
        protected int hauteur;

        protected int nbColonnes;
        protected int nbRangees;
        
        protected int imageEnCours;
        protected readonly int nbImagesSequence;

        protected int milliSecParImage;
        protected int tempsdepuisDerniereImage;

        public SpriteEffects effet{get;set;}
        public Matrix MatriceDeCamera { get; set; }

        public ObjetPhysiqueAnimer(Game jeu, World mondePhysique, Shape forme, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            :base(jeu, mondePhysique, forme)
        {
            this.nbColonnes = nbColonnes;
            this.nbRangees = nbRangees;
            this.nbImagesSequence = nbColonnes * nbRangees;
            this.milliSecParImage = milliSecParImage;
            effet = SpriteEffects.None;
        }

        protected override void LoadContent()
        {
            this.largeur = texture.Width / nbColonnes;
            this.hauteur = texture.Height / nbRangees;
            base.LoadContent();
        }

        /// <summary>
        /// Met à jour le sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="borduresDeFenetre">Rectangle délimitant la région où peut se déplacer le sprite</param>
        public virtual void Update(GameTime gameTime)
        {
            Animer(gameTime, 0, nbImagesSequence);
        }

        protected void Animer(GameTime gameTime, int posDebut, int posFin)
        {
            tempsdepuisDerniereImage += gameTime.ElapsedGameTime.Milliseconds;
            if (tempsdepuisDerniereImage > milliSecParImage)
            {
                tempsdepuisDerniereImage -= milliSecParImage;
                imageEnCours++;
                if (imageEnCours == posFin)
                {
                    imageEnCours = posDebut;
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix() * MatriceDeCamera);
            int rangeeActuelle = imageEnCours / nbColonnes;
            int colonneActuelle = imageEnCours % nbColonnes;
            Rectangle selection = new Rectangle(colonneActuelle * largeur, rangeeActuelle * hauteur, largeur, hauteur);
            spriteBatch.Draw(texture, corpsPhysique.Position * 40, selection, Color.White, corpsPhysique.Rotation,
                             new Vector2(largeur / 2f, hauteur / 2f), 1, effet, 0);
            spriteBatch.End();
        }


    }
}
