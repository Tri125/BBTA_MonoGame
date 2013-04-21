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

namespace BBTA.Partie_De_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {

        private const int TEMPS_TOUR_DEFAUT = 3000;
        private readonly int tempsTour;
        private SpriteBatch spriteBatch;
        private IndicateurPuissance ip;
        private World mondePhysique;
        private int tempsEcouler;
        public SelectionArme sa;
        GestionnaireActeurs gs;
        private Camera2d camPartie;
        private ViseurVisuel vs;
        Carte carte;
        int[] carteTuile;

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
            this.carteTuile = carteTuile;
            this.tempsTour = tempsParTour;
            gs = new GestionnaireActeurs(jeu, nbrEquipe1, nbrEquipe2, true);
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
            camPartie.pos = new Vector2(Game1.chargeurCarte.InformationCarte().NbColonne / 2,
                            Game1.chargeurCarte.InformationCarte().NbRange / 2) * 40;
            base.Initialize();
            Game.Components.Add(gs);
            gs.CreerJoueurs(ref mondePhysique, carte.ListeApparition);
            gs.DrawOrder = 1;
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

            camPartie.SuivreObjet(gs.equipeActive.JoueurActif.ObtenirPosition(), Game1.chargeurCarte.InformationCarte().NbRange * 40);
            gs.matriceCamera = camPartie.get_transformation(GraphicsDevice);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix() * camPartie.get_transformation(GraphicsDevice));
            carte.Draw(spriteBatch, camPartie.Pos);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}