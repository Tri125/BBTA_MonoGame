using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Classe.Elements
{
    /// <summary>
    /// Nuage est un sprite qui traverse l'écran d'un bout à l'autre et qui, une fois cela fait, est replacé à l'extrémité gauche pour recommencer à nouveau
    /// </summary>
    public class Nuage:Sprite
    {
        //Taille de l'espace que prend le joueur------------------------------------------------------------------------------------------------------------
        protected Viewport surfaceVue;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture du nuage</param>
        /// <param name="position">Position initiale du nuage</param>
        /// <param name="vitesse">Vitesse du nuage.</param>
        /// <param name="surfaceVue">Taille de l'espace que prend le joueur</param>
        public Nuage(Texture2D texture, Vector2 position, Vector2 vitesse, Viewport surfaceVue)
            :base(texture, position, vitesse)
        {
            this.surfaceVue = surfaceVue;
        }

        /// <summary>
        /// Met à jour la position du nuage. Le repositionne à la gauche s'il dépasse entièrement l'extrémité droite.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Gère le repositionnement si la vitesse est dirigée vers la gauche
            if (this.Position.X < -largeur)
            {
                this.Position = new Vector2(surfaceVue.Width, this.Position.Y);
            }
            //Gère le repositionnement si la vitesse est dirigée vers la droite
            else if (this.Position.X > surfaceVue.Width)
            {
                this.Position = new Vector2(-largeur, this.Position.Y);
            }
            base.Update(gameTime);
        }
    }
}
