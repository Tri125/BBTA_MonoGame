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
using BBTA.Interface;
using IndependentResolutionRendering;
using FarseerPhysics.Collision.Shapes;
using System.Timers;
using BBTA.Classe.Outils;
using System.Text;
using BBTA.Classe.Elements;
using BBTA.Classe.IA.Robot;
using BBTA.Carte;
using BBTA.Classe;

namespace BBTA.Partie_De_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {
        private const int TEMPS_TOUR_DEFAUT = 30000;
        private readonly int tempsTour;
        private SpriteBatch spriteBatch;
        private SpriteFont policeCompte;
        private SpriteFont policeNbJoueurs;
        private Color CouleurSecondes = Color.DarkGray;
        private Texture2D secondesRestantes;
        private bool EstEnTransition = false;
        private Timer compteReboursApresTir = new Timer(2000); 
        private World mondePhysique;
        private int tempsEcouler;
        private List<Equipe> equipes = new List<Equipe>();
        private Equipe equipeActive;
        private Color equipePerdante;
        GestionnaireMenusTir gestionnaireMenusTir;
        GestionnaireProjectile gestionnaireProjectile;
        private Camera2d camPartie;
        CarteJeu carte;
        int[] carteTuile;

        private EtatJeu prochainEtat;

        public PartieJeu(Game jeu, int[] carteTuile, Vector2 dimensionsCarte, int nbrEquipe1, int nbrEquipe2, int tempsParTour = TEMPS_TOUR_DEFAUT)
            : base(jeu)
        {
            prochainEtat = EtatJeu.Jeu;
            this.carteTuile = carteTuile;
            this.tempsTour = tempsParTour;
            tempsEcouler = tempsTour;
            equipes.Add(new Equipe(Color.Firebrick, nbrEquipe1, true));
            equipes.Add(new Equipe(Color.Blue, nbrEquipe2, true));
            foreach (Equipe equipe in equipes)
            {
                equipe.TirDemande += new Equipe.DelegateTirDemande(equipe_TirDemande);
            }
            mondePhysique = new World(new Vector2(0, 20));
            gestionnaireMenusTir = new GestionnaireMenusTir(jeu);
            gestionnaireProjectile = new GestionnaireProjectile(jeu, ref mondePhysique);
            gestionnaireMenusTir.Visible = false;
            gestionnaireProjectile.Visible = false;
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
            camPartie = new Camera2d(Conversion.MetreAuPixel(Game1.chargeurCarte.InformationCarte().NbRange));
            camPartie.pos = new Vector2(Game1.chargeurCarte.InformationCarte().NbColonne / 2,
                            Game1.chargeurCarte.InformationCarte().NbRange / 2) * 40;
            base.Initialize();            
            gestionnaireMenusTir.ProcessusDeTirTerminer += new GestionnaireMenusTir.DelegateProcessusDeTirTerminer(gestionnaireMenusTir_ProcessusDeTirTerminer);
            Game.Components.Add(gestionnaireMenusTir);
            Game.Components.Add(gestionnaireProjectile);
            gestionnaireProjectile.Explosion += new GestionnaireProjectile.DelegateExplosion(gestionnaireProjectile_Explosion);
            gestionnaireProjectile.ProcessusTerminer += new EventHandler(gestionnaireProjectile_ProcessusTerminer);
            gestionnaireMenusTir.TirAvorte += new EventHandler(gestionnaireMenusTir_TirAvorte);
            camPartie.Verouiller += new EventHandler(camPartie_Verouiller);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            carte = new CarteJeu(carteTuile, Game1.chargeurCarte.InformationCarte().NbColonne, Game1.chargeurCarte.InformationCarte().NbRange,
                              Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\blocs"), 
                              mondePhysique, 40);
            Texture2D textureJoueur = Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp");
            policeCompte = Game.Content.Load<SpriteFont>(@"CompteRebours");
            secondesRestantes = Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\SecondesRestantes");
            policeNbJoueurs = Game.Content.Load<SpriteFont>(@"PoliceNbJoueursVie");

            List<Vector2> listeApparition = carte.ListeApparition;            foreach (Equipe equipe in equipes)
            {
                for (int nbJoueursAjoutes = 0; nbJoueursAjoutes < equipe.NombreJoueursOriginel; nbJoueursAjoutes++)
                {
                    Vector2 pointApparition = PhaseApparition(ref listeApparition);
                    if (equipe.EstHumain == true)
                    {
                        equipe.RajoutMembre(new JoueurHumain(mondePhysique, textureJoueur, pointApparition,
                                                             3, 1, 100));
                    }
                }
                equipe.ChargerPolice(policeNbJoueurs);
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            equipeActive = equipes[0];
            equipeActive.DebutTour();
        }


        void equipe_TirDemande(Vector2 position, Armement munitions)
        {
            gestionnaireMenusTir.DemarrerSequenceTir(equipeActive.JoueurActif.ObtenirPosition(), munitions);
        }

        void gestionnaireProjectile_ProcessusTerminer(object sender, EventArgs e)
        {
            if (compteReboursApresTir.Enabled == false)
            {
                compteReboursApresTir.Elapsed += new ElapsedEventHandler(compteReboursApresTir_Elapsed);
                compteReboursApresTir.Start();

                ChangementEquipe();
            }
            EstEnTransition = true;
        }

        void gestionnaireMenusTir_TirAvorte(object sender, EventArgs e)
        {
            equipeActive.JoueurActif.enModeTir = false;
        }

        void gestionnaireProjectile_Explosion(Vector2 position, int rayonExplosion)
        {
            compteReboursApresTir.Elapsed += new ElapsedEventHandler(compteReboursApresTir_Elapsed);
            compteReboursApresTir.Start();
            EstEnTransition = true;
            foreach (Equipe equipe in equipes)
            {
                equipe.RecevoirDegats(position, rayonExplosion);
            }
            carte.Explosion(position, rayonExplosion);
            ChangementEquipe();
        }

        void compteReboursApresTir_Elapsed(object sender, ElapsedEventArgs e)
        {
            compteReboursApresTir.Stop();
            camPartie.SeDirigerVers(equipeActive.JoueurActif);
        }

        void camPartie_Verouiller(object sender, EventArgs e)
        {
            EstEnTransition = false;
            tempsEcouler = tempsTour;
        }

        void gestionnaireMenusTir_ProcessusDeTirTerminer(Vector2 position, Vector2 vitesse, Armes type, Armement munitions)
        {
            equipeActive.Munitions = munitions;
            gestionnaireProjectile.CreerProjectile(ref mondePhysique, position, vitesse, type);
            equipeActive.JoueurActif.enModeTir = false;
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

            //Mise à jour des équipes
            foreach (Equipe equipe in equipes)
            {
                equipe.Update(gameTime);
                //Vérifier perdant
                if (equipe.DeterminerEquipePerdante())
                {
                    equipePerdante = equipe.couleur;
                    prochainEtat = EtatJeu.FinDePartie;
                }
            }

            //Transition de la caméra
            if (EstEnTransition == false)
            {
                if (gestionnaireProjectile.ObtenirProjectileEnMouvement() != null)
                {
                    camPartie.ObjetSuivi = gestionnaireProjectile.ObtenirProjectileEnMouvement();
                }
                else
                {
                    camPartie.ObjetSuivi = equipeActive.JoueurActif;

                    if (tempsEcouler > 0)
                    {
                        tempsEcouler -= gameTime.ElapsedGameTime.Milliseconds;
                    }                    
                    else
                    {
                        EstEnTransition = true;
                        ChangementEquipe();
                        camPartie.SeDirigerVers(equipeActive.JoueurActif);
                    }
                }
            }
            carte.Update(gameTime);
            camPartie.Update(gameTime);
            gestionnaireProjectile.MatriceDeCamera = camPartie.get_transformation(GraphicsDevice);
            gestionnaireMenusTir.MatriceDeCamera = camPartie.get_transformation(GraphicsDevice);

            //Éffacement d
            for (int nbCorps = 0; nbCorps < mondePhysique.BodyList.Count; nbCorps++)
            {
                if (mondePhysique.BodyList[nbCorps].Position.Y  > Conversion.PixelAuMetre(carte.ObtenirTailleCarte().Height)+2)
                {
                    //Si l'acteur est tombé dans le vide, changement d'équipe
                    if (mondePhysique.BodyList[nbCorps] == equipeActive.JoueurActif.ObtenirCorpsPhysique())
                    {
                        EstEnTransition = true;
                        ChangementEquipe();
                        camPartie.SeDirigerVers(equipeActive.JoueurActif);
                    }

                    mondePhysique.RemoveBody(mondePhysique.BodyList[nbCorps]);
                    mondePhysique.BodyList[nbCorps].IsDisposed = true;
                }
            }
            base.Update(gameTime);
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Jeu;
        }

        public Color ObtenirCouleurEquipePerdante()
        {
            return equipePerdante;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix() * camPartie.get_transformation(GraphicsDevice));
            carte.Draw(spriteBatch, camPartie.Pos);
            foreach (Equipe equipe in equipes)
            {
                equipe.Draw(spriteBatch);
            }
            spriteBatch.End();
            gestionnaireProjectile.Draw(gameTime);
            gestionnaireMenusTir.Draw(gameTime);
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix());

            string temps = (Math.Ceiling((float)tempsEcouler/1000)).ToString();
            if (tempsEcouler <= 9000)
            {
                CouleurSecondes = Color.Firebrick;
                StringBuilder stringBuilder = new StringBuilder(temps);
                stringBuilder.Insert(0, "0");
                temps = stringBuilder.ToString();
            }
            spriteBatch.Draw(secondesRestantes, new Vector2(IndependentResolutionRendering.Resolution.getVirtualViewport().Width / 2 - secondesRestantes.Width / 2, 0), Color.White);
            spriteBatch.DrawString(policeCompte, temps, new Vector2(IndependentResolutionRendering.Resolution.getVirtualViewport().Width / 2 - 130, 0), CouleurSecondes);
            spriteBatch.DrawString(policeNbJoueurs, equipes[0].TailleEquipe.ToString(), new Vector2(150, -5), Color.Firebrick);
            spriteBatch.DrawString(policeNbJoueurs, equipes[1].TailleEquipe.ToString(), new Vector2(IndependentResolutionRendering.Resolution.getVirtualViewport().Width - 150, -5), Color.Blue);
            CouleurSecondes = Color.DarkGray;
            spriteBatch.End();
            DrawOrder = 0;
        }

        public void ChangementEquipe()
        {
            equipeActive.FinTour();
            equipeActive = equipes[(equipes.IndexOf(equipeActive) + 1) % equipes.Count()];
            equipeActive.DebutTour();
        }


        private Vector2 PhaseApparition(ref List<Vector2> listeApparition)
        {
            //Si tout les points ont déjà  été utilisé, on reprend la liste complète.
            if (listeApparition.Count == 0)
            {
                Console.WriteLine("PartieJeu::PhaseApparition: Nombre insuffisant de points d'apparitions.");
                listeApparition = carte.ListeApparition;
                foreach  (Vector2 point in listeApparition.ToList())
                {
                    //On rajoute un peu de déphasage pour qu'il n'y a pas un acteur qui apparait exactement à la même position.
                    //Il est possible que le déphasage ammène un acteur à tomber.
                    listeApparition[listeApparition.IndexOf(point)] += new Vector2(20,0);
                }
            }
            int numHasard = Game1.hasard.Next(listeApparition.Count);
            Vector2 apparition = listeApparition[numHasard];
            listeApparition.RemoveAt(numHasard);
            return apparition / 40;
        }       
    }
}
