using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Elements;
using FarseerPhysics.Dynamics;

namespace BBTA.Classe.IA.Robot
{
    public class SystemeTrajectoire
    {
        private Vector2 PositionTireur;
        private Vector2 PositionCible;
        private float vitesse;
        private Vector2 Direction;
        private World mondePhysique;

        public SystemeTrajectoire()
        {

        }

        public void ObtenirPositions(Acteur tireur, Acteur cible)
        {
            PositionTireur = tireur.ObtenirPosition();
            PositionCible = cible.ObtenirPosition();
        }
    }
}