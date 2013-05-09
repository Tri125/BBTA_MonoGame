using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using IndependentResolutionRendering;

namespace BBTA.Classe.Interface
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

        //Événements----------------------------------------------
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
                //Modifie la position du bouton en fonction du Vector2
                rectangleCollisionBouton = new Rectangle((int)value.X, (int)value.Y,
                    rectangleCollisionBouton.Width, rectangleCollisionBouton.Height);
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture servant à l'affichage du bouton</param>
        ///<param name="position">La position du bouton</param>
        /// <param name="positionDansSpriteSheet">L'endroit et l'espace occupés par les deux textures du bouton dans la SpriteSheet.</param>
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

            //On détermine la section de l'écran prise par le bouton.
            this.rectangleCollisionBouton = new Rectangle((int)position.X - boutonDansSprite.Width / 4,
                                                              (int)position.Y - boutonDansSprite.Height / 2,
                                                              (int)boutonDansSprite.Width/2, (int)boutonDansSprite.Height); 
        }

        /// <summary>
        /// Détecte s'il y a un clic.  Modifie la section de la texture affichée en conséquence.
        /// </summary>
        /// <param name="matriceCamera">Matrice de caméra si elle existe</param>
        public virtual void Update(Matrix? matriceCamera)
        {
            if (!matriceCamera.HasValue)
            {
                matriceCamera = Matrix.Identity;
            }

            //Si la souris est positionnée sur le bouton...
            if(rectangleCollisionBouton.Contains(IndependentResolutionRendering.Resolution.MouseHelper.PositionSourisCamera(matriceCamera.Value)))
            {
                //Et qu'elle relâche le clic, alors un événement indiquant un clic est déclanché.
                if (etat == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released && Clic != null)
                {
                    LancementClic();
                }
                etat = Mouse.GetState().LeftButton;
            }
        }

        /// <summary>
        /// Lance l'événement indiquant qu'un clic s'est produit.  Permet aux classes filles de déclancher l'événement.
        /// </summary>
        protected void LancementClic()
        {
            if (Clic != null)
            {
                Clic(this, new EventArgs());
            }
        }
      

        /// <summary>
        /// Affiche le bouton à l'écran.
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
    }
}
