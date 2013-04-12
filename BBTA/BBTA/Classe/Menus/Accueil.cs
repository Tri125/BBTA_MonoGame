using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interface;
using BBTA.Elements;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Classe.Menus;

namespace BBTA.Menus
{
    public class Accueil : MenuArrierePlan
    {        
        //Propre à accueil
        private Texture2D lettrage;
        public Bouton btnJouer;
        public Bouton btnOptions;
        public Bouton btnQuitter;

        public Accueil(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            //Contenu propre au menu Accueil
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\lettrage");
            btnJouer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnJouer"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 500));
            btnOptions = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnOptions"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 625));
            btnQuitter = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnQuitter"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 750));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            
            //Éléments
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            btnJouer.Draw(spriteBatch);
            btnOptions.Draw(spriteBatch);
            btnQuitter.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
