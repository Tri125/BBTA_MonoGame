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
using EditeurCarteXNA;
using BBTA.Classe.Elements;
using FarseerPhysics.Factories;
using BBTA.Outils;
using BBTA.Menus;
using BBTA.Classe.Interface;
using BBTA.Partie_De_Jeu;
using IndependentResolutionRendering;
using BBTA.Classe.Menus;
using BBTA.Classe.Option;
using EventInput;
using BBTA.Classe.IA.Navigation;
using BBTA.Classe.Outils;
using BBTA.Classe.GestionAudio;

namespace BBTA
{
    //�tats du menu
    public enum EtatJeu
    {
        Accueil = 0, //X       //�tat initial du jeu -> Options, Configuration ou Quitter
        Options = 1, //X       //Permet de modifier les options du jeu tels que le volume et les touches -> Accueil ou Quitter
        Configuration = 2, //X //Permet de d�finir les param�tres de la partie -> Jeu
        Jeu = 3,  //x          //�tat du menu o� que la partie � lieu -> Victoire, D�faite ou Pause 
        FinDePartie = 4,
        Pause = 5           //�tat que le joueur acc�de lorsqu'il est en jeu -> Jeu, Accueil ou Quitter
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //�tat initial du jeu
        EtatJeu EtatActuel = EtatJeu.Accueil;
        EtatJeu EtatPrecedent;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState avant;
        MouseState now;
        private OutilGraphe TesteGraphe;
        private GestionMusique gestionnaireMusique;
        //�v�nement utilis� pour communiquer un changement d'�tat du jeu gestionnaire de musique.
        private event EventHandler ChangementEtat;
        private GestionSon gestionnaireSon;

        private MenuAccueil acc;
        private PartieJeu partie;
        private MenuOptions option;
        private MenuConfiguration config;
        private MenuFinDePartie finPartie;

        //On construit plusieurs objet static public pour que des classes externes puissent avoir acc�ss aux outils.
        //Game1.cs est construit imm�diatement apr�s Program.cs, alors nous sommes assur� que les classes externes n'auront jamais
        //un null en essayant de prendre les objets.
        public static Random hasard = new Random();
        static public BBTA_ConstructeurCarte chargeurCarte = new BBTA_ConstructeurCarte(3, "*.xml", "Carte Jeu");
        static public BBTA_ConstructeurOption chargeurOption = new BBTA_ConstructeurOption();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.Title = "Bang Bang Total Annihilation";
            //Initialisation de la r�solution ind�pendante de l'affichage.
            Resolution.Init(ref graphics);
            //R�solution virtuelle interne.
            Resolution.SetVirtualResolution(1440, 900);
            //La r�solution de la fen�tre de jeu pr�sent� � l'utilisateur
            Resolution.SetResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, 
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, false);
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            //Chargement des cartes successivement.
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

            gestionnaireMusique = new GestionMusique(this);
            this.Components.Add(gestionnaireMusique);
            ChangementEtat += gestionnaireMusique.ChangementEtatJeu;

            gestionnaireSon = new GestionSon(this);
            this.Components.Add(gestionnaireSon);
            //On enregistre gestionnaireSon en tant que Services. Cela permet aux classes externes de consommer gestionnaireSon
            //sans avoir de r�f�rence ou de param�tre.
            Services.AddService(typeof (GestionSon), gestionnaireSon);

            //Etat Accueil
            acc = new MenuAccueil(this);

            //Etat Options
            option = new MenuOptions(this);

            //Etat configuration
            config = new MenuConfiguration(this);

            TesteGraphe = new OutilGraphe(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ChangementEtat(this.EtatActuel, EventArgs.Empty);
            //Pour le menu des options de configuration, on donne en param�tre le gestionnaire de son et celui de la musique
            //pour li� les �v�nements correspondant au contr�le du volume.
            option.InitControlAudio(gestionnaireMusique , gestionnaireSon);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gestionnaireMusique.Update(gameTime);
            gestionnaireSon.Update(gameTime);

            EtatPrecedent = EtatActuel;
            //Ne roule plus la boucle principale Update si la fen�tre perd le focus.
            if (this.IsActive)
            {
                avant = now;
                now = Mouse.GetState();

                // Permet de fermer la fen�tre du jeu
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }

                //S'occupe du transfer d'�tat entre les parties du menu
                switch (EtatActuel)
                {
                    case EtatJeu.Accueil:
                        /*TransferEtat
                        //V�rifie si le component correspond � l'�tat en cour*/
                        if (!this.Components.Contains(acc)) //Le component n'est pas celui qui correspond � l'�tat
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
                            if (chargeurCarte.ChargementReussis)
                            {
                                partie = new PartieJeu(this, chargeurCarte.InfoTuileTab(), new Vector2(chargeurCarte.InformationCarte().NbColonne,
                                    chargeurCarte.InformationCarte().NbRange), config.NbSoldatsJ1, config.NbSoldatsJ2);
                            }
                            this.Components.Add(partie);
                        }
                        EtatActuel = partie.ObtenirEtat();
                        partie.RemiseAZeroEtat();
                        break;

                    case EtatJeu.FinDePartie:
                        if (!this.Components.Contains(finPartie))
                        {
                            this.Components.Clear();
                            finPartie = new MenuFinDePartie(this, partie.ObtenirCouleurEquipePerdante());
                            this.Components.Add(finPartie);
                        }
                        break;

                    case EtatJeu.Pause:
                        break;
                }
                //On d�tecte un changement d'�tat.
                //On signale le gestionnaire de musique par un �v�nement pour jouer une musique appropri�.
                if (EtatActuel != EtatPrecedent)
                {
                    ChangementEtat(this.EtatActuel, EventArgs.Empty);
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
