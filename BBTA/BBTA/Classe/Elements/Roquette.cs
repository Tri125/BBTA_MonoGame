using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BBTA.Classe.Outils;

namespace BBTA.Classe.Elements
{
    public class Roquette : Projectile
    {
        private const int RAYON_EXPLOSION_MAX = 5;

        public Roquette(World mondePhysique, Texture2D texture, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 vitesse)
            : base(mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(Conversion.PixelAuMetre(positionSpriteSheet.Width), Conversion.PixelAuMetre(positionSpriteSheet.Height)), 1), 
                   texture, positionSpriteSheet, positionDepart, vitesse, RAYON_EXPLOSION_MAX)
        {
            corpsPhysique.FixedRotation = true;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            corpsPhysique.CollidesWith = Category.All;
            corpsPhysique.CollisionCategories = Category.All;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            corpsPhysique.Rotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X);
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            explose = true;
            return true;
        }
    }
}