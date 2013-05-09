using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using IndependentResolutionRendering;

namespace BBTA.Interface
{
    /// <summary>
    /// Bouton standard avec textures personnalisées sur lequel on peut cliquer. 
    /// </summary>
    public class Bouton
    {
        //Variables------------------------------------------------
        protected Texture2D texture;
        protected Rectangle boutonDansSprite;
        protected Rectangle rectangleCollisionBouton;
        protected ButtonState etat = ButtonState.Released;
        public event EventHandler Clic;

        //Propriétés----------------------------------------------
        public Vector2 Position
        {
            get
            {
                //Transforme la position du rectangle en Vecteur
                return new Vector2(rectangleCollisionBouton.X, rectangleCollisionBouton.Y);
            }

            set
            {
                //'Modifie la position du bouton en fonction du Vector2
                rectangleCollisionBouton = new Rectangle((int)value.X, (int)value.Y,
                    rectangleCollisionBouton.Width, rectangleCollisionBouton.Height);
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture servant à l'affichage du bouton</param>
        /// <param name="positionDansSpriteSheet">
        /// L'endroit et l'espace occupés par les deux textures du bouton dans la SpriteSheet.
        /// </param>
        /// <param name="position">La position du bouton</param>
        public Bouton(Texture2D texture, Vector2 position, Rectangle? positionDansSpriteSheet)
        {
            this.texture = texture;
            if (positionDansSpriteSheet.HasValue)
            {
                boutonDansSprite = positionDansSpriteSheet.Value;
            }
            else
            {
                boutonDansSprite = new Rectangle(0, 0, texture.Width, texture.Height);
            }

            this.rectangleCollisionBouton = new Rectangle((int)position.X - boutonDansSprite.Width / 4,
                                                              (int)position.Y - boutonDansSprite.Height / 2,
                                                              (int)boutonDansSprite.Width/2, (int)boutonDansSprite.Height); 
        }

        public virtual void Update(Matrix? matriceCamera)
        {
            if (!matriceCamera.HasValue)
            {
                matriceCamera = Matrix.Identity;
            }

            Point positionSouris = IndependentResolutionRendering.Resolution.MouseHelper.PositionSourisCamera(matriceCamera.Value);

            if(rectangleCollisionBouton.Contains(positionSouris))
            {
                if (etat == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released && Clic != null)
                {
                    LancementClic();
                }
                etat = Mouse.GetState().LeftButton;
            }
        }
      

        /// <summary>
        /// Affiche le bouton à l'écran selon la présence de clic ou non.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Si l'état est "Attente", alors la texture du bouton est standard. Autrement, la texture du bouton est celle indiquant le clic.
            Rectangle selection = new Rectangle((int)boutonDansSprite.X + (int)etat * rectangleCollisionBouton.Width, 
                                                (int)boutonDansSprite.Y, 
                                                (int)rectangleCollisionBouton.Width, 
                                                (int)rectangleCollisionBouton.Height);
            spriteBatch.Draw(texture, rectangleCollisionBouton, selection, Color.White);
        }

        protected void LancementClic()
        {
            Clic(this, new EventArgs());
        }
    }
}
