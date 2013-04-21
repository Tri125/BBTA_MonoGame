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

namespace BBTA.Partie_De_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {       
        //Valeurs reliées aux règles et mécaniques de jeu------------------------
        private const int TEMPS_TOUR_DEFAUT = 3000;
        private readonly int tempsTour;
        private int tempsEcouler;

        //Variables techniques---------------------------------------------------
        private SpriteBatch spriteBatch;
        private Camera2d camPartie;

        //Variables reliées à l'utilisation du moteur physique Farseer-----------
        private World mondePhysique;

        //Variables reliées aux éléments d'une partie de BBTA
        private Carte carte;
        private int[] carteTuile;
        private List<Equipe> listeEquipes;
        private Equipe equipeActive;

        //public List<Acteur> ListeActeur
        //{
        //    get
        //    {
        //        List<Acteur> temp = new List<Acteur>();
        //        foreach (Equipe equipe in listeEquipes)
        //        {
        //            foreach (Acteur acteur in equipe.ListeMembres)
        //            {
        //                temp.Add(acteur);
        //            }
        //        }
        //        return temp;
        //    }
        //}

        public PartieJeu(Game jeu, int[] carteTuile, int nbrEquipe1, int nbrEquipe2, int tempsParTour = TEMPS_TOUR_DEFAUT)
            : base(jeu)
        {
            mondePhysique = new World(new Vector2(0, 20));
            this.carteTuile = carteTuile;
            this.tempsTour = tempsParTour;
            carte = new Carte(Game, carteTuile, Game1.chargeurCarte.InformationCarte().NbColonne, mondePhysique);
            Game.Components.Add(carte);
            this.listeEquipes = new List<Equipe>();
            listeEquipes.Add(new Equipe());
            listeEquipes.Add(new Equipe());
            List<Vector2> listeApparition = carte.ListeApparition;

            for (int iBoucle = 0; iBoucle < nbrEquipe1; iBoucle++)
            {
                JoueurHumain joueurH = new JoueurHumain(jeu, mondePhysique, PhaseApparition(ref listeApparition), 100, 3, 1, 75);
                Game.Components.Add(joueurH);
                listeEquipes[0].RajoutMembre(joueurH);
                joueurH.TourCompleter += EvenTourCompleter;
            }

            for (int iBoucle = 0; iBoucle < nbrEquipe2; iBoucle++)
            {
                JoueurHumain joueurH = new JoueurHumain(jeu, mondePhysique, PhaseApparition(ref listeApparition), 100, 3, 1, 75);
                Game.Components.Add(joueurH);
                listeEquipes[1].RajoutMembre(joueurH);
                joueurH.TourCompleter += EvenTourCompleter;
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            camPartie = new Camera2d();
            camPartie.pos = new Vector2(Game1.chargeurCarte.InformationCarte().NbColonne / 2, 
                                        Game1.chargeurCarte.InformationCarte().NbRange / 2) * 40;
            carte.MatriceDeCamera = camPartie.get_transformation(GraphicsDevice);
            carte.PositionCamera = camPartie.Pos;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            ChangementEquipe();
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

            camPartie.SuivreObjet(equipeActive.JoueurActif.ObtenirPosition(), Game1.chargeurCarte.InformationCarte().NbRange * 40);
            base.Update(gameTime);
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
