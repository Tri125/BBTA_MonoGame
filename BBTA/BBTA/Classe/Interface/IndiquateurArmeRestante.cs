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
        private Armes type;

        public IndiquateurArmeRestante(Texture2D texture, Rectangle? boutonDansSpriteSheet, Vector2 position, Armes type, SpriteFont police)
            : base(texture, position, boutonDansSpriteSheet)
        {
            this.type = type;
            this.police = police;
        }

        public Armes ObtenirType()
        {
            return type;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(police, nbArmeRestantes.ToString(), new Vector2(Position.X + 65, Position.Y), Color.White);
        }
    }
}
