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

namespace BBTA.Classe.Partie_de_Jeu
{
    public class Equipe
    {
        readonly private int numEquipe;
        private List<Acteur> membresEquipe;

        public int NumEquipe { get { return numEquipe; } }
        public int NbrMembre { get { return membresEquipe.Count(); } }



        public Equipe(int numEquipe, List<Acteur> membresEquipe)
        {
            this.numEquipe = numEquipe;
            this.membresEquipe = new List<Acteur>();
            this.membresEquipe = membresEquipe.ToList();
        }

        public Equipe(int numEquipe, params Acteur[] membresEquipe)
        {
            this.numEquipe = numEquipe;
            this.membresEquipe = new List<Acteur>();
            this.membresEquipe = membresEquipe.ToList();
        }

        public void RajoutMembre(Acteur nouveauMembre)
        {
            this.membresEquipe.Add(nouveauMembre);
        }

        public void SupressionMembre(Acteur nouveauMembre)
        {
            this.membresEquipe.Remove(nouveauMembre);
        }
    }
}
