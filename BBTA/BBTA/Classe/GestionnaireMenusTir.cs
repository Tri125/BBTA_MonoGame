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

    public class GestionnaireMenusTir:DrawableGameComponent, IUtiliseMatriceCamera
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

        //Événements--------------------------------------------------------------
        public delegate void DelegateProcessusDeTirTerminer(Vector2 position, Vector2 vitesse, Armes type, Armement munitions);
        public event DelegateProcessusDeTirTerminer ProcessusDeTirTerminer;
        public event EventHandler TirAvorte;


        public GestionnaireMenusTir(Game jeu)
            : base(jeu)
        {

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            indicateur = new IndicateurPuissance(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Puissance"));
            indicateur.ForceFinaleDeterminee += new IndicateurPuissance.delegueForceFinaleDeterminee(indicateur_ForceFinaleDeterminee);
            texturesArmes = Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\armesPanneau");
            selecteur = new SelectionArme(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\panneauSelecteurArme"), 
                                          texturesArmes,
                                          Game.Content.Load<SpriteFont>(@"PoliceIndicateur"), 200);
            selecteur.ArmeSelectionnee += new SelectionArme.DelegateArmeSelectionnee(selecteur_ArmeSelectionnee);
            selecteur.PanneauFermer += new EventHandler(selecteur_PanneauFermer);
            viseur = new ViseurVisuel(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Viseur"));
            viseur.Verouiller += new EventHandler(viseur_Verouiller);
            base.LoadContent();
        }

        public void DemarrerSequenceTir(Vector2 position, Armement munitions)
        {
            this.position = position;
            modeEncours = ModeTir.Selection;
            selecteur.Position = new Vector2(position.X, position.Y - 50);
            selecteur.Munitions = munitions;
            viseur.Position = position;
            indicateur.Position = new Vector2(position.X, position.Y - 50);
            selecteur.estOuvert = true;
        }

        void selecteur_ArmeSelectionnee(Armes armeSelectionnee)
        {
            prochainMode = ModeTir.Visee;
            type = armeSelectionnee;
        }


        void selecteur_PanneauFermer(object sender, EventArgs e)
        {
            if (TirAvorte != null && prochainMode == ModeTir.nul)
            {
                TirAvorte(this, new EventArgs());
            }
        }

        void viseur_Verouiller(object sender, EventArgs e)
        {
            prochainMode = ModeTir.DeterminerForce;
        }

        void indicateur_ForceFinaleDeterminee(int forceFinale)
        {
            prochainMode = ModeTir.nul;
            ProcessusDeTirTerminer(Conversion.PixelAuMetre(position), viseur.ObtenirAngle() * forceFinale, type, selecteur.Munitions);
            forceFinale = 0;
        }

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
