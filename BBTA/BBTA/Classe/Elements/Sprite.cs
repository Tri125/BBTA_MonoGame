using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTA.Elements
{
    public class Sprite
    {
        public Vector2 Position { get; protected set; }
        protected Texture2D texture;
        public Vector2 Vitesse { get; set; }       
        protected float echelle = 1; //Grandeur du sprite par rapport à la texture originale        
        protected float angleRotation = 0;
        protected int largeur; //Largueur d'une seule image de la sprite sheet
        protected int hauteur; //Hauteur d'une seule image de la sprite sheet
        protected float opacite = 1;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture pour un affichage en 2 dimmensions</param>
        /// <param name="position">Position initiale du sprite</param>
        /// <param name="vitesse">Vitesse initiale de l'objet</param>
        /// <param name="">Vitesse initiale de l'objet</param>
        protected Sprite(Texture2D texture)
        {
            this.texture = texture;           
            this.largeur = texture.Width;
            this.hauteur = texture.Height;
            
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture pour un affichage en 2 dimmensions</param>
        /// <param name="position">Position initiale du sprite</param>
        /// <param name="vitesse">Vitesse initiale de l'objet</param>
        /// <param name="">Vitesse initiale de l'objet</param>
        public Sprite(Texture2D texture, Vector2 position):this(texture)
        {
            this.Position = position;                 
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture pour un affichage en 2 dimmensions</param>
        /// <param name="position">Position initiale du sprite</param>
        /// <param name="vitesse">Vitesse initiale de l'objet</param>
        /// <param name="">Vitesse initiale de l'objet</param>
        public Sprite(Texture2D texture, Vector2 position, Vector2 vitesse):this(texture, position)
        {
            this.Vitesse = vitesse;
        }       

        /// <summary>
        /// Met à jour le sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="limitesDeMouvements">Rectangle délimitant la région où peut se déplacer le sprite</param>
        public virtual void Update(GameTime gameTime)
        {
            this.Position += Vitesse;            
        }

        /// <summary>
        /// Dessine le sprite à l'écran lors de la mise à jour
        /// </summary>
        /// <param name="spriteBatch">Objet dessinant le sprite</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White*opacite, angleRotation, Vector2.Zero, echelle, SpriteEffects.None, 0);
        }
    }
}
