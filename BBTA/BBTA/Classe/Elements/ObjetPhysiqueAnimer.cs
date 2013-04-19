﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace BBTA.Elements
{
    public abstract class ObjetPhysiqueAnimer:ObjetPhysique
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

        public ObjetPhysiqueAnimer(World mondePhysique, Shape forme, Texture2D texture, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            :base(texture, mondePhysique, forme)
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

        /// <summary>
        /// Dessine le sprite à l'écran lors de la mise à jour
        /// </summary>
        /// <param name="spriteBatch">Objet dessinant le sprite</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            int rangeeActuelle = imageEnCours / nbColonnes;
            int colonneActuelle = imageEnCours % nbColonnes;
            Rectangle selection = new Rectangle(colonneActuelle*largeur, rangeeActuelle*hauteur, largeur, hauteur);           
            spriteBatch.Draw(texture, corpsPhysique.Position*40, selection, Color.White, corpsPhysique.Rotation, 
                             new Vector2(largeur/2f, hauteur/2f), 1, effet, 0);
        }


    }
}