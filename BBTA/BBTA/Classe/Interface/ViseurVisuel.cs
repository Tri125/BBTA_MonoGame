using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BBTA.Elements;

namespace BBTA.Interface
{
    public class ViseurVisuel
    {
        private Texture2D texture;
        public Vector2 positionJoueur;
        public float angleRotation{get;set;}
        public bool estVerouiller = false;

        public ViseurVisuel(Texture2D texture)
        {
            this.texture = texture;
        }

        public void AssocierAujoueur(Acteur joueur)
        {
            positionJoueur = joueur.ObtenirPosition();
        }

        public void Verrouiller()
        {
            estVerouiller = true;
        }

        public void Deverouiller()
        {
            estVerouiller = false;
        }

        public void Update(GameTime gameTime, Point positionSouris)
        {
            angleRotation = (float)Math.Atan2(positionSouris.Y - positionJoueur.Y, positionSouris.X - positionJoueur.X);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionJoueur, null, Color.White, angleRotation, new Vector2(texture.Width/2f, texture.Height/2f), 1, SpriteEffects.None, 0);
        }
    }
}
