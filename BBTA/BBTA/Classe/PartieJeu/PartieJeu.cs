using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using XNATileMapEditor;
using BBTA.Elements;
using FarseerPhysics.Factories;
using BBTA.Outils;
using BBTA.Menus;
using BBTA.Interface;
using IndependentResolutionRendering;
using FarseerPhysics.Collision.Shapes;
using BBTA.Classe;
using System.Timers;
using BBTA.Classe.Outils;

namespace BBTA.Partie_De_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {

        private const int TEMPS_TOUR_DEFAUT = 3000;
        private readonly int tempsTour;
        private SpriteBatch spriteBatch;
        private bool EstEnTransition = false;
        private Timer compteReboursApresTir = new Timer(2000); 
        private World mondePhysique;
        private int tempsEcouler;
        GestionnaireActeurs gestionnaireEquipes;
        GestionnaireMenusTir gestionnaireMenusTir;
        GestionnaireProjectile gestionnaireProjectile;
        private Camera2d camPartie;
        Carte carte;
        int[] carteTuile;

        public PartieJeu(Game jeu, int[] carteTuile, int nbrEquipe1, int nbrEquipe2, int tempsParTour = TEMPS_TOUR_DEFAUT)
            : base(jeu)
        {
            this.carteTuile = carteTuile;
            this.tempsTour = tempsParTour;
            gestionnaireEquipes = new GestionnaireActeurs(jeu, nbrEquipe1, nbrEquipe2, true);
            gestionnaireMenusTir = new GestionnaireMenusTir(jeu);
            gestionnaireProjectile = new GestionnaireProjectile(jeu);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization logic here
            mondePhysique = new World(new Vector2(0, 20));
            camPartie = new Camera2d(Conversion.MetreAuPixel(Game1.chargeurCarte.InformationCarte().NbRange));
            camPartie.pos = new Vector2(Game1.chargeurCarte.InformationCarte().NbColonne / 2,
                            Game1.chargeurCarte.InformationCarte().NbRange / 2) * 40;
            base.Initialize();
            Game.Components.Add(gestionnaireEquipes);
            gestionnaireEquipes.CreerJoueurs(ref mondePhysique, carte.ListeApparition);
            gestionnaireEquipes.DrawOrder = 1;
            gestionnaireEquipes.Tir += new GestionnaireActeurs.DelegateTirEntamme(gestionnaireEquipes_Tir);
            gestionnaireMenusTir.ProcessusDeTirTerminer += new GestionnaireMenusTir.DelegateProcessusDeTirTerminer(gestionnaireMenusTir_ProcessusDeTirTerminer);
            Game.Components.Add(gestionnaireMenusTir);
            gestionnaireMenusTir.DrawOrder = 3;
            Game.Components.Add(gestionnaireProjectile);
            gestionnaireProjectile.Explosion += new GestionnaireProjectile.DelegateExplosion(gestionnaireProjectile_Explosion);
            gestionnaireMenusTir.TirAvorte += new EventHandler(gestionnaireMenusTir_TirAvorte);
            gestionnaireProjectile.DrawOrder = 2;
            this.DrawOrder = 0;
        }

        void gestionnaireMenusTir_TirAvorte(object sender, EventArgs e)
        {
            gestionnaireEquipes.equipeActive.JoueurActif.enModeTir = false;
        }

        void gestionnaireProjectile_Explosion(Vector2 position, float energieExplosion)
        {
            carte.Explosion(position, energieExplosion);
            compteReboursApresTir.Elapsed += new ElapsedEventHandler(compteReboursApresTir_Elapsed);
            compteReboursApresTir.Start();
            EstEnTransition = true;
        }

        void compteReboursApresTir_Elapsed(object sender, ElapsedEventArgs e)
        {
            compteReboursApresTir.Stop();
            camPartie.SeDirigerVers(gestionnaireEquipes.equipeActive.JoueurActif);
            camPartie.Verouiller += new EventHandler(camPartie_Verouiller);
        }

        void camPartie_Verouiller(object sender, EventArgs e)
        {
            EstEnTransition = false;
            gestionnaireProjectile.Enabled = false;
            gestionnaireEquipes.Enabled = true;
        }

        void gestionnaireMenusTir_ProcessusDeTirTerminer(Vector2 position, Vector2 direction, float vitesse, Armes type)
        {
            gestionnaireProjectile.CreerProjectile(ref mondePhysique, position, direction, vitesse, type);
            gestionnaireProjectile.Enabled = true;
            gestionnaireEquipes.Enabled = false;
            gestionnaireEquipes.equipeActive.JoueurActif.enModeTir = false;
        }

        void gestionnaireEquipes_Tir(Vector2 position)
        {
            gestionnaireMenusTir.Position = position;
            gestionnaireMenusTir.DemarrerSequenceTir();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            carte = new Carte(carteTuile, Game1.chargeurCarte.InformationCarte().NbColonne, Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\blocs"), mondePhysique, 40);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            mondePhysique.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            if (EstEnTransition == false)
            {
                gestionnaireMenusTir.Position = gestionnaireEquipes.equipeActive.JoueurActif.ObtenirPosition();
                if (gestionnaireProjectile.Enabled)
                {
                    gestionnaireProjectile.MatriceDeCamera = camPartie.get_transformation(GraphicsDevice);
                    camPartie.ObjetSuivi = gestionnaireProjectile.ObtenirProjectile();
                }
                else
                {
                    camPartie.ObjetSuivi = gestionnaireEquipes.equipeActive.JoueurActif;
                }
            }
            camPartie.Update(gameTime);
            gestionnaireEquipes.matriceCamera = camPartie.get_transformation(GraphicsDevice);
            gestionnaireMenusTir.MatriceDeCamera = camPartie.get_transformation(GraphicsDevice);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix() * camPartie.get_transformation(GraphicsDevice));
            carte.Draw(spriteBatch, camPartie.Pos);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
