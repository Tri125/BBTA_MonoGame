using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BBTA.Elements;
using BBTA.Classe.Option;

namespace BBTA.Interface
{
    public class ViseurVisuel:MenuDeployable
    {
        private Texture2D texture;
        public Armes armeChoisie { get; set; }
        private float angleRotation;
        public event EventHandler Verouiller;

        public ViseurVisuel(Texture2D texture):base(texture, null, 200)
        {
            this.texture = texture;
            angleRotation = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState clavier = Keyboard.GetState();
            if (clavier.IsKeyDown(Game1.chargeurOption.OptionActive.InformationTouche.Gauche))
            {
                angleRotation += MathHelper.ToRadians(3);
            }
            else if (clavier.IsKeyDown(Game1.chargeurOption.OptionActive.InformationTouche.Droite))
            {
                angleRotation -= MathHelper.ToRadians(3);
            }

            angleRotation %= MathHelper.TwoPi;

            if (clavier.IsKeyDown(Keys.Space))
            {
                if (Verouiller != null)
                {
                    Verouiller(this, new EventArgs());
                }
                estOuvert = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White * progressionDeploiement, angleRotation, new Vector2(texture.Width / 2f, texture.Height / 2f), progressionDeploiement, SpriteEffects.None, 0);
        }

        public Vector2 ObtenirAngle()
        {
            Vector2 angle = new Vector2((float)Math.Cos(angleRotation), (float)Math.Sin(angleRotation));
            angle.Normalize();
            return angle;
        }
    }
}
