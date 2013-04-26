using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interfaces;
using BBTA.Interface;
using BBTA.Elements;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Classe.Outils;
using BBTA.Classe.Elements;

namespace BBTA.Classe
{
    public class GestionnaireProjectile:DrawableGameComponent, IUtiliseMatriceCamera
    {
        public Matrix MatriceDeCamera { get; set; }
        public Vector2 Position { get; private set; }
        private Texture2D texturesProjectiles;
        private Projectile projectile;
        private SpriteBatch spriteBatch;
        public delegate void DelegateExplosion(Vector2 position, int rayonExplosion);
        public event DelegateExplosion Explosion;

        public GestionnaireProjectile(Game jeu)
            : base(jeu)
        {
            Enabled = false;
            Visible = false;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texturesProjectiles = Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\projectiles");
            base.LoadContent();
        }

        public void CreerProjectile(ref World mondePhysique, Vector2 position, Vector2 direction, float vitesse, Armes type)
        {
            switch (type)
            {
                case Armes.Roquette:
                    projectile = new Roquette(mondePhysique, new Rectangle(1, 4, 18, 12), position + Conversion.PixelAuMetre(direction*50),
                                              direction, vitesse, texturesProjectiles);
                    break;
                case Armes.Grenade:
                    projectile = new Grenade(mondePhysique, new Rectangle(1, 19, 18, 22), position + Conversion.PixelAuMetre(direction*40), direction, vitesse, texturesProjectiles);
                    break;
                default:
                    break;
            }
            Enabled = true;
            Visible = true;
            projectile.Explosion += new Projectile.DelegateExplosion(projectile_Explosion);
        }

        void projectile_Explosion(Vector2 position, int rayonExplosion)
        {
            Explosion(position, rayonExplosion);
            this.Enabled = false;
            this.Visible = false;
        }

        public Projectile ObtenirProjectile()
        {
            return projectile;
        }

        public override void Update(GameTime gameTime)
        {
            projectile.Update(gameTime);
            if (projectile != null)
            {
                Position = projectile.ObtenirPosition();
            }
            else
            {
                Position = Vector2.Zero;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, 
                              BlendState.AlphaBlend, null, null, null, null, 
                              Resolution.getTransformationMatrix() * MatriceDeCamera);
            projectile.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
