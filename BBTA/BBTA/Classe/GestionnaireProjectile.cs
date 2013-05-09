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
using BBTA.Outils;
using BBTA.GestionAudio;

namespace BBTA.Partie_De_Jeu
{
    public class GestionnaireProjectile:DrawableGameComponent, IUtiliseMatriceCamera
    {
        private GestionSon gestionnaireSon;
        private event EventHandler SonLancementArme;
        private event EventHandler SonDestructionArme;

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
            gestionnaireSon = Game.Services.GetService(typeof(GestionSon)) as GestionSon;
            SonLancementArme += gestionnaireSon.SonLancement;
            SonDestructionArme += gestionnaireSon.SonExplosion;
            base.LoadContent();
        }

        public void CreerProjectile(ref World mondePhysique, Vector2 position, Vector2 vitesse, Armes type)
        {
            Vector2 distanceDepart = vitesse;
            distanceDepart.Normalize();
            distanceDepart *= 60;
            switch (type)
            {
                case Armes.Roquette:
                    projectiles.Add(new Roquette(mondePhysique, texturesProjectiles, new Rectangle(1, 4, 18, 12), position + Conversion.PixelAuMetre(distanceDepart), vitesse));
                    break;
                case Armes.Grenade:
                    projectiles.Add(new Grenade(mondePhysique, texturesProjectiles, new Rectangle(1, 19, 18, 22), position + Conversion.PixelAuMetre(distanceDepart), vitesse));
                    break;
                case Armes.Mine:
                    projectiles.Add(new Mine(ref mondePhysique, texturesProjectiles, new Rectangle(3, 45, 14, 22), position + Conversion.PixelAuMetre(distanceDepart), vitesse));
                    (projectiles[projectiles.Count - 1] as Mine).FixationAuSol += new EventHandler(GestionnaireProjectile_FixationAuSol);
                    break;
                default:
                    break;
            }
            projectiles[projectiles.Count - 1].Explosion += new Projectile.DelegateExplosion(projectile_Explosion);
            projectiles[projectiles.Count - 1].Detruit +=new EventHandler(GestionnaireProjectile_Detruit);
            enAction = true;
            SonLancementArme(type, EventArgs.Empty);
        }

        void GestionnaireProjectile_Detruit(object sender, EventArgs e)
        {
            projectiles.Remove((sender as Projectile));
            if(ProcessusTerminer != null)
            {
                ProcessusTerminer(this, new EventArgs());
            }
        }

        void GestionnaireProjectile_FixationAuSol(object sender, EventArgs e)
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
            SonDestructionArme(null, new EventArgs());
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
