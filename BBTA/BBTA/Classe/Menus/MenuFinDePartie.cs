﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BBTA.Interface;
using IndependentResolutionRendering;

namespace BBTA.Classe.Menus
{
    public class MenuFinDePartie : MenuArrierePlan
    {
        private Texture2D lettrage;
        private Bouton btnAccueil;
        private EtatJeu prochainEtat;
        private Color equipePerdante;

        public MenuFinDePartie(Game game, Color equipePerdante)
            : base(game)
        {
            prochainEtat = EtatJeu.FinDePartie;
            this.equipePerdante = equipePerdante;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageFinDePartie");
            btnAccueil = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\BoutonAccueilPNG"), new Vector2(1175, 600), null);
            btnAccueil.Clic += new EventHandler(btnAccueil_Clic);
        }

        void btnAccueil_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            btnAccueil.Update(null);
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.FinDePartie;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            btnAccueil.Draw(spriteBatch);

            spriteBatch.End();
        }




        //Color.Firebrick
        //Color.Blue
    }
}
