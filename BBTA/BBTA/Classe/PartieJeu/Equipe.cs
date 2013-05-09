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

namespace BBTA.Partie_De_Jeu
{
    public class Equipe
    {
        private readonly bool estHumain;
        private bool EstPerdante;
        private bool policeChargee = false;
        public bool EstHumain { get { return estHumain; } }
        private List<Acteur> equipiers = new List<Acteur>();
        public int TailleEquipe { get { return equipiers.Count; } }
        public Acteur JoueurActif { get; set; }
        private SpriteFont policeAffichage;
        
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

        public void ChargerPolice(SpriteFont police)
        {
            this.policeAffichage = police;
            policeChargee = true;
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
            equipiers.Remove(sender as Acteur);
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
            foreach (Acteur joueur in equipiers.ToList())
            {
                joueur.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Acteur joueur in equipiers)
            {
                joueur.Draw(spriteBatch);
                if (policeChargee == true && joueur.enModeTir == false)
                {
                    string vie = ((int)Math.Ceiling((double)joueur.Vies)).ToString();
                    spriteBatch.DrawString(policeAffichage, vie,
                                           new Vector2((joueur.ObtenirPosition().X - policeAffichage.MeasureString(vie).X / 2f), 
                                                       (int)(joueur.ObtenirPosition().Y - 60)), couleur);
                }
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
            if (JoueurActif == null && equipiers.Count() != 0)
            {
                JoueurActif = equipiers[Game1.hasard.Next(equipiers.Count)];
            }
            else
            {
                JoueurActif = equipiers[(equipiers.IndexOf(JoueurActif) + 1) % equipiers.Count()];
            }
            JoueurActif.monTour = true;
        }

        public bool DeterminerEquipePerdante()
        {
            if (equipiers.Count() == 0)
            {
                return true;
            }
            return false;
        }

        public bool ObtenirEtatDefaite()
        {
            return EstPerdante;
        }
    }
}
