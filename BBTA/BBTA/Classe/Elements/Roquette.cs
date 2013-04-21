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

namespace BBTA.Elements
{
    public class Roquette:Projectile
    {
        const float ENERGIE_EXPLOSION = 500f;

        public Roquette(Game jeu, World mondePhysique, Vector2 positionDepart, Vector2 direction)
            : base(jeu, mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(10, 10), 1), positionDepart, ENERGIE_EXPLOSION)
        {
            direction.Normalize();
            corpsPhysique.ApplyLinearImpulse(new Vector2(10*direction.X, 20*direction.Y));
            corpsPhysique.FixedRotation = true;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>(@"Ressources\Acteur\ActeurBleu");
            base.LoadContent();
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Explose = true;
            return true;
        }
    }
}
