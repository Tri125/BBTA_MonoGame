using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BBTA.Classe.Menus
{
    public class MenuFinDePartie : MenuArrierePlan
    {
        private EtatJeu prochainEtat;

        public MenuFinDePartie(Game game, Color equipePerdante)
            : base(game)
        {
            prochainEtat = EtatJeu.FinDePartie;
        }

        //Color.Firebrick
        //Color.Blue
    }
}
