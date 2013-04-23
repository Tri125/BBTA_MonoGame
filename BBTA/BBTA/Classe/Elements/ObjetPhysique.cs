using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;

namespace BBTA.Elements
{
    public abstract class ObjetPhysique
    {
        protected Body corpsPhysique;
        protected Texture2D texture;
        private Fixture joint;

        public ObjetPhysique(Texture2D texture, World mondePhysique, Shape forme)
        {
            this.texture = texture;
            corpsPhysique = new Body(mondePhysique);
            joint = corpsPhysique.CreateFixture(forme);
        }

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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 uniteXna = corpsPhysique.Position * 40;
            spriteBatch.Draw(texture, uniteXna, null, Color.White, corpsPhysique.Rotation,
                new Vector2(texture.Width / 2f, texture.Height / 2f), 1, SpriteEffects.None, 0);
        }
    }
}