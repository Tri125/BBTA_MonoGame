﻿using System;
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

namespace BBTA.Partie_De_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {
        //Valeurs réliées aux conditions et réglements d'une partie---------------
        private const int TEMPS_TOUR_DEFAUT = 3000;
        private readonly int tempsTour;
        private int tempsEcouler;

        //Valeurs et variables techniques-----------------------------------------
        private SpriteBatch spriteBatch;
        private Camera2d camPartie;

        //Éléments d'interface----------------------------------------------------
        private IndicateurPuissance indicateurPuissance;
        private ViseurVisuel viseur;
        private SelectionArme selecteurArme;

        //Varaibles issues du moteur physique Farseer-----------------------------
        private World mondePhysique;

        //Éléments de jeu---------------------------------------------------------
        private Carte carte;
        private int[] carteTuile;
        private List<Equipe> listeEquipes;
        private Equipe equipeActive;
        private int nbrEquipe1;
        private int nbrEquipe2;

        public List<Acteur> ListeActeur
        {
            get
            {
                List<Acteur> temp = new List<Acteur>();
                foreach (Equipe equipe in listeEquipes)
                {
                    foreach (Acteur acteur in equipe.ListeMembres)
                    {
                        temp.Add(acteur);
                    }
                }
                return temp;
            }
        }

        public PartieJeu(Game jeu, int[] carteTuile, int nbrEquipe1, int nbrEquipe2, int tempsParTour = TEMPS_TOUR_DEFAUT)
            : base(jeu)
        {
            this.carteTuile = carteTuile;
            this.tempsTour = tempsParTour;
            this.listeEquipes = new List<Equipe>();
            this.nbrEquipe1 = nbrEquipe1;
            this.nbrEquipe2 = nbrEquipe2;
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
            camPartie = new Camera2d();
            mondePhysique = new World(new Vector2(0, 20));
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            viseur = new ViseurVisuel(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Viseur"), Game.Content.Load<Texture2D>(@"Ressources\Roquette"));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            carte = new Carte(carteTuile, Game1.chargeurCarte.InformationCarte().NbColonne, Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\blocs"), mondePhysique, 40);
            //La position de départ de la caméra est le centre de la carte
            camPartie.pos = new Vector2(Game1.chargeurCarte.InformationCarte().NbColonne /2, Game1.chargeurCarte.InformationCarte().NbRange/2) * 40;
            // TODO: use this.Content to load your game content here
            listeEquipes.Add(new Equipe());
            listeEquipes.Add(new Equipe());
            List<Vector2> listeApparition = carte.ListeApparition;
            for (int iBoucle = 0; iBoucle < nbrEquipe1; iBoucle++)
            {
                JoueurHumain joueurH = new JoueurHumain(mondePhysique, Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp"), PhaseApparition(ref listeApparition), 100, 3, 1, 75);
                listeEquipes[0].RajoutMembre(joueurH);
                joueurH.TourCompleter += EvenTourCompleter;
            }

            for (int iBoucle = 0; iBoucle < nbrEquipe2; iBoucle++)
            {
                JoueurHumain joueurH = new JoueurHumain(mondePhysique, Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp"), PhaseApparition(ref listeApparition), 100, 3, 1, 75);
                listeEquipes[1].RajoutMembre(joueurH);
                joueurH.TourCompleter += EvenTourCompleter;
            }
            //On charge une première équipe pour son tour.
            ChangementEquipe();
            List<Texture2D> tex = new List<Texture2D>();
            tex.Add(Game.Content.Load<Texture2D>(@"Ressources\Roquette"));
            selecteurArme = new SelectionArme(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\panneauSelecteurArme"), tex, Game.Content.Load<SpriteFont>(@"PoliceIndicateur"), 200);
            selecteurArme.ArmeSelectionnee += new EventHandler(sa_ArmeSelectionnee);
            indicateurPuissance = new IndicateurPuissance(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Puissance"));
        }

        void sa_ArmeSelectionnee(object sender, EventArgs e)
        {
            equipeActive.JoueurActif.enModeTir = true;
            viseur.Dessiner = true;
            viseur.estOuvert = true;
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
            foreach (Equipe equipe in listeEquipes)
            {
                foreach (Acteur acteur in equipe.ListeMembres)
                {
                    acteur.Update(gameTime);
                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && selecteurArme.estOuvert == false && !equipeActive.JoueurActif.enModeTir)
            {
                selecteurArme.estOuvert = true;
            }

            selecteurArme.Position = equipeActive.JoueurActif.ObtenirPosition();
            selecteurArme.Update(gameTime, camPartie.get_transformation(GraphicsDevice));
            viseur.Position = equipeActive.JoueurActif.ObtenirPosition();
            viseur.Update(gameTime);
            indicateurPuissance.Position = equipeActive.JoueurActif.ObtenirPosition();
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                indicateurPuissance.estOuvert = true;
            }
            indicateurPuissance.Update(gameTime);

            camPartie.SuivreObjet(equipeActive.JoueurActif.ObtenirPosition(), Game1.chargeurCarte.InformationCarte().NbRange * 40);
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            /* Circle position and rotation */
            // Convert physics position (meters) to screen coordinates (pixels)
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix() * camPartie.get_transformation(GraphicsDevice));
            carte.Draw(spriteBatch, camPartie.Pos);
            foreach (Equipe equipe in listeEquipes)
            {
                foreach (Acteur acteur in equipe.ListeMembres)
                {
                    acteur.Draw(spriteBatch);
                }
            }
            viseur.Draw(spriteBatch);
            selecteurArme.Draw(spriteBatch, GraphicsDevice);
            indicateurPuissance.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Vector2 PhaseApparition(ref List<Vector2> listeApparition)
        {
            //Si tout les points ont déjà été utilisé, on reprend la liste complète.
            if (listeApparition.Count == 0)
            {
                listeApparition = carte.ListeApparition;
            }
            int numHasard = Game1.hasard.Next(listeApparition.Count);
            Vector2 apparition = listeApparition[numHasard];
            listeApparition.RemoveAt(numHasard);
            return apparition / 40;
        }

        public void ChangementEquipe()
        {
            if (listeEquipes.Count != 0 && equipeActive == null)
            {
                equipeActive = listeEquipes[Game1.hasard.Next(listeEquipes.Count)];
            }
            else
            {
                equipeActive.ChangementEquipe();
                equipeActive = listeEquipes[(listeEquipes.IndexOf(equipeActive) + 1) % listeEquipes.Count()];
            }
            equipeActive.ChangementEquipe();
            equipeActive.ChangementJoueur();
            equipeActive.DebutTour();
        }

        public void EvenTourCompleter(object sender, EventArgs eventArgs)
        {
            if (sender is Acteur)
            {
                Acteur envoyeur = sender as Acteur;
                if (envoyeur != null )
                {
                    equipeActive.FinTour();
                    ChangementEquipe();
                }
                
            }
        }
    }
}
