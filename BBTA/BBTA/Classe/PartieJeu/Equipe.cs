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
    /// <summary>
    /// Equipe est une classe réunissant des Acteurs (qu'ils soient humains ou contrôlés par l'ordinateur)
    /// Elle décide du joueur qui doit jouer.
    /// Elle affiche la vie des joueurs au-dessus de leur tête avec la bonne couleur.
    /// </summary>
    public class Equipe
    {
        //Ressources et affichage---------------------------------------------------------------------------
        private SpriteFont policeAffichage;
        private bool policeChargee = false;

        //États---------------------------------------------------------------------------------------------
        private readonly bool estHumain;
        private bool EstPerdante;

        //Joueurs-------------------------------------------------------------------------------------------
        private List<Acteur> equipiers = new List<Acteur>();

        //Propriétés----------------------------------------------------------------------------------------
        public int NombreJoueursOriginel { get { return equipiers.Capacity; } }
        public Color couleur { get; set; }
        public Armement Munitions { get; set; }
        public bool EstHumain { get { return estHumain; } }
        public int TailleEquipe { get { return equipiers.Count; } }
        public Acteur JoueurActif { get; set; }

        //Événements et délégués----------------------------------------------------------------------------
        public delegate void DelegateTirDemande(Vector2 position, Armement munitions);
        public event DelegateTirDemande TirDemande;
        public event EventHandler JoueursTousMorts;
      
        /// <summary>
        /// Cosntructeur
        /// </summary>
        /// <param name="couleur">Couleur de l'équipe</param>
        /// <param name="nbJoueurs">Nombre de joueur initial dans l'équipe</param>
        /// <param name="estHumain">Si c'est une équipe contrôlé par un joueur humain</param>
        public Equipe(Color couleur, int nbJoueurs, bool estHumain)
        {
            this.estHumain = estHumain;
            this.couleur = couleur;
            this.equipiers.Capacity = nbJoueurs;
            Munitions = new Armement();
        }

        /// <summary>
        /// Permet de cherger la police pour l'affichage de la quantité de vie restante
        /// </summary>
        /// <param name="police"></param>
        public void ChargerPolice(SpriteFont police)
        {
            this.policeAffichage = police;
            policeChargee = true;
        }

        /// <summary>
        /// Permet de rajouté un membre à l'équipe
        /// </summary>
        /// <param name="nouveauMembre">Nouveau membre de l'équipe</param>
        public void RajoutMembre(Acteur nouveauMembre)
        {
            equipiers.Add(nouveauMembre);
            nouveauMembre.Detruit += new EventHandler(nouveauMembre_Detruit);
            nouveauMembre.TirDemande += new EventHandler(nouveauMembre_TirDemande);
        }

        /// <summary>
        /// Est appelée lorsque le joueur désire effectuer un tir.
        /// Déclanche un événement transmettant la position du joueur et l'armement disponible de l'équipe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void nouveauMembre_TirDemande(object sender, EventArgs e)
        {
            if (TirDemande != null)
            {
                TirDemande((sender as Acteur).ObtenirPosition(), Munitions);
            }
        }

        /// <summary>
        /// Le joueur est retiré de la liste lorsqu'il meurt.
        /// Un événement est déclanché lorsque l'ensemble de l'équipe est morte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void nouveauMembre_Detruit(object sender, EventArgs e)
        {
            equipiers.Remove(sender as Acteur);
            if (equipiers.Count == 0)
            {
                EstPerdante = true;
               // JoueursTousMorts(this, new EventArgs());
            }
        }

        /// <summary>
        /// Est appelée lorsqu'une explosion se produit.
        /// Distribue les dégats aux joueurs
        /// </summary>
        /// <param name="lieuExplosion">Lieu de l'explosion</param>
        /// <param name="rayonExplosion">Rayon de l'explosion</param>
        public void RecevoirDegats(Vector2 lieuExplosion, int rayonExplosion)
        {
            for (int nbJoueurs = 0; nbJoueurs < equipiers.Count; nbJoueurs++)
            {
                equipiers[nbJoueurs].RecevoirDegat(lieuExplosion, rayonExplosion);
            }
        }

        /// <summary>
        /// Met à jour la position et l'état des joueurs
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public void Update(GameTime gameTime)
        {
            foreach (Acteur joueur in equipiers.ToList())
            {
                joueur.Update(gameTime);
            }
        }

        /// <summary>
        /// Affiche l'ensemble des joueurs de l'équipe ainsi que leur vie respective.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Acteur joueur in equipiers)
            {
                joueur.Draw(spriteBatch);
                //Si le joueur n'est pas en mode tir et qu'une police fut chargée, alors on affiche son nombre de vie restant au-dessus de sa tête.
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
            //Si c'est la première fois, on prend un joueur au hasard.
            if (JoueurActif == null && equipiers.Count() != 0)
            {
                JoueurActif = equipiers[Game1.hasard.Next(equipiers.Count)];
            }
            //Sinon, on prend un joueur au hasard.
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
