using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Interface
{
    public class IndiquateurArmeRestante:Bouton
    {
        private SpriteFont police;
        public int nbArmeRestantes { get; set; }

        public IndiquateurArmeRestante(Texture2D texture, Vector2 position, SpriteFont police)
            : base(texture, position)
        {
            this.police = police;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(police, nbArmeRestantes.ToString(), new Vector2(Position.X + 70, Position.Y), Color.White);
        }
    }
}
