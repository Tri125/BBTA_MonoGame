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
    enum EtatBouton
    {
        Attente,
        Clic
    }

    public class Bouton
    {
        private Texture2D texture;
        public Vector2 position;
        public Rectangle position2;
        private int largeur;
        private int hauteur;
        private MouseState etatAvant;
        private MouseState etatMaintenant;
        private EtatBouton etat;
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture servant à l'affichage du bouton</param>
        /// <param name="position">La position du bouton</param>
        public Bouton(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            hauteur = texture.Height;
            largeur = texture.Width / 2;
            //position2 = new Rectangle(largeur * (int)etat, 0, largeur, hauteur);
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
            //Si la sourris est dans la zone occupée par le bouton
         
            if (Resolution.MouseHelper.CurrentMousePosition.X>= position.X-largeur/2f && Resolution.MouseHelper.CurrentMousePosition.X <= position.X+largeur/2f)
            {
                if (Resolution.MouseHelper.CurrentMousePosition.Y >= position.Y - hauteur / 2f && Resolution.MouseHelper.CurrentMousePosition.Y <= position.Y + hauteur / 2f)
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
            }
            return false;
        }

        /// <summary>
        /// Affiche le bouton à l'écran selon la présence de clic ou non.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Si l'état est "Attente", alors la texture du bouton est standard. Autrement, la texture du bouton est celle indiquant le clic.
            spriteBatch.Draw(texture, position, new Rectangle(largeur*(int)etat, 0, largeur, hauteur), Color.White, 0, new Vector2(largeur/2f, hauteur/2f), 1, SpriteEffects.None, 0);
        }        
    }
}
