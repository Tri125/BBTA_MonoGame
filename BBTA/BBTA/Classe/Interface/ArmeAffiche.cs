using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Classe.Elements;

namespace BBTA.Classe.Interface
{
    public class ArmeAffiche:MenuDeployable
    {
        private Armes type;
        public float angleRotation { get; set; }
        SpriteEffects tournerLaTexture = SpriteEffects.None;

        public ArmeAffiche(Texture2D texture, Armes type):base(texture, new Rectangle(0, (int)type*30, texture.Width, 30), 200)
        {
            this.type = type;
        }

        public override void Update(GameTime gameTime)
        {
            this.angleRotation = angleRotation;

            if (Math.Abs(angleRotation % MathHelper.TwoPi) > MathHelper.PiOver2 && Math.Abs(angleRotation % (2 * Math.PI)) < MathHelper.PiOver2 * 3)
            {
                tournerLaTexture = SpriteEffects.FlipVertically;
            }
            else
            {
                tournerLaTexture = SpriteEffects.None;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturePanneau, Position, new Rectangle(0, (int)type * 30, texturePanneau.Width, 30), 
                             Color.White * progressionDeploiement, angleRotation, new Vector2(texturePanneau.Width / 2, 15), progressionDeploiement, tournerLaTexture, 0);
        }

    }
}
