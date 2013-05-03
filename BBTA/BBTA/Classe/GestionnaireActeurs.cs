using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Partie_De_Jeu;
using BBTA.Elements;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using IndependentResolutionRendering;
using BBTA.Classe.Elements;
using BBTA.Classe.Interface;

namespace BBTA.Classe
{
    public class GestionnaireActeurs:DrawableGameComponent
    {
        public Matrix matriceCamera{get;set;}
        private Texture2D textureJoueur;
        private bool joueursFurentIlsCreer = false;
        private bool equipeAdverseHumaine;
        private List<Equipe> equipes = new List<Equipe>();
        private SpriteBatch spriteBatch;
        //private AffichageNom infosJoueur;

        public Equipe equipeActive { get; private set; }

        public delegate void DelegateTirEntamme(Vector2 position, Armement munitions);
        public event DelegateTirEntamme Tir;

        public delegate void DelegateMortDunJoueur(bool joueurActif);
        public event DelegateMortDunJoueur MortDunJoueur;

        public GestionnaireActeurs(Game jeu, int nbJoueursEquipe1, int nbJoueursEquipe2, Vector2 dimensionsCarte, bool equipeAdverseHumaine)
            :base(jeu)
        {
            this.equipeAdverseHumaine = equipeAdverseHumaine;
            this.equipes.Add(new Equipe(Color.Blue, nbJoueursEquipe1));
            this.equipes.Add(new Equipe(Color.Firebrick, nbJoueursEquipe2));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureJoueur = Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp");
            infosJoueur = new AffichageNom(Game.Content.Load<SpriteFont>(@"CompteRebours"), Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\vie"));
            base.LoadContent();
        }

        public void CreerJoueurs(ref World mondePhysique, List<Vector2>positions)
        {
            if (joueursFurentIlsCreer == false)
            {
                joueursFurentIlsCreer = true;
                for (int compteurEquipe = 0; compteurEquipe < 2; compteurEquipe++)
                {
                    for (int compteurActeur = 0; compteurActeur < equipes[compteurEquipe].NombreJoueursOriginel; compteurActeur++)
                    {
                        if (!equipeAdverseHumaine && compteurEquipe == 1)
                        {
                            
                        }
                        else
                        {
                            equipes[compteurEquipe].RajoutMembre(new JoueurHumain(mondePhysique, textureJoueur, PhaseApparition(ref positions),
                                                                                  3, 1, 75));                            
                        }
                    }
                    equipes[compteurEquipe].TirDemande += new Equipe.DelegateTirDemande(GestionnaireActeurs_TirDemande);
                }
                equipeActive = equipes[Game1.hasard.Next(equipes.Count)];
                equipeActive.DebutTour();
                infosJoueur.estOuvert = true;
            }
        }

        void GestionnaireActeurs_TirDemande(Vector2 position, Armement munitions)
        {
            if (Tir != null)
            {
                Tir(position, munitions);
                infosJoueur.estOuvert = false;
            }
        }

        public void Explosion(Vector2 lieu, int rayonExplosion)
        {
            foreach (Equipe equipe in equipes)
            {
                equipe.RecevoirDegats(lieu, rayonExplosion);
            }

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Equipe equipe in equipes)
            {
                equipe.Update(gameTime);
            }
            infosJoueur.Position = equipeActive.JoueurActif.ObtenirPosition();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                              BlendState.AlphaBlend, null, null, null, null, 
                              Resolution.getTransformationMatrix() * matriceCamera);
            foreach (Equipe equipe in equipes)
            {
                equipe.Draw(spriteBatch);
            }
            infosJoueur.Draw(spriteBatch, Color.Red, equipeActive.JoueurActif.ObtenirPosition(), "90");

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ChangementEquipe()
        {
            equipeActive.FinTour();
            equipeActive = equipes[(equipes.IndexOf(equipeActive) + 1) % equipes.Count()];
            equipeActive.DebutTour();
            infosJoueur.estOuvert = true;
        }


        private Vector2 PhaseApparition(ref List<Vector2> listeApparition)
        {
            int numHasard = Game1.hasard.Next(listeApparition.Count);
            Vector2 apparition = listeApparition[numHasard];
            listeApparition.RemoveAt(numHasard);
            return apparition / 40;
        }        
    }
}
