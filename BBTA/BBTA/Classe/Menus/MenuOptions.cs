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
        private Slider sliderMusique;

        public MenuOptions(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Options;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\lettrageOption");
            btnRetour = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnRetour"), new Vector2(1200, 800), null);
            btnRetour.Clic += new EventHandler(btnRetour_Clic);
            sliderEffet = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 225),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"));

            sliderMusique = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 325),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"));
            base.LoadContent();
        }

        void btnRetour_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        public override void Update(GameTime gameTime)
        {
            btnRetour.Update(null);
            base.Update(gameTime);
            sliderEffet.Deplacement();
            sliderMusique.Deplacement();
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
