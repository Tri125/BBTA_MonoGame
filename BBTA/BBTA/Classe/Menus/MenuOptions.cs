using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Interface;
using BBTA.Classe.Interface;

namespace BBTA.Classe.Menus
{
    public class MenuOptions : MenuArrierePlan
    {
        private Texture2D lettrage;
        private Bouton btnRetour;
        private EtatJeu prochainEtat;

        private Slider sliderEffet;
        private int pourcentageEffet;

        private Slider sliderMusique;
        private int pourcentageMusique;

        public MenuOptions(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Options;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\lettrageOption");
            btnRetour = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnRetour"), new Vector2(1200, 800));
            sliderEffet = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 225),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"));

            sliderMusique = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 325),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (btnRetour.ClicComplet())
            {
                prochainEtat = EtatJeu.Accueil;
            }
            sliderEffet.Deplacement();
            pourcentageEffet = sliderEffet.ObtenirPourcentage();

            sliderMusique.Deplacement();
            pourcentageMusique = sliderMusique.ObtenirPourcentage();
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Options;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            btnRetour.Draw(spriteBatch);
            sliderEffet.Draw(spriteBatch);
            sliderMusique.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
