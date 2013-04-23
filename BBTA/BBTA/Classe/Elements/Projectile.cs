using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using BBTA.Partie_De_Jeu;
using BBTA.Classe.Outils;

namespace BBTA.Elements
{
    public abstract class Projectile : ObjetPhysique
    {
        private float energieExplosion;
        protected bool explose = false;
        private World mondePhysique;
        protected bool EstEnMain = true;
        Rectangle positionSpriteSheet;
        public delegate void DelegateExplosion(Vector2 position, float energieExplosion);
        public event DelegateExplosion Explosion;

        public Projectile(World mondePhysique, Shape forme, Rectangle positionSpriteSheet, Vector2 positionDepart, Texture2D texture, float energieExplosion)
            : base(texture, mondePhysique, forme)
        {
            this.mondePhysique = mondePhysique;
            this.energieExplosion = energieExplosion;
            this.positionSpriteSheet = positionSpriteSheet;
            corpsPhysique.IsBullet = true;
            corpsPhysique.Position = positionDepart;
            corpsPhysique.BodyType = BodyType.Dynamic;
        }

        public virtual void Update(GameTime gameTime)
        {
            corpsPhysique.Rotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X);
            if (explose == true && Explosion != null)
            {
                Explosion(Conversion.MetreAuPixel(corpsPhysique.Position), energieExplosion);
                corpsPhysique.Dispose();
            }
        }

        public Vector2 ObtenirPosition()
        {
            return Conversion.MetreAuPixel(corpsPhysique.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Conversion.MetreAuPixel(corpsPhysique.Position), positionSpriteSheet, Color.White,
                             AngleRotation, new Vector2(positionSpriteSheet.Width / 2, positionSpriteSheet.Height / 2), 1, SpriteEffects.None, 0);
        }

    }
}