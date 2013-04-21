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
    public abstract class ObjetPhysique:DrawableGameComponent
    {
        protected Body corpsPhysique;
        private Fixture joint;
        protected Texture2D texture;
        protected SpriteBatch spriteBatch;

        public float AngleRotation
        {
            get
            {
                return corpsPhysique.Rotation;
            }

            set
            {
                corpsPhysique.Rotation = value;
            }
        }

        public ObjetPhysique(Game jeu, World mondePhysique, Shape forme)
            :base(jeu)
        {
            corpsPhysique = new Body(mondePhysique);
            joint = corpsPhysique.CreateFixture(forme);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Conversion.MetreAuPixel(corpsPhysique.Position), null, Color.White, corpsPhysique.Rotation,
                new Vector2(texture.Width / 2f, texture.Height / 2f), 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
