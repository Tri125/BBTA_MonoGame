using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Interface
{
    public class IndicateurArmeRestante:Bouton
    {
        private SpriteFont police;
        public int nbArmeRestantes { get; set; }
        private Armes type;

        public IndicateurArmeRestante(Texture2D texture, Rectangle? boutonDansSpriteSheet, Vector2 position, Armes type, SpriteFont police)
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
            StringBuilder nbArmes = new StringBuilder(nbArmeRestantes.ToString());
            if (nbArmeRestantes < 10)
            {
                nbArmes.Insert(0, "0");
            }
            spriteBatch.DrawString(police, nbArmes.ToString(), new Vector2(Position.X + 60, Position.Y+5), Color.White);
        }
    }
}
