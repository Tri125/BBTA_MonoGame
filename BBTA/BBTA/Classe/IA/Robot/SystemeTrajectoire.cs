using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Elements;
using FarseerPhysics.Dynamics;
using BBTA.Carte;

namespace BBTA.IA
{
    //public class SystemeTrajectoire
    //{
    //    private Vector2 PositionTireur;
    //    private Vector2 PositionCible;
    //    private float positionY;
    //    private float vitesseIdeal;
    //    private float angleIdeal;

    //    public SystemeTrajectoire()
    //    {

    //    }

    //    public void ObtenirPositions(Vector2 tireur, Vector2 cible)
    //    {
    //        PositionTireur = tireur;
    //        PositionCible = cible;
    //    }

    //    public void DeterminerAngleVitesse(CarteJeu carte)
    //    {
    //        //Test vitesse
    //        for (float vitesse = 1; vitesse <= 30; vitesse++)
    //        {
    //            //Test angle
    //            for (float angle = 0; angle < 360; angle++)   //L'axe des y est inverse et le cercle trigonométrique va dans le sens inverse
    //            {
    //                positionY = 9.81f * (float)Math.Pow(2, PositionCible.X - PositionTireur.X) *
    //                    (1 + (float)Math.Pow(2,(float)Math.Tan(MathHelper.ToRadians(angle))))
    //                    / (2*(float)Math.Pow(2,(vitesse)))
    //                    + (PositionCible.X - PositionTireur.X) * (float)Math.Tan(MathHelper.ToRadians(angle));

    //                if (!float.IsInfinity(positionY))
    //                {
    //                    int h = 8;
    //                }

    //                //Si la courbe au point de la cible l'atteint à + ou - un pourcentage... utiliser l'angle et vitesse
    //                if (positionY <= positionY * 1.10f && positionY >= positionY * 0.90f)
    //                {
    //                    vitesseIdeal = vitesse;
    //                    angleIdeal = angle;
    //                }                    
    //            }
    //        }
    //    }

    //    public bool InterceptionTrajectoire(CarteJeu carte)
    //    {
    //        return false;
    //    }
    //}
}