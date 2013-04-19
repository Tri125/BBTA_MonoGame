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
    public enum EtatBouton
    {
        Attente,
        Clic
    }

    public class Bouton
    {
        protected Texture2D texture;
        private Rectangle rectangleCollisionBouton;
        public Vector2 Position
        {
            get
            {
                return new Vector2(rectangleCollisionBouton.X, rectangleCollisionBouton.Y);
            }

            set
            {
                rectangleCollisionBouton = new Rectangle((int)value.X, (int)value.Y, rectangleCollisionBouton.Width, rectangleCollisionBouton.Height);
            }
        }

        private MouseState etatAvant;
        private MouseState etatMaintenant;
        protected EtatBouton etat;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture servant à l'affichage du bouton</param>
        /// <param name="position">La position du bouton</param>
        public Bouton(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            int largeurUnique = texture.Width / 4;
            int hauteurUnique = texture.Height / 2;
            this.rectangleCollisionBouton = new Rectangle((int)position.X-largeurUnique, (int)position.Y - hauteurUnique, (int)texture.Width / 2, (int)texture.Height);
        }

        /// <summary>
        /// Indique si un clic réglementaire fut fait sur le bouton.
        /// Un tel clic correspond à un clic suivit d'un relâchement dans la zone occupée par le bouton
        /// </summary>
        /// <returns>Si clic il y eu</returns>
        public bool ClicComplet(Matrix matriceCamera)
        {
            etat = EtatBouton.Attente;
            etatAvant = etatMaintenant;
            etatMaintenant = Mouse.GetState();
            //Si la sourris est dans la zone occupée par le bouton
            Point souris = IndependentResolutionRendering.Resolution.MouseHelper.PositionSourisCamera(matriceCamera);
            Console.WriteLine(rectangleCollisionBouton);
            Console.WriteLine(souris);

            if(rectangleCollisionBouton.Contains(souris))
            {
                //Si clic il y a, mais sans relâchement, alors la texture servant à indiquer un clic sera celle affichée
                if (etatMaintenant.LeftButton == ButtonState.Pressed)
                {
                    etat = EtatBouton.Clic;
                }
                //S'il y a relâchement, alors on le communique 
                else if (etatMaintenant.LeftButton == ButtonState.Released && etatAvant.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indique si un clic réglementaire fut fait sur le bouton.
        /// Un tel clic correspond à un clic suivit d'un relâchement dans la zone occupée par le bouton
        /// </summary>
        /// <returns>Si clic il y eu</returns>
        public bool ClicComplet()
        {         
            etat = EtatBouton.Attente;
            etatAvant = etatMaintenant;
            etatMaintenant = Mouse.GetState();
            Point souris = new Point((int)IndependentResolutionRendering.Resolution.MouseHelper.CurrentMousePosition.X,
                                                              (int)IndependentResolutionRendering.Resolution.MouseHelper.CurrentMousePosition.Y);
            //Si la sourris est dans la zone occupée par le bouton

            if (rectangleCollisionBouton.Contains(souris))
            {
                //Si clic il y a, mais sans relâchement, alors la texture servant à indiquer un clic sera celle affichée

                if (etatMaintenant.LeftButton == ButtonState.Pressed)
                {
                    etat = EtatBouton.Clic;
                }
                //S'il y a relâchement, alors on le communique 
                else if (etatMaintenant.LeftButton == ButtonState.Released && etatAvant.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Affiche le bouton à l'écran selon la présence de clic ou non.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Si l'état est "Attente", alors la texture du bouton est standard. Autrement, la texture du bouton est celle indiquant le clic.
            Rectangle selection = new Rectangle((int)etat * rectangleCollisionBouton.Width, 0, rectangleCollisionBouton.Width, rectangleCollisionBouton.Height);
            spriteBatch.Draw(texture, rectangleCollisionBouton, selection, Color.White);
        }        
    }
}
