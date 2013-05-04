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

        public Equipe equipeActive { get; private set; }

        public delegate void DelegateTirEntamme(Vector2 position);
        public event DelegateTirEntamme Tir;

        public delegate void DelegateMortDunJoueur(bool joueurActif);
        public event DelegateMortDunJoueur MortDunJoueur;

        public GestionnaireActeurs(Game jeu, int nbJoueursEquipe1, int nbJoueursEquipe2, Vector2 dimensionsCarte, bool equipeAdverseHumaine)
            :base(jeu)
        {
            this.equipeAdverseHumaine = equipeAdverseHumaine;
            this.equipes.Add(new Equipe(nbJoueursEquipe1));
            this.equipes.Add(new Equipe(nbJoueursEquipe2));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureJoueur = Game.Content.Load<Texture2D>(@"Ressources\Acteur\wormsp");
            base.LoadContent();
        }

        public void CreerJoueurs(ref World mondePhysique, List<Vector2>positions)
        {
            if (joueursFurentIlsCreer == false)
            {
                joueursFurentIlsCreer = true;
                for (int compteurEquipe = 0; compteurEquipe < 2; compteurEquipe++)
                {
                    for (int compteurActeur = 0; compteurActeur < equipes[compteurEquipe].NbrMembre; compteurActeur++)
                    {
                        if (!equipeAdverseHumaine && compteurEquipe == 1)
                        {
                            
                        }
                        else
                        {
                            equipes[compteurEquipe].RajoutMembre(new JoueurHumain(mondePhysique, textureJoueur, PhaseApparition(ref positions),
                                                                                  3, 1, 75));                            
                        }
                        equipes[compteurEquipe].ListeMembres[compteurActeur].TirDemande += new EventHandler(GestionnaireActeurs_TirDemande);
                    }
                }
                equipeActive = equipes[Game1.hasard.Next(equipes.Count)];
                equipeActive.DebutTour();
            }
        }

        public void Explosion(Vector2 lieu, int rayonExplosion)
        {
            foreach (Equipe equipe in equipes)
            {
                for (int nbJoueur = 0; nbJoueur < equipe.NbrMembre; nbJoueur++)
                {
                    equipe.ListeMembres[nbJoueur].RecevoirDegat(lieu, rayonExplosion);
                }
            }

        }

        void GestionnaireActeurs_TirDemande(object sender, EventArgs e)
        {
            if (Tir != null)
            {
                Tir((sender as Acteur).ObtenirPosition());
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Equipe equipe in equipes)
            {
                for(int nbActeurs = 0; nbActeurs < equipe.NbrMembre; nbActeurs++)
                {
                    equipe.ListeMembres[nbActeurs].Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                              BlendState.AlphaBlend, null, null, null, null, 
                              Resolution.getTransformationMatrix() * matriceCamera);
            foreach (Equipe equipe in equipes)
            {
                foreach (Acteur item in equipe.ListeMembres)
                {
                    item.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ChangementEquipe()
        {
            equipeActive.FinTour();
            equipeActive = equipes[(equipes.IndexOf(equipeActive) + 1) % equipes.Count()];
            equipeActive.DebutTour();
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
