using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interfaces;
using BBTA.Interface;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Outils;
using BBTA.Elements;

namespace BBTA.Partie_De_Jeu
{
    enum ModeTir
    {
        Selection,
        Visee,
        DeterminerForce, 
        nul
    }

    /// <summary>
    /// GestionnaireMenusTir est une classe qui permet de gérer l'ordre des différents menus intervenants dans le processus de tir.
    /// </summary>
    public class GestionnaireMenusTir:DrawableGameComponent
    {
        //Modules de tir----------------------------------------------------------
        private IndicateurPuissance indicateur;
        private SelectionArme selecteur;
        private ViseurVisuel viseur;
        private ArmeAffiche arme;

        //Variables pour les transitions------------------------------------------
        private ModeTir modeEncours = ModeTir.nul;
        private ModeTir prochainMode = ModeTir.nul;

        //Variables pour l'affichage de l'arme à l'écran--------------------------
        private Texture2D texturesArmes;
        private Armes type;

        //Variables techniques----------------------------------------------------
        private SpriteBatch spriteBatch;
        public Matrix MatriceDeCamera { get; set; }
        private Vector2 position;

        //Événements et délégués--------------------------------------------------
        public delegate void DelegateProcessusDeTirTerminer(Vector2 position, Vector2 vitesse, Armes type, Armement munitions);
        public event DelegateProcessusDeTirTerminer ProcessusDeTirTerminer;
        public event EventHandler TirAvorte;

        public GestionnaireMenusTir(Game jeu)
            : base(jeu)
        {

        }

        /// <summary>
        /// Charge le contenu nécessaire en mémoire et initialise les composants ayant besoin de ressources extérieures.
        /// </summary>
        protected override void LoadContent()
        {
            //Chargement des ressources et initialisations--------------------------------------
            spriteBatch = new SpriteBatch(GraphicsDevice);
            indicateur = new IndicateurPuissance(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Puissance"));
            texturesArmes = Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\armesPanneau");
            selecteur = new SelectionArme(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\panneauSelecteurArme"), 
                                          texturesArmes,
                                          Game.Content.Load<SpriteFont>(@"Police\PoliceIndicateur"), 200);
            viseur = new ViseurVisuel(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Viseur"));

            //Association des événement---------------------------------------------------------
            selecteur.ArmeSelectionnee += new SelectionArme.DelegateArmeSelectionnee(selecteur_ArmeSelectionnee);
            selecteur.PanneauFermer += new EventHandler(selecteur_PanneauFermer);
            viseur.Verouiller += new EventHandler(viseur_Verouiller);
            indicateur.ForceFinaleDeterminee += new IndicateurPuissance.delegueForceFinaleDeterminee(indicateur_ForceFinaleDeterminee);
            base.LoadContent();
        }

        /// <summary>
        /// Permet de déterminer quel menu doit être affiché.
        /// Ferme et ouvre les différents menus.
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            switch (modeEncours)
            {
                case ModeTir.Selection:
                    selecteur.Update(gameTime, MatriceDeCamera);
                    if (selecteur.estOuvert == false && selecteur. estDeploye == false)
                    {
                        modeEncours = prochainMode;
                        viseur.estOuvert = true;
                        arme = new ArmeAffiche(texturesArmes, type);
                        arme.Position = new Vector2(position.X, position.Y + 5);
                        arme.estOuvert = true;
                    }
                    break;
                case ModeTir.Visee:
                    viseur.Update(gameTime);
                    arme.angleRotation = Conversion.ValeurAngle(viseur.ObtenirAngle());
                    arme.Update(gameTime);
                    if (viseur.estOuvert == false)
                    {
                        modeEncours = prochainMode;
                        indicateur.estOuvert = true;
                    }
                    break;
                case ModeTir.DeterminerForce:
                    indicateur.Update(gameTime);
                    if (indicateur.estOuvert == false)
                    {
                        arme.estOuvert = false;
                        if (indicateur.estDeploye == false)
                        {
                            modeEncours = prochainMode;
                        }
                    }
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Permet de démarrer le processus de sélection des paramètres de tir en affichant les divers menus associés à cette opération.
        /// </summary>
        /// <param name="position">Position du joueur à l'attaque</param>
        /// <param name="munitions">Armement de l'équipe du joueur</param>
        public void DemarrerSequenceTir(Vector2 position, Armement munitions)
        {
            this.position = position;
            modeEncours = ModeTir.Selection;
            selecteur.Position = new Vector2(position.X, position.Y - 50); //Le menu est légèrement au-dessus de la tête du joueur
            selecteur.Munitions = munitions;
            viseur.Position = position;
            indicateur.Position = new Vector2(position.X, position.Y - 50); //Le menu est légèrement au-dessus de la tête du joueur
            selecteur.estOuvert = true;
        }

        /// <summary>
        /// Lorsqu'une arme est sélectionnée dans le menu d'arme, on passe au choix de la direction.
        /// </summary>
        /// <param name="armeSelectionnee">Arme choisie</param>
        void selecteur_ArmeSelectionnee(Armes armeSelectionnee)
        {
            prochainMode = ModeTir.Visee;
            type = armeSelectionnee;
        }

        /// <summary>
        /// Lorsque le joueur décide de ne pas choisir une arme, le gestionnaire déclanche un événement pour aviser la partie pour que le joueur puisse se déplacer à nouveau.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void selecteur_PanneauFermer(object sender, EventArgs e)
        {
            if (TirAvorte != null && prochainMode == ModeTir.nul)
            {
                TirAvorte(this, new EventArgs());
            }
        }

        public void ForceAnnule()
        {
            TirAvorte(this, new EventArgs());
            selecteur.estOuvert = false;
            if (viseur != null)
            {
                viseur.estOuvert = false;
            }
            if (indicateur != null)
            {
                indicateur.estOuvert = false;
                indicateur.RemiseAZero();
            }
            if (arme != null)
            {
                arme.estOuvert = false;
            }

        }

        /// <summary>
        /// Lorsque le joueur a décidé son angle de tir, on passe au choix de la vitesse du projectile.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void viseur_Verouiller(object sender, EventArgs e)
        {
            prochainMode = ModeTir.DeterminerForce;
        }

        /// <summary>
        /// Lorsque la vitesse est choisie, les menus sont tous fermés.
        /// Un événement est déclanché pour indiquer que le choix est fait et que le tir peut se produire.
        /// </summary>
        /// <param name="forceFinale">Vitesse finale</param>
        void indicateur_ForceFinaleDeterminee(int forceFinale)
        {
            prochainMode = ModeTir.nul;
            ProcessusDeTirTerminer(Conversion.PixelAuMetre(position), viseur.ObtenirAngle() * forceFinale, type, selecteur.Munitions);
            forceFinale = 0;
        }

        public void FermetureManuelleMenus()
        {
            switch (modeEncours)
            {
                case ModeTir.Selection:
                    selecteur.estOuvert = false;
                    break;
                case ModeTir.Visee:
                    viseur.estOuvert = false;
                    break;
                case ModeTir.DeterminerForce:
                    indicateur.estOuvert = false;
                    break;
            }
            prochainMode = ModeTir.nul;
        }

        /// <summary>
        /// Affiche le bon menu à l'écran
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, 
                              BlendState.AlphaBlend, null, null, null, null, 
                              Resolution.getTransformationMatrix() * MatriceDeCamera);
            switch (modeEncours)
            {
                case ModeTir.Selection:
                    selecteur.Draw(spriteBatch);
                    break;
                case ModeTir.Visee:
                    viseur.Draw(spriteBatch);
                    arme.Draw(spriteBatch);
                    break;
                case ModeTir.DeterminerForce:
                    indicateur.Draw(spriteBatch);
                    arme.Draw(spriteBatch);
                    break;
                default:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        } 
    }
}
