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
        private Texture2D textureArmes;
        public Armes armeChoisie { get; set; }
        private float angleRotation;
        private SpriteEffects effet;
        public event EventHandler Verouiller;

        public ViseurVisuel(Texture2D texture, Texture2D textureArmes):base(texture, null, 200)
        {
            this.texture = texture;
            this.textureArmes = textureArmes;
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

            if (Math.Abs(angleRotation % MathHelper.TwoPi) > MathHelper.PiOver2 && Math.Abs(angleRotation % (2 * Math.PI)) < MathHelper.PiOver2 * 3)
            {
                effet = SpriteEffects.FlipVertically;
            }
            else
            {
                effet = SpriteEffects.None;
            }

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
            spriteBatch.Draw(textureArmes, new Vector2(Position.X, Position.Y+5), new Rectangle(0, (int)armeChoisie * 68, textureArmes.Width, 30), Color.White, 
                             angleRotation, new Vector2(textureArmes.Width/2, textureArmes.Height/2), progressionDeploiement, effet, 0);
        }

        public Vector2 ObtenirAngle()
        {
            Vector2 angle = new Vector2((float)Math.Cos(angleRotation), (float)Math.Sin(angleRotation));
            angle.Normalize();
            return angle;
        }
    }
}
