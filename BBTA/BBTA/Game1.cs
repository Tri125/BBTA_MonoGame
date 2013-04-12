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
using BBTA.Partie_De_Jeu;
using IndependentResolutionRendering;
using BBTA.Classe.Option;

namespace BBTA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Random hasard = new Random();
        //États du menu
        public enum EtatJeu
        {
            Accueil,        //État initial du jeu -> Options, Configuration ou Quitter
            Options,        //Permet de modifier les options du jeu tels que volumee et touches -> Accueil ou Quitter
            Configuration,  //Permet de définir les paramètres de la partie -> Jeu
            Jeu,            //État du menu où que la partie à lieu -> Victoire, Défaite ou Pause 
            Victoire,       //État qui indique au joueur qu'il a gagné -> Accueil
            Defaite,        //État qui indique au joueur qu'il a perdu -> Accueil
            Pause           //État que le joueur accède lorsqu'il est en jeu -> Jeu, Accueil ou Quitter
        }
        EtatJeu EtatActuel = EtatJeu.Accueil;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState avant;
        MouseState now;
        private Accueil acc;
        private PartieJeu partie;
        static public BBTA_MapFileBuilder chargeurCarte = new BBTA_MapFileBuilder();
        static public BBTA_ConstructeurOption chargeurOption = new BBTA_ConstructeurOption();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.Title = "Bang Bang Total Annihilation";
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1440, 900);
            //La résolution de la fenêtre de jeu présenté à l'utilisateur
            Resolution.SetResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, false);
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            chargeurOption.Initialisation();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Etat Accueil
            acc = new Accueil(this);

            //Etat Jeu
            chargeurCarte.LectureCarte(@"Carte Jeu\lghill.xml");
            if (chargeurCarte.ChargementReussis)
            {
                partie = new PartieJeu(this, chargeurCarte.InfoTuileTab(), 1, 1);
            }
            //this.Components.Add(partie);
            partie.Visible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                avant = now;
                now = Mouse.GetState();

                // Permet de fermer la fenêtre du jeu
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }

                //S'occupe du transfer d'état entre les parties du menu
                switch (EtatActuel)
                {
                    case EtatJeu.Accueil:
                        /*TransferEtat
                        //Vérifie si le component correspond à l'état en cour*/
                        if (!this.Components.Contains(acc)) //Le component n'est pas celui qui correspond à l'état
                        {
                            this.Components.Clear();    //On l'éfface
                            this.Components.Add(acc);   //Et on s'assure que ce soit le bon
                        }

                        //Clic sur btnJouer -> Configuration en temps normal, mais pour la phase de développement, aller directement au jeu
                        if (acc.btnJouer.ClicComplet())
                        {
                            EtatActuel = EtatJeu.Jeu;
                        }
                        //Clic btnOptions -> Options
                        if (acc.btnOptions.ClicComplet())
                        {
                            EtatActuel = EtatJeu.Options;
                        }
                        //Clic btnQuitter -> Quitter
                        if (acc.btnQuitter.ClicComplet())
                        {
                            this.Exit();
                        }

                        break;
                    case EtatJeu.Options:
                        break;
                    case EtatJeu.Configuration:
                        break;
                    case EtatJeu.Jeu:
                        //TransferEtat
                        if (!this.Components.Contains(partie))
                        {
                            this.Components.Clear();
                            this.Components.Add(partie);
                        }
                        break;
                    case EtatJeu.Victoire:
                        break;
                    case EtatJeu.Defaite:
                        break;
                    case EtatJeu.Pause:
                        break;
                    default:
                        break;
                }
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Resolution.BeginDraw();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
