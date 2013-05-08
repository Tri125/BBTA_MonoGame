using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Elements;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using BBTA.Classe.Outils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using System.Timers;

namespace BBTA.Classe.Elements
{
    /// <summary>
    /// Une grande dans BBTA est un dispositif explosif qui explose après un certain délai.
    /// </summary>
    public class Grenade:Projectile
    {
        //Constantes----------------------------------------------------------------------------------
        public const int RAYON_EXPLOSION = 4;

        //Variables reliées au processus d'explosion de la grenade------------------------------------
        private Timer compteReboursExplosion = new Timer(4000);
        private bool estAusol = false;

        public Grenade(World mondePhysique, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 direction, float vitesse, Texture2D texture)
            : base(mondePhysique, new CircleShape(Conversion.PixelAuMetre(9), 5), positionSpriteSheet, positionDepart, texture, RAYON_EXPLOSION)
        {
            corpsPhysique.ApplyLinearImpulse(direction * vitesse);
            corpsPhysique.Restitution = 0.5f;
            corpsPhysique.Rotation = Conversion.ValeurAngle(direction);
            compteReboursExplosion.Start();
            compteReboursExplosion.Elapsed += new ElapsedEventHandler(compteReboursExplosion_Elapsed);
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            corpsPhysique.OnSeparation += new OnSeparationEventHandler(corpsPhysique_OnSeparation);
        }

        void corpsPhysique_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            estAusol = false;
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            estAusol = true;
            corpsPhysique.ApplyTorque(1);
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (corpsPhysique.Rotation >= MathHelper.TwoPi)
            {
                corpsPhysique.Rotation %= MathHelper.TwoPi;
            }
            while (corpsPhysique.Rotation < 0)
            {
                corpsPhysique.Rotation += MathHelper.TwoPi;
            }
            if (corpsPhysique.Rotation > MathHelper.PiOver2 && corpsPhysique.Rotation < MathHelper.PiOver2 * 3)
            {
                retourner = SpriteEffects.FlipVertically;
            }
            if (estAusol)
            {
                corpsPhysique.LinearDamping = 2;
            }
            angleRotation = corpsPhysique.Rotation;
        }

        void compteReboursExplosion_Elapsed(object sender, ElapsedEventArgs e)
        {
            explose = true;
        }
    }
}
