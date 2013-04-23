﻿using System;
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

namespace BBTA.Partie_De_Jeu
{
    public class Equipe
    {
        readonly private int numEquipe;
        private List<Acteur> membresEquipe;
        private int nombreMembres;
        private bool notreTour;
        private Acteur joueurActif;
       
        public int NumEquipe { get { return numEquipe; } }
        public int NbrMembre { get { return nombreMembres; } }
        public List<Acteur> ListeMembres { get { return membresEquipe; } }
        public Acteur JoueurActif { get { return joueurActif; } }
        public bool NotreTour { get { return notreTour; } }

        public Equipe()
        {
            this.membresEquipe = new List<Acteur>();
        }

        public Equipe(int nombreEquipiers)
            : this()
        {
            this.nombreMembres = nombreEquipiers;
        }


        public Equipe(int numEquipe, List<Acteur> membresEquipe)
        {
            this.numEquipe = numEquipe;
            this.membresEquipe = membresEquipe.ToList();
        }

        public Equipe(int numEquipe, params Acteur[] membresEquipe):this(numEquipe)
        {
            this.membresEquipe = new List<Acteur>();
            this.membresEquipe = membresEquipe.ToList();
        }


        public void RajoutMembre(Acteur nouveauMembre)
        {
            membresEquipe.Add(nouveauMembre);
        }

        public void Update(GameTime gameTime)
        {
            foreach (JoueurHumain joueur in ListeMembres)
            {
                joueur.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (JoueurHumain joueur in membresEquipe)
            {
                joueur.Draw(spriteBatch);
            }
        }

        public void SupressionMembre(Acteur ancienMembre)
        {
            foreach (Acteur membre in membresEquipe.ToList())
            {
                if (membre == ancienMembre)
                {
                    membresEquipe.Remove(ancienMembre);
                }
            }
        }

        //Pour enregistrer le prochain joueur de l'équipe à jouer
        public void ChangementJoueur()
        {
            if (membresEquipe.Count != 0 && joueurActif == null)
            {
                joueurActif = membresEquipe[Game1.hasard.Next(membresEquipe.Count)];
            }
            else
            {
                joueurActif = membresEquipe[(membresEquipe.IndexOf(joueurActif) + 1) % membresEquipe.Count()];
            }
        }
        //Lorsque le tour est fini, le joueurActif est désactivé
        public void FinTour()
        {
            joueurActif.monTour = false;
        }
        //Lorsque c'est le début du tour, le joueurActif est activé
        public void DebutTour()
        {
            joueurActif.monTour = true;
        }
        //Pour la transition entre équipe ----- LARGEMENT MAL FAIT POUR LE MOMENT
        public void ChangementEquipe()
        {
            if (notreTour == false)
            {
                notreTour = true;
                return;
            }


            if (notreTour == true)
            {
                notreTour = false;
                return;
            }
        }
    }
}
