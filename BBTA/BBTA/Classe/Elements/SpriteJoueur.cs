using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Elements
{
    public class JoueurHumain : Acteur
    {
        KeyboardState clavierAvant;
        KeyboardState clavierMaintenant;
        private bool estAuSol = true;

        public JoueurHumain(World mondePhysique, Texture2D texture, Vector2 position, float pointsVie, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(mondePhysique, pointsVie, texture, position, nbColonnes, nbRangees, milliSecParImage)
        {
        }

        public override void Update(GameTime gameTime)
        {
            clavierAvant = clavierMaintenant;
            clavierMaintenant = Keyboard.GetState();
            corpsPhysique.LinearVelocity = new Vector2(0, corpsPhysique.LinearVelocity.Y);


            if (clavierMaintenant.IsKeyDown(Keys.D))
            {
                corpsPhysique.LinearVelocity = new Vector2(corpsPhysique.LinearVelocity.X + 5f, corpsPhysique.LinearVelocity.Y);
            }

            if (clavierMaintenant.IsKeyDown(Keys.A))
            {
                corpsPhysique.LinearVelocity = new Vector2(corpsPhysique.LinearVelocity.X - 5f, corpsPhysique.LinearVelocity.Y);
            }

            if (clavierMaintenant.IsKeyDown(Keys.Space) && !clavierAvant.IsKeyDown(Keys.Space) && estAuSol == true)
            {
                estAuSol = false;
                corpsPhysique.ApplyLinearImpulse(new Vector2(0,-6));
                corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            }
            base.Update(gameTime);
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
             if (contact.Manifold.LocalPoint.Y == -0.5f && contact.Manifold.LocalPoint.X == 0)
            {
                estAuSol = true;
            }
            return true;
        }

        

    }
}
