﻿using System;
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
    public abstract class ObjetPhysique
    {
        protected Body corpsPhysique;
        protected Texture2D texture;
        protected Fixture joint;
        protected float angleRotation = 0;
        public event EventHandler Detruit;
      

        public ObjetPhysique(Texture2D texture, World mondePhysique, Shape forme)
        {
            this.texture = texture;
            corpsPhysique = new Body(mondePhysique);
            joint = corpsPhysique.CreateFixture(forme);
        }

        public Vector2 ObtenirPosition()
        {
            return Conversion.MetreAuPixel(corpsPhysique.Position);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (corpsPhysique.IsDisposed && Detruit != null)
            {
                Detruit(this, new EventArgs());
            }
        }
        

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2((int)Conversion.MetreAuPixel(corpsPhysique.Position.X), (int)Conversion.MetreAuPixel(corpsPhysique.Position.Y)),
                             null, Color.White, corpsPhysique.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }

        public Body ObtenirCorpsPhysique()
        {
            return corpsPhysique;
        }
    }
}