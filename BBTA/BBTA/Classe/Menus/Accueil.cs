﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interface;
using BBTA.Elements;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;

namespace BBTA.Menus
{
    enum Etats
    {
        Accueil,
        Options,
        Configuration,
        Jeu,
        Victoire,
        Defaite,
        Pause
    }


    public class Accueil : DrawableGameComponent
    {
        private Nuage nuage1;
        private Nuage nuage2;
        private Bouton btnJouer;
        private Bouton btnOptions;
        private Bouton btnQuitter;
        private Texture2D lettrage;
        private Texture2D arrierePlan;
        private SpriteBatch spriteBatch;
        Texture2D dummyTexture;

        public Accueil(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 vitesseNuage = new Vector2(3, 0);
            nuage1 = new Nuage(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\Nuage1"), new Vector2(100, 50), vitesseNuage, Resolution.getVirtualViewport());
            nuage2 = new Nuage(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\Nuage2"), new Vector2(1000, 50), vitesseNuage, Resolution.getVirtualViewport());
            arrierePlan = Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\ArrierePlan");
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\lettrage");

            btnJouer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnJouer"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 500));
            btnOptions = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnOptions"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 625));
            btnQuitter = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnQuitter"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 750));
            Console.WriteLine(GraphicsDevice.Viewport.Width / 2f + "750");
            Console.WriteLine(btnQuitter.position);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            nuage1.Update(gameTime);
            nuage2.Update(gameTime);
            if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Console.WriteLine("Souris X" + Microsoft.Xna.Framework.Input.Mouse.GetState().X + "Souris Y" + Microsoft.Xna.Framework.Input.Mouse.GetState().Y);
                Console.WriteLine(btnQuitter.position);
            }
            if (btnQuitter.ClicComplet())
            {
                Game.Exit();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(arrierePlan, Vector2.Zero, Color.White);
            nuage1.Draw(spriteBatch);
            nuage2.Draw(spriteBatch);
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            btnJouer.Draw(spriteBatch);
            btnOptions.Draw(spriteBatch);
            btnQuitter.Draw(spriteBatch);
           // spriteBatch.Draw(dummyTexture, btnQuitter.position2, Color.AliceBlue);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
