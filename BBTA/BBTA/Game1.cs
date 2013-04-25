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
using BBTA.Classe.Menus;using BBTA.Classe.Option;
using EventInput;

namespace BBTA
{
    //États du menu
    public enum EtatJeu
    {
        Accueil, //X       //État initial du jeu -> Options, Configuration ou Quitter
        Options, //X       //Permet de modifier les options du jeu tels que le volume et les touches -> Accueil ou Quitter
        Configuration, //X //Permet de définir les paramètres de la partie -> Jeu
        Jeu,  //x          //État du menu où que la partie à lieu -> Victoire, Défaite ou Pause 
        Victoire,       //État qui indique au joueur qu'il a gagné -> Accueil
        Defaite,        //État qui indique au joueur qu'il a perdu -> Accueil
        Pause           //État que le joueur accède lorsqu'il est en jeu -> Jeu, Accueil ou Quitter
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Random hasard = new Random();
        //État initial du jeu
        EtatJeu EtatActuel = EtatJeu.Accueil;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState avant;
        MouseState now;

        private MenuAccueil acc;
        private PartieJeu partie;
        private MenuOptions option;
        private MenuConfiguration config;
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

            EventInput.EventInput.Initialize(Window);

            //Etat Accueil
            acc = new MenuAccueil(this);

            //Etat Options
            option = new MenuOptions(this);

            //Etat configuration
            config = new MenuConfiguration(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
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
                            this.Components.Clear();
                            this.Components.Add(acc);   //Et on s'assure que ce soit le bon
                        }
                        EtatActuel = acc.ObtenirEtat();
                        acc.RemiseAZeroEtat();
                        break;

                    case EtatJeu.Options:
                        if (!this.Components.Contains(option))
                        {
                            this.Components.Clear();
                            this.Components.Add(option);
                        }
                        EtatActuel = option.ObtenirEtat();
                        option.RemiseAZeroEtat();
                        break;

                    case EtatJeu.Configuration:
                        if (!this.Components.Contains(config))
                        {
                            this.Components.Clear();
                            this.Components.Add(config);
                        }
                        EtatActuel = config.ObtenirEtat();
                        config.RemiseAZeroEtat();
                        break;

                    case EtatJeu.Jeu:
                        //TransferEtat
                        if (!this.Components.Contains(partie))
                        {
                            this.Components.Clear();
                            chargeurCarte.LectureCarte(@"Carte Jeu\lgHill.xml");
                            if (chargeurCarte.ChargementReussis)
                            {
                                partie = new PartieJeu(this, chargeurCarte.InfoTuileTab(), 1, 1);
                            }
                            this.Components.Add(partie);
                        }
                        break;

                    case EtatJeu.Victoire:
                        break;

                    case EtatJeu.Defaite:
                        break;

                    case EtatJeu.Pause:
                        break;
                }
                base.Update(gameTime);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            Resolution.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
