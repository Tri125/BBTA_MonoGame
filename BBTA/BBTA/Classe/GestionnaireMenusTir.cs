using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interfaces;
using BBTA.Interface;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Classe.Outils;

namespace BBTA.Classe
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
        private IndicateurPuissance indicateur;
        private SelectionArme selecteur;
        private ViseurVisuel viseur;
        private ModeTir modeEncours = ModeTir.nul;
        private ModeTir prochainMode;
        private SpriteBatch spriteBatch;
        private Armes type;
        public delegate void DelegateProcessusDeTirTerminer(Vector2 position, Vector2 direction, float vitesse, Armes type);
        public event DelegateProcessusDeTirTerminer ProcessusDeTirTerminer;
        public Matrix MatriceDeCamera{get;set;}
        public Vector2 Position { get; set; }


        public GestionnaireMenusTir(Game jeu)
            : base(jeu)
        {

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            indicateur = new IndicateurPuissance(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Puissance"));
            indicateur.ForceFinaleDeterminee += new IndicateurPuissance.delegueForceFinaleDeterminee(indicateur_ForceFinaleDeterminee);
            List<Texture2D> armes = new List<Texture2D>();
            armes.Add(Game.Content.Load<Texture2D>(@"Ressources\Roquette"));
            selecteur = new SelectionArme(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\panneauSelecteurArme"), armes,
                                          Game.Content.Load<SpriteFont>(@"PoliceIndicateur"), 200);
            selecteur.ArmeSelectionnee += new SelectionArme.DelegateArmeSelectionnee(selecteur_ArmeSelectionnee);
            selecteur.SortieDuPanneau += new EventHandler(selecteur_SortieDuPanneau);
            viseur = new ViseurVisuel(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Viseur"), armes[0]);
            viseur.Verouiller += new EventHandler(viseur_Verouiller);
            base.LoadContent();
        }

        public void DemarrerSequenceTir()
        {
            modeEncours = ModeTir.Selection;
            selecteur.Position = new Vector2(Position.X, Position.Y - 50);
            viseur.Position = Position;
            indicateur.Position = new Vector2(Position.X, Position.Y - 50);
            selecteur.estOuvert = true;
        }

        void selecteur_ArmeSelectionnee(Armes armeSelectionnee)
        {
            prochainMode = ModeTir.Visee;
            type = armeSelectionnee;
        }


        void selecteur_SortieDuPanneau(object sender, EventArgs e)
        {
            prochainMode = ModeTir.nul;
        }

        void viseur_Verouiller(object sender, EventArgs e)
        {
            prochainMode = ModeTir.DeterminerForce;
        }

        void indicateur_ForceFinaleDeterminee(int forceFinale)
        {
            prochainMode = ModeTir.nul;
            ProcessusDeTirTerminer(Conversion.PixelAuMetre(Position), viseur.ObtenirAngle(), forceFinale, type);
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
                    }
                    break;
                case ModeTir.Visee:
                    viseur.Update(gameTime);
                    if (viseur.estOuvert == false && selecteur.estDeploye == false)
                    {
                        modeEncours = prochainMode;
                        indicateur.estOuvert = true;
                    }
                    break;
                case ModeTir.DeterminerForce:
                    indicateur.Update(gameTime);
                    if (indicateur.estOuvert == false && indicateur.estDeploye == false)
                    {
                        modeEncours = prochainMode;
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
                    break;
                case ModeTir.DeterminerForce:
                    indicateur.Draw(spriteBatch);
                    break;
                default:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        } 
    }
}
