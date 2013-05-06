using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Elements;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using System.Timers;
using BBTA.Classe.Outils;
using FarseerPhysics.Collision;

namespace BBTA.Elements
{
    public class Mine:Projectile
    {
        public const int RAYON_EXPLOSION = 4;
        private int tempsDepuisLumierePrecedente = 0;
        private const int DELAI_LUMIERE = 200;
        private World mondePhysique;
        private Timer compteRebours = new Timer(200);
        public event EventHandler VitesseNulle;

        public Mine(ref World mondePhysique, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 direction, float vitesse, Texture2D texture)
            : base(mondePhysique, new CircleShape(Conversion.PixelAuMetre(7), 5), positionSpriteSheet, positionDepart, texture, RAYON_EXPLOSION)

        {
            this.mondePhysique = mondePhysique;
            corpsPhysique.ApplyLinearImpulse(direction * vitesse);
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            compteRebours.Elapsed += new ElapsedEventHandler(compteRebours_Elapsed);
        }

        void compteRebours_Elapsed(object sender, ElapsedEventArgs e)
        {
            explose = true;
        }

        public override void Update(GameTime gameTime)
        {
            tempsDepuisLumierePrecedente += gameTime.ElapsedGameTime.Milliseconds;
            if (tempsDepuisLumierePrecedente > DELAI_LUMIERE)
            {
                tempsDepuisLumierePrecedente -= DELAI_LUMIERE;
                if (positionSpriteSheet.X == 3)
                {
                    positionSpriteSheet.Offset(texture.Width/2, 0);
                }
                else
                {
                    positionSpriteSheet.Offset(-texture.Width / 2, 0);
                }
            }
            if (corpsPhysique.LinearVelocity != Vector2.Zero)
            {
                angleRotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X);
            }

            if (corpsPhysique.IgnoreGravity)
            {
                AABB detectionAutourMine = new AABB(corpsPhysique.Position - new Vector2(1.5f), corpsPhysique.Position + new Vector2(1.5f));
                bool objetRencontrer = false;
                mondePhysique.QueryAABB(Fixture =>
                                        {
                                            if (Fixture.Body.BodyType == BodyType.Dynamic && Fixture.Body != corpsPhysique)
                                            {
                                                compteRebours.Start();
                                                return false;
                                            }
                                            else
                                            {
                                                objetRencontrer = true;
                                                return true;
                                            }
                                        },
                                        ref detectionAutourMine);

            }

            if (corpsPhysique.LinearVelocity.Length() == 0 && VitesseNulle != null && compteRebours.Enabled == false)
            {
                VitesseNulle(this, new EventArgs());
                VitesseNulle = null;
            }

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ObtenirPosition(), positionSpriteSheet,
                             Color.White, angleRotation, new Vector2(positionSpriteSheet.Width / 2, positionSpriteSheet.Height / 2), 1, retourner, 0);
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            corpsPhysique.IgnoreGravity = true;
            corpsPhysique.LinearVelocity = Vector2.Zero;
            if (contact.Manifold.LocalPoint.Y < 0)
            {
                angleRotation = MathHelper.PiOver2 * 3;
            }
            else if (contact.Manifold.LocalPoint.Y > 0)
            {
                angleRotation = MathHelper.PiOver2;
            }
            else if (contact.Manifold.LocalPoint.X > 0)
            {
                angleRotation = 0;
            }
            else
            {
                angleRotation = MathHelper.Pi;
            }
            return true;
        }
    }
}
