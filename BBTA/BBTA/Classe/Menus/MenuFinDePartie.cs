using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBTA.Classe.Menus
{
    public class MenuFinDePartie : MenuArrierePlan
    {
        private Texture2D lettrage;
        private EtatJeu prochainEtat;
        private Color equipePerdante;

        public MenuFinDePartie(Game game, Color equipePerdante)
            : base(game)
        {
            prochainEtat = EtatJeu.FinDePartie;
            this.equipePerdante = equipePerdante;
        }


        //Color.Firebrick
        //Color.Blue
    }
}
