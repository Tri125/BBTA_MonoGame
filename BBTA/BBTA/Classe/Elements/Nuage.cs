using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Elements
{
    public class Nuage:Sprite
    {
        protected Viewport surfaceVue;
        public Nuage(Texture2D texture, Vector2 position, Vector2 vitesse, Viewport surfaceVue)
            :base(texture, position, vitesse)
        {
            this.surfaceVue = surfaceVue;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.Position.X < -largeur)
            {
                this.Position = new Vector2(surfaceVue.Width, this.Position.Y);
            }
            else if (this.Position.X > surfaceVue.Width)
            {
                this.Position = new Vector2(-largeur, this.Position.Y);
            }
            base.Update(gameTime);
        }
    }
}
