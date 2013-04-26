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
    public class Grenade:Projectile
    {
        public const int RAYON_EXPLOSION = 4;
        private Timer compteReboursExplosion = new Timer(5000);

        public Grenade(World mondePhysique, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 direction, float vitesse, Texture2D texture)
            : base(mondePhysique, new CircleShape(Conversion.PixelAuMetre(9), 5), positionSpriteSheet, positionDepart, texture, RAYON_EXPLOSION)
        {
            corpsPhysique.ApplyLinearImpulse(direction * vitesse);
            corpsPhysique.Restitution = 0.5f;
            corpsPhysique.Friction = 0.5f;
            compteReboursExplosion.Start();
            compteReboursExplosion.Elapsed += new ElapsedEventHandler(compteReboursExplosion_Elapsed);
        }

        void compteReboursExplosion_Elapsed(object sender, ElapsedEventArgs e)
        {
            explose = true;
        }
    }
}
