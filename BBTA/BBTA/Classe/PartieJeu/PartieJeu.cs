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
using BBTA.Elements;
using FarseerPhysics.Factories;
using BBTA.Outils;
using BBTA.Menus;
using BBTA.Interface;
using IndependentResolutionRendering;
using FarseerPhysics.Collision.Shapes;
using System.Timers;
using System.Text;
using BBTA.IA;
using BBTA.Carte;

namespace BBTA.Partie_De_Jeu
{
    /// <summary>
    /// PartieJeu est la classe qui constitue le jeu.  
    /// Elle détient les équipes, change les tours, fait intervenirs les différents menus.
    /// Elle fait interagir tous les éléments de BBTA ensemble.
    /// C'est un composant
    /// </summary>
    public class PartieJeu : DrawableGameComponent
    {
        //Compte à rebours d'un tour ----------------------------------------------------------------------------------------------------
        private const int TEMPS_TOUR_DEFAUT = 30000; //Temps d'un tour par défaut
        private readonly int tempsTour;
        private int tempsEcouler;
        private Timer compteReboursApresTir = new Timer(2000); 

        //Ressources et variables pour l'affichage --------------------------------------------------------------------------------------
        private SpriteBatch spriteBatch;
        private SpriteFont policeCompte;
        private SpriteFont policeNbJoueurs;
        private Texture2D secondesRestantes;
        private Color CouleurSecondes = Color.DarkGray;
        private Color equipePerdante;

        //Éléments de jeu ---------------------------------------------------------------------------------------------------------------
        private CarteJeu carte;
        private int[] carteTuile;
        private List<Equipe> equipes = new List<Equipe>();
        private Equipe equipeActive;
        private Option.Option.ParametreTouche optionClavier = Game1.chargeurOption.OptionActive.InformationTouche;

        //Caméra-------------------------------------------------------------------------------------------------------------------------
        private bool EstEnTransition = false; //Signifie que la caméra se déplace
        private Camera2d camPartie;

        //Variables Farseer--------------------------------------------------------------------------------------------------------------
        private World mondePhysique;

        //Composants autonomes-----------------------------------------------------------------------------------------------------------
        GestionnaireMenusTir gestionnaireMenusTir;
        GestionnaireProjectile gestionnaireProjectile;

        //Logique du jeu-----------------------------------------------------------------------------------------------------------------
        private EtatJeu prochainEtat;
        
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="jeu">Jeu XNA</param>
        /// <param name="carteTuile">Grille de tuiles issue de la carte chargée</param>
        /// <param name="dimensionsCarte">Dimensions de la carte (en blocs)</param>
        /// <param name="nbrEquipe1">Nombre de joueurs initial de l'équipe 1</param>
        /// <param name="nbrEquipe2">Nombre de joueurs initial de l'équipe 2</param>
        /// <param name="tempsParTour">Temps disponible pour chaque tour.</param>
        public PartieJeu(Game jeu, int[] carteTuile, Vector2 dimensionsCarte, int nbrEquipe1, int nbrEquipe2, int tempsParTour = TEMPS_TOUR_DEFAUT)
            : base(jeu)
        {
            prochainEtat = EtatJeu.Jeu;
            this.carteTuile = carteTuile;
            this.tempsTour = tempsParTour;
            tempsEcouler = tempsTour;
            mondePhysique = new World(new Vector2(0, 20));

            //Initialisation des équipes
            equipes.Add(new Equipe(Color.Firebrick, nbrEquipe1, true));
            equipes.Add(new Equipe(Color.Blue, nbrEquipe2, true));
            foreach (Equipe equipe in equipes)
            {
                equipe.TirDemande += new Equipe.DelegateTirDemande(equipe_TirDemande);
            }

            gestionnaireMenusTir = new GestionnaireMenusTir(jeu);
            gestionnaireProjectile = new GestionnaireProjectile(jeu, ref mondePhysique);
            //L'affichage des composants autonome est géré par cette classe et non par le jeu lui-même.
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
            //Chargement des ressources et initialisations des objets les utilisant
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            carte = new CarteJeu(carteTuile, Game1.chargeurCarte.InformationCarte().NbColonne, Game1.chargeurCarte.InformationCarte().NbRange,
                                 Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\blocs"), 
                                 mondePhysique, 40);
            Texture2D textureJoueur = Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp");
            policeCompte = Game.Content.Load<SpriteFont>(@"Police\CompteRebours");
            secondesRestantes = Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\SecondesRestantes");
            policeNbJoueurs = Game.Content.Load<SpriteFont>(@"Police\PoliceNbJoueursVie");

            //Création des joueurs
            List<Vector2> listeApparition = carte.ListeApparition;    
            foreach (Equipe equipe in equipes)
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
            // Une équipe est désormais en jeu
            equipeActive = equipes[0];
            equipeActive.DebutTour();
        }

        /// <summary>
        /// Permet l'éxécution du jeu à proprement parlé
        /// Changements d'équipes, menus de tirs, projectiles, explosions, caméra, tout y est.
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState clavier = Keyboard.GetState(); //État actuel du clavier
            if (clavier.IsKeyDown(optionClavier.Pause))
            {
                prochainEtat = EtatJeu.Pause;
            }

            //Mise à jour du monde physique Farseer
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
                //S'il y un projectile dans les airs, la caméra suit ce dernier
                if (gestionnaireProjectile.ObtenirProjectileEnMouvement() != null)
                {
                    camPartie.ObjetSuivi = gestionnaireProjectile.ObtenirProjectileEnMouvement();
                }
                //Autrement, elle suit le joueur actif.
                else
                {
                    camPartie.ObjetSuivi = equipeActive.JoueurActif;
                    if (tempsEcouler > 0)
                    {
                        tempsEcouler -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                    //Si le temps est écoulé, on change de joueur et la caméra se dirige vers se dernier.
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

            //Elimination des objets qui tombent hors de la carte. Changement de tour en conséquence s'il y a lieu
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
                    mondePhysique.BodyList[nbCorps].IsDisposed = true; //Lorsque l'objet détenant ce corps entrera dans son Update, il se détruira.
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Lorsqu'un joueur désire tirer, le gestionnaire de tir affiche les menus en conséquence
        /// </summary>
        /// <param name="position">Position du joueur</param>
        /// <param name="munitions">Armement disponible de l'équipe</param>
        void equipe_TirDemande(Vector2 position, Armement munitions)
        {
            gestionnaireMenusTir.DemarrerSequenceTir(equipeActive.JoueurActif.ObtenirPosition(), munitions);
        }

        /// <summary>
        /// Lorsque le gestionnaire met fin à ses activités, que ce soit parce que le projectile a explosé ou parce que la mine s'est posé.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gestionnaireProjectile_ProcessusTerminer(object sender, EventArgs e)
        {
            //Un délai existe pour empêcher de se diriger immédiatement vers le joueur suivant et laisser le temps de voir les dégats.
            //Si il y a eu explosion, le chronomètre est déjà démarré, nul besoin de le faire à nouveau.
            if (compteReboursApresTir.Enabled == false)
            {
                compteReboursApresTir.Elapsed += new ElapsedEventHandler(compteReboursApresTir_Elapsed);
                compteReboursApresTir.Start();

                ChangementEquipe();
            }
            EstEnTransition = true;
        }

        /// <summary>
        /// Si le joueur décide de ne pas choisir d'armes, alors il peut continuer à jouer tant que le temps de son tour n'est pas expiré
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gestionnaireMenusTir_TirAvorte(object sender, EventArgs e)
        {
            equipeActive.JoueurActif.enModeTir = false;
        }

        /// <summary>
        /// Lorsqu'un projectile explose, le chronomètre démarre pour laisser un délai avant le déplacement de la caméra.
        /// Les dégats sont appliqués aux joueurs et à la carte.
        /// L'équipe active est modifiée 
        /// </summary>
        /// <param name="position">Position de l'explosion</param>
        /// <param name="rayonExplosion">Rayon de l'explosion</param>
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

        /// <summary>
        /// Lorsque le chronomètre après un tir est terminé, la caméra se déplace vers le prochain joueur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void compteReboursApresTir_Elapsed(object sender, ElapsedEventArgs e)
        {
            compteReboursApresTir.Stop();
            camPartie.SeDirigerVers(equipeActive.JoueurActif);
        }

        /// <summary>
        /// Lorsque la caméra est rendue à destination, le jeu peut reprendre comme normalement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void camPartie_Verouiller(object sender, EventArgs e)
        {
            EstEnTransition = false;
            tempsEcouler = tempsTour;
        }

        /// <summary>
        /// Lorsque le joueur a sélectionné toutes les paramètres d'un tir, le projectile est créé et le tour du joueur est terminé.
        /// Les munitions de l'équipe sont mises à jour.
        /// </summary>
        /// <param name="position">Position du joueur</param>
        /// <param name="vitesse">Vitesse du projectile</param>
        /// <param name="type">Type de projectile</param>
        /// <param name="munitions">Armement de l'équipe</param>
        void gestionnaireMenusTir_ProcessusDeTirTerminer(Vector2 position, Vector2 vitesse, Armes type, Armement munitions)
        {
            equipeActive.Munitions = munitions;
            gestionnaireProjectile.CreerProjectile(ref mondePhysique, position, vitesse, type);
            equipeActive.JoueurActif.enModeTir = false;
        }

        /// <summary>
        /// Permet de modifier l'équipe en jeu
        /// </summary>
        public void ChangementEquipe()
        {
            equipeActive.FinTour();
            equipeActive = equipes[(equipes.IndexOf(equipeActive) + 1) % equipes.Count()];
            equipeActive.DebutTour();
        }

        /// <summary>
        /// À partir des points d'apparitions de la carte, le jeu détermine un emplacement de départ pour un joueur
        /// </summary>
        /// <param name="listeApparition">Coordonnées d'apparition</param>
        /// <returns>Coordonnées d'apparition du joueur</returns>
        private Vector2 PhaseApparition(ref List<Vector2> listeApparition)
        {
            //Si tout les points ont déjà  été utilisé, on reprend la liste complète.
            if (listeApparition.Count == 0)
            {
                Console.WriteLine("PartieJeu::PhaseApparition: Nombre insuffisant de points d'apparitions.");
                listeApparition = carte.ListeApparition;
                foreach (Vector2 point in listeApparition.ToList())
                {
                    //On rajoute un peu de déphasage pour qu'il n'y a pas un acteur qui apparait exactement à la même position.
                    //Il est possible que le déphasage ammène un acteur à tomber.
                    listeApparition[listeApparition.IndexOf(point)] += new Vector2(20, 0);
                }
            }
            int numHasard = Game1.hasard.Next(listeApparition.Count);
            Vector2 apparition = listeApparition[numHasard]; //Sélection au hasard d'une coordonnée.
            listeApparition.RemoveAt(numHasard); //La coordonnée est retirée de la liste pour ne pas être réutilisée
            return Conversion.PixelAuMetre(apparition);
        }  

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        //Remet le jeu à zéro en vue d'une nouvelle partie
        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Jeu;
        }

        public Color ObtenirCouleurEquipePerdante()
        {
            return equipePerdante;
        }

        /// <summary>
        /// Affiche l'ensemble des éléments du jeu à l'écran.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Draw(GameTime gameTime)
        {
            //Affichage de la carte et des joueurs.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, 
                              Resolution.getTransformationMatrix() * camPartie.get_transformation(GraphicsDevice));
            carte.Draw(spriteBatch, camPartie.Pos);
            foreach (Equipe equipe in equipes)
            {
                equipe.Draw(spriteBatch);
            }
            spriteBatch.End();
            ///Affichage des composants autonomes
            gestionnaireProjectile.Draw(gameTime);
            gestionnaireMenusTir.Draw(gameTime);
            base.Draw(gameTime);

            //Affichage de l'indicateur de temps et de joueurs restants
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix());

            string temps = (Math.Ceiling((float)tempsEcouler/1000)).ToString();

            //Un "zéro" est ajouté devant un nombre de seconde inférieur à 10. De même, les secondes deviennent rouges
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
        }     
    }
}
