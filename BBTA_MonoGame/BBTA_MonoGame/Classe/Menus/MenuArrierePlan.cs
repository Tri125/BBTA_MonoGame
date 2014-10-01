using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Elements;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;

namespace BBTA.Menus
{
    public class MenuArrierePlan : DrawableGameComponent
    {
        
        private Nuage nuage1;
        private Nuage nuage2;
        private Texture2D arrierePlan;
        protected SpriteBatch spriteBatch;

        /// <summary>
        /// Constructeur de base pour la classe MenuArrierePlan
        /// </summary>
        /// <param name="game"></param>
        public MenuArrierePlan(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Chargement des textures pour l'arrière plan
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 vitesseNuage = new Vector2(3, 0);
            nuage1 = new Nuage(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\Nuage1"), new Vector2(100, 50), vitesseNuage, Resolution.getVirtualViewport());
            nuage2 = new Nuage(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\Nuage2"), new Vector2(1000, 50), vitesseNuage, Resolution.getVirtualViewport());
            arrierePlan = Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\ArrierePlan");
            base.LoadContent();
        }

        /// <summary>
        /// Mise à jour des éléments de l'arriere plan
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            nuage1.Update(gameTime);
            nuage2.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Affichage des éléments de l'arrière plan
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            
            //Lettrage
            spriteBatch.Draw(arrierePlan, Vector2.Zero, Color.White);

            //Nuages
            nuage1.Draw(spriteBatch);
            nuage2.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
