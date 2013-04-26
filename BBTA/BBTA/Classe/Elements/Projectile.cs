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
        private readonly int rayonExplosion;
        protected bool explose = false;
        private World mondePhysique;
        protected bool EstEnMain = true;
        Rectangle positionSpriteSheet;
        public delegate void DelegateExplosion(Vector2 position, int rayonExplosion);
        public event DelegateExplosion Explosion;

        public Projectile(World mondePhysique, Shape forme, Rectangle positionSpriteSheet, Vector2 positionDepart, Texture2D texture, int rayonExplosion)
            : base(texture, mondePhysique, forme)
        {
            this.mondePhysique = mondePhysique;
            this.rayonExplosion = rayonExplosion;
            this.positionSpriteSheet = positionSpriteSheet;
            corpsPhysique.IsBullet = true;
            corpsPhysique.Position = positionDepart;
            corpsPhysique.BodyType = BodyType.Dynamic;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (explose == true && Explosion != null)
            {
                Explosion(Conversion.MetreAuPixel(corpsPhysique.Position), Conversion.MetreAuPixel(rayonExplosion));
                corpsPhysique.Dispose();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Conversion.MetreAuPixel(corpsPhysique.Position), positionSpriteSheet, Color.White,
                             angleRotation, new Vector2(positionSpriteSheet.Width / 2, positionSpriteSheet.Height / 2), 1, SpriteEffects.None, 0);
        }

    }
}