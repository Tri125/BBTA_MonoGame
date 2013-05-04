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
using BBTA.Classe.Elements;
using BBTA.Classe.Interface;

namespace BBTA.Partie_De_Jeu
{
    public class Equipe
    {
        private readonly bool estHumain;
        public bool EstHumain { get { return estHumain; } }
        private List<Acteur> equipiers = new List<Acteur>();
        public int TailleEquipe { get { return equipiers.Count; } }
        public Acteur JoueurActif { get; private set; }
        
        public int NombreJoueursOriginel
        {
            get
            {
                return equipiers.Capacity;
            }
        }
        public Color couleur { get; set; }
        public Armement Munitions { get; set; }
        public delegate void DelegateTirDemande(Vector2 position, Armement munitions);

        public event DelegateTirDemande TirDemande;
        public event EventHandler JoueursTousMorts;
      

        public Equipe(Color couleur, int nbJoueurs, bool estHumain)
        {
            this.estHumain = estHumain;
            this.couleur = couleur;
            this.equipiers.Capacity = nbJoueurs;
            Munitions = new Armement();
        }


        public void RajoutMembre(Acteur nouveauMembre)
        {
            equipiers.Add(nouveauMembre);
            nouveauMembre.Detruit += new EventHandler(nouveauMembre_Detruit);
            nouveauMembre.TirDemande += new EventHandler(nouveauMembre_TirDemande);
        }

        void nouveauMembre_TirDemande(object sender, EventArgs e)
        {
            if (TirDemande != null)
            {
                TirDemande((sender as Acteur).ObtenirPosition(), Munitions);
            }
        }

        void nouveauMembre_Detruit(object sender, EventArgs e)
        {
            equipiers.ToList().Remove(sender as Acteur);
            if (equipiers.Count == 0 && JoueursTousMorts != null)
            {
                JoueursTousMorts(this, new EventArgs());
            }
        }

        public void RecevoirDegats(Vector2 lieuExplosion, int rayonExplosion)
        {
            for (int nbJoueurs = 0; nbJoueurs < equipiers.Count; nbJoueurs++)
            {
                equipiers[nbJoueurs].RecevoirDegat(lieuExplosion, rayonExplosion);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Acteur joueur in equipiers)
            {
                joueur.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Acteur joueur in equipiers)
            {
                joueur.Draw(spriteBatch);
            }
        }

        //Pour enregistrer le prochain joueur de l'équipe à jouer
        public void ChangementJoueur()
        {
            if (equipiers.Count != 0 && JoueurActif == null)
            {
                JoueurActif = equipiers[Game1.hasard.Next(equipiers.Count)];
            }
            else
            {
                JoueurActif = equipiers[(equipiers.IndexOf(JoueurActif) + 1) % equipiers.Count()];
            }
        }
        //Lorsque le tour est fini, le joueurActif est désactivé
        public void FinTour()
        {
            JoueurActif.monTour = false;
        }
        //Lorsque c'est le début du tour, le joueurActif est activé
        public void DebutTour()
        {
            if (JoueurActif == null)
            {
                JoueurActif = equipiers[Game1.hasard.Next(equipiers.Count)];
            }
            else
            {
                JoueurActif = equipiers[(equipiers.IndexOf(JoueurActif) + 1) % equipiers.Count()];
            }
            JoueurActif.monTour = true;
        }
    }
}
