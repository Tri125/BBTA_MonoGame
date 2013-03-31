using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using XNATileMapEditor;
using BBTA.Elements;

namespace BBTA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private BBTA_MapFileBuilder chargeurCarte;
        private int[] carte1;
        private int[] carte2;
        private int[] carte3;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Carte carte;
        int[] blocs = new int[49] { 0, 1, 1, 1, 0, 1, 0, 1, 1, 0,1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,1,0,1,0,1,1,1,1,0,0};
        World monde = new World(new Vector2(0, 9.81f));

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Instantiation du chargeur de carte
            chargeurCarte = new BBTA_MapFileBuilder();
            chargeurCarte.LectureCarte(@"Carte Jeu\rectangle.xml");
            if (chargeurCarte.ChargementReussis)
            {
                carte1 = chargeurCarte.InfoTuileTab();
            }

            chargeurCarte.LectureCarte(@"Carte Jeu\escalator.xml");
            if (chargeurCarte.ChargementReussis)
            {
                carte2 = chargeurCarte.InfoTuileTab();
            }

            chargeurCarte.LectureCarte(@"Carte Jeu\lgHill.xml");
            if (chargeurCarte.ChargementReussis)
            {
                carte3 = chargeurCarte.InfoTuileTab();
            }
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            carte = new Carte(blocs, 20, Content.Load<Texture2D>(@"Ressources\HoraireNico"), Content.Load<Texture2D>(@"Ressources\test"), monde, 40);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            monde.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            carte.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
