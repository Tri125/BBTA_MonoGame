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

namespace BBTA.Elements
{
    public class Roquette:Projectile
    {
        const float ENERGIE_EXPLOSION = 500f;

        public Roquette(World mondePhysique, Vector2 vitesseDepart, Vector2 positionDepart, Texture2D texture)
            : base(mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(texture.Width, texture.Height), 1),
                  vitesseDepart, positionDepart, texture, ENERGIE_EXPLOSION)
        {
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Explose = true;
            return true;
        }
    }
}
