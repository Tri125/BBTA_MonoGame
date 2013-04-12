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
        private const int TEMPS_TOUR_DEFAUT = 3000;
        private readonly int tempsTour;
        private SpriteBatch spriteBatch;
        MouseState avant;
        MouseState now;

        private World mondePhysique;
        private int tempsEcouler;

        private Camera2d camPartie;
        private ViseurVisuel vs;
        Carte carte;
        int[] carteTuile;
        List<Equipe> listeEquipes;
        private int nbrEquipe1;
        private int nbrEquipe2;
        Roquette ro;
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

            vs = new ViseurVisuel(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Viseur"));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            carte = new Carte(carteTuile, Game1.chargeurCarte.InformationCarte().NbColonne, Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\blocs"), mondePhysique, 40);
            //La position de départ de la caméra est le centre de la carte
            camPartie.pos = new Vector2(Game1.chargeurCarte.InformationCarte().NbColonne /2, Game1.chargeurCarte.InformationCarte().NbRange/2) * 40;
            // TODO: use this.Content to load your game content here
            ro = new Roquette(mondePhysique, new Vector2(10, 0), Game.Content.Load<Texture2D>(@"Ressources\Acteur\ActeurBleu"));            listeEquipes.Add(new Equipe());
            listeEquipes.Add(new Equipe());
            List<Vector2> listeApparition = carte.ListeApparition;
            for (int iBoucle = 0; iBoucle < nbrEquipe1; iBoucle++)
            {
                listeEquipes[0].RajoutMembre(new JoueurHumain(mondePhysique, Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp"), PhaseApparition(ref listeApparition), 100, 3, 1, 75));
            }

            for (int iBoucle = 0; iBoucle < nbrEquipe2; iBoucle++)
            {
                listeEquipes[0].RajoutMembre(new JoueurHumain(mondePhysique, Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp"), PhaseApparition(ref listeApparition), 100, 3, 1, 75));
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            avant = now;
            now = Mouse.GetState();
            Point nowPos = Resolution.MouseHelper.PositionSourisCamera(camPartie.transform);


            // TODO: Add your update logic here
            mondePhysique.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            ro.Update(gameTime);
            foreach (Equipe equipe in listeEquipes)
            {
                foreach (Acteur acteur in equipe.ListeMembres)
                {
                    acteur.Update(gameTime);
                }
            }
            vs.AssocierAujoueur(listeEquipes[0].ListeMembres[0]);
            vs.Update(gameTime, nowPos);
            camPartie.SuivreObjet(listeEquipes[0].ListeMembres[0].ObtenirPosition(), Game1.chargeurCarte.InformationCarte().NbRange * 40);
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
            vs.Draw(spriteBatch);
            ro.Draw(spriteBatch);
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
    }
}
