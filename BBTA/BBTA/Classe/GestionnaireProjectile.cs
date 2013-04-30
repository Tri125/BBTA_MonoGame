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
        private bool enAction = false;
        public Vector2 Position { get; private set; }
        private Texture2D texturesProjectiles;
        private List<Projectile> projectiles = new List<Projectile>();
        private SpriteBatch spriteBatch;
        public delegate void DelegateExplosion(Vector2 position, int rayonExplosion);
        public event DelegateExplosion Explosion;
        public event EventHandler ProcessusTerminer;

        public GestionnaireProjectile(Game jeu, ref World mondePhysique)
            : base(jeu)
        {

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
                    projectiles.Add(new Roquette(mondePhysique, new Rectangle(1, 4, 18, 12), position + Conversion.PixelAuMetre(direction * 60),
                                              direction, vitesse, texturesProjectiles));
                    break;
                case Armes.Grenade:
                    projectiles.Add(new Grenade(mondePhysique, new Rectangle(1, 19, 18, 22), position + Conversion.PixelAuMetre(direction*40), direction, vitesse, texturesProjectiles));
                    break;
                case Armes.Mine:
                    projectiles.Add(new Mine(ref mondePhysique, new Rectangle(3, 45, 14, 22), position + Conversion.PixelAuMetre(direction * 40), direction, vitesse, texturesProjectiles));
                    (projectiles[projectiles.Count - 1] as Mine).VitesseNulle += new EventHandler(GestionnaireProjectile_VitesseNulle);
                    break;
                default:
                    break;
            }
            projectiles[projectiles.Count - 1].Explosion += new Projectile.DelegateExplosion(projectile_Explosion);
            projectiles[projectiles.Count - 1].Detruit +=new EventHandler(GestionnaireProjectile_Detruit);
            enAction = true;
        }

        void GestionnaireProjectile_Detruit(object sender, EventArgs e)
        {
            projectiles.Remove((sender as Projectile));
            if(ProcessusTerminer != null)
            {
                ProcessusTerminer(this, new EventArgs());
            }
        }

        void GestionnaireProjectile_VitesseNulle(object sender, EventArgs e)
        {
            enAction = false;
            if (ProcessusTerminer != null)
            {
                ProcessusTerminer(this, new EventArgs());
            }
        }

        void projectile_Explosion(Projectile projectileExplosant, Vector2 position, int rayonExplosion)
        {
            enAction = false;
            Explosion(position, rayonExplosion);
        }

        public Projectile ObtenirProjectileEnMouvement()
        {
            if (projectiles.Count == 0 || enAction == false)
            {
                return null;
            }
            else
            {
                return projectiles[projectiles.Count-1];
            }
        }

        public override void Update(GameTime gameTime)
        {
            for(int nbProjectiles = 0; nbProjectiles < projectiles.Count; nbProjectiles++)
            {
                projectiles[nbProjectiles].Update(gameTime);
            }

            if (projectiles.Count > 0)
            {
                Position = projectiles[projectiles.Count - 1].ObtenirPosition();
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
            foreach (Projectile item in projectiles)
            {
                item.Draw(spriteBatch);   
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
