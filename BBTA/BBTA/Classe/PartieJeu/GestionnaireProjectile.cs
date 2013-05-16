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
    /// <summary>
    /// Gestionnaire de projectile gère la création de projectiles, leur explosion et détermine quand que la caméra doit cesser d'être sur le projectile.
    /// Il garde en mémoire les différentes mines au sol.
    /// </summary>
    public class GestionnaireProjectile:DrawableGameComponent
    {
        //Ressources-----------------------------------------------------------------------------------------------------------------------------------
        private GestionSon gestionnaireSon;
        private SpriteBatch spriteBatch;
        private Texture2D texturesProjectiles;

        //Variables------------------------------------------------------------------------------------------------------------------------------------
        private List<Projectile> projectiles = new List<Projectile>();
        private bool enAction = false;

        //Propriétés-----------------------------------------------------------------------------------------------------------------------------------
        public Matrix MatriceDeCamera { get; set; }        

        //Événements et délégués-----------------------------------------------------------------------------------------------------------------------
        public delegate void DelegateExplosion(Vector2 position, int rayonExplosion);
        public event DelegateExplosion Explosion;
        public event EventHandler ProcessusTerminer;
        private event EventHandler SonLancementArme;
        private event EventHandler SonDestructionArme;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="jeu">Jeu XNA</param>
        public GestionnaireProjectile(Game jeu)
            : base(jeu)
        {

        }

        /// <summary>
        /// Initialise les variables reliés aux effets sonores.
        /// </summary>
        public override void Initialize()
        {
            gestionnaireSon = Game.Services.GetService(typeof(GestionSon)) as GestionSon;
            SonLancementArme += gestionnaireSon.SonLancement;
            SonDestructionArme += gestionnaireSon.SonExplosion;
            base.Initialize();
        }

        /// <summary>
        /// Chargement du contenu et initialisation des variables utilisant ces ressources
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texturesProjectiles = Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\projectiles");
            base.LoadContent();
        }

        /// <summary>
        /// Permet de créer le projectile désiré et lui associe tous les événements nécessaires
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel évoluera le projectile</param>
        /// <param name="position">Position du joueur tirant le projectile</param>
        /// <param name="vitesse">Vitesse initiale du projectile</param>
        /// <param name="type">Type du projectile (Grenade, mine, etc.)</param>
        public void CreerProjectile(ref World mondePhysique, Vector2 position, Vector2 vitesse, Armes type)
        {
            Vector2 distanceDepart = vitesse;
            distanceDepart.Normalize();
            distanceDepart *= 50;           /* Le projectile doit être créer à une certaine distance du joueur pour qu'il concorde avec l'extrémité du bazooka et qu'il ne soit pas en 
                                             * collision avec le joueur lui-même */

            switch (type)
            {
                case Armes.Roquette:
                    projectiles.Add(new Roquette(mondePhysique, texturesProjectiles, new Rectangle(1, 4, 18, 12), position + Conversion.PixelAuMetre(distanceDepart), vitesse));
                    break;
                case Armes.Grenade:
                    projectiles.Add(new Grenade(mondePhysique, texturesProjectiles, new Rectangle(1, 19, 18, 22), position, vitesse));
                    break;
                case Armes.Mine:
                    projectiles.Add(new Mine(ref mondePhysique, texturesProjectiles, new Rectangle(3, 45, 14, 22), position, vitesse));
                    (projectiles[projectiles.Count - 1] as Mine).FixationAuSol += new EventHandler(GestionnaireProjectile_FixationAuSol);
                    break;
                default:
                    break;
            }
            projectiles[projectiles.Count - 1].Explosion += new Projectile.DelegateExplosion(projectile_Explosion);
            projectiles[projectiles.Count - 1].Detruit +=new EventHandler(GestionnaireProjectile_Detruit);
            enAction = true; //Indique qu'un projectile est en mouvement
            SonLancementArme(type, EventArgs.Empty);
        }

        /// <summary>
        /// Met à jour la position des projectiles.
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            for(int nbProjectiles = 0; nbProjectiles < projectiles.Count; nbProjectiles++)
            {
                projectiles[nbProjectiles].Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Si un projectile explose, alors on le retire de la liste et on déclanche un événement pour indiquer que le jeu peut passer à autre chose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GestionnaireProjectile_Detruit(object sender, EventArgs e)
        {
            enAction = false;
            projectiles.Remove((sender as Projectile));
            if (ProcessusTerminer != null)
            {
                ProcessusTerminer(this, new EventArgs());
            }
        }

        /// <summary>
        /// Si la mine se colle au sol, alors on déclanche un événement pour indiquer au jeu de passer à autre chose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GestionnaireProjectile_FixationAuSol(object sender, EventArgs e)
        {
            enAction = false;
            if (ProcessusTerminer != null)
            {
                ProcessusTerminer(this, new EventArgs());
            }
        }

        /// <summary>
        /// Si un projectile explose, on fait entendre les bons effets sonores et on déclanche un événement pour transmettre les informations de l'explosion
        /// </summary>
        /// <param name="position">Position de l'explosion</param>
        /// <param name="rayonExplosion">Rayon de dégats de l'explosion</param>
        void projectile_Explosion(Vector2 position, int rayonExplosion)
        {
            enAction = false;
            SonDestructionArme(null, new EventArgs());
            Explosion(position, rayonExplosion);
        }

        /// <summary>
        /// Retourne le projectile en mouvement s'il existe.
        /// </summary>
        /// <returns>Projectile en mouvement s'il existe</returns>
        public Projectile ObtenirProjectileEnMouvement()
        {
            //S'il n'y a aucun projectile en mouvement, on retourne null
            if (projectiles.Count == 0 || enAction == false)
            {
                return null;
            }
            //Autrement, on retourne le projectile en mouvement.
            else
            {
                return projectiles[projectiles.Count - 1];
            }
        }

        /// <summary>
        /// Affiche les divers projectiles à l'écran
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
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
