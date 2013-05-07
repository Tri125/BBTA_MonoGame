using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using BBTA.Classe.Outils;

namespace BBTA.Elements
{
    public abstract class ObjetPhysique
    {
        //Variables Farseer----------------------------------------------------------------
        protected Body corpsPhysique;
        protected Fixture joint;

        //Variables d'affichage------------------------------------------------------------
        protected Texture2D texture;
        protected float angleRotation = 0;

        //Événements-----------------------------------------------------------------------
        public event EventHandler Detruit;
      
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture de l'objet à l'écran</param>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel il évoluera</param>
        /// <param name="forme">La forme géométrique</param>
        public ObjetPhysique(Texture2D texture, World mondePhysique, Shape forme)
        {
            this.texture = texture;
            corpsPhysique = new Body(mondePhysique);
            joint = corpsPhysique.CreateFixture(forme);
        }

        /// <summary>
        /// Met à jour l'objet.  Déclanche un événement "Détruit" s'il détecte que son corps physique fut détruit par une quelconque source.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            if (corpsPhysique.IsDisposed && Detruit != null)
            {
                Detruit(this, new EventArgs());
            }
        }
        
        /// <summary>
        /// Permet de dessiner l'objet à l'écran.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2((int)Conversion.MetreAuPixel(corpsPhysique.Position.X), (int)Conversion.MetreAuPixel(corpsPhysique.Position.Y)),
                             null, Color.White, corpsPhysique.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Permet d'obtenir la position de l'objet par rapport à la coordonnée (0;0) en pixels
        /// </summary>
        /// <returns></returns>
        public Vector2 ObtenirPosition()
        {
            return Conversion.MetreAuPixel(corpsPhysique.Position);
        }

        /// <summary>
        /// Permet d'obtenir le corps physique de l'objet
        /// </summary>
        /// <returns></returns>
        public Body ObtenirCorpsPhysique()
        {
            return corpsPhysique;
        }
    }
}