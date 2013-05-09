using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Classe.Elements;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;

namespace BBTA.Classe.Menus
{
    public class MenuArrierePlan : DrawableGameComponent
    {
        //Arrière plan
        private Nuage nuage1;
        private Nuage nuage2;
        private Texture2D arrierePlan;
        protected SpriteBatch spriteBatch;

        public MenuArrierePlan(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            //Contenu arrière plan
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 vitesseNuage = new Vector2(3, 0);
            nuage1 = new Nuage(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\Nuage1"), new Vector2(100, 50), vitesseNuage, Resolution.getVirtualViewport());
            nuage2 = new Nuage(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\Nuage2"), new Vector2(1000, 50), vitesseNuage, Resolution.getVirtualViewport());
            arrierePlan = Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\ArrierePlan");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Déplacement nuage en arrière plan
            nuage1.Update(gameTime);
            nuage2.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            //Draw arrière plan
            spriteBatch.Draw(arrierePlan, Vector2.Zero, Color.White);
            nuage1.Draw(spriteBatch);
            nuage2.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
