﻿using System;
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

namespace BBTA.Elements
{
    public class Roquette:Projectile
    {
        const float ENERGIE_EXPLOSION = 500f;

        public Roquette(World mondePhysique, Vector2 positionDepart, Vector2 direction, Texture2D texture)
            : base(mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(texture.Width/80f, texture.Height/80f), 1), positionDepart, texture, ENERGIE_EXPLOSION)
        {
            direction.Normalize();
            corpsPhysique.ApplyLinearImpulse(new Vector2(10*direction.X, 20*direction.Y));
            corpsPhysique.FixedRotation = true;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Explose = true;
            return true;
        }
    }
}
