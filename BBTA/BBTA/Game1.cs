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
using FarseerPhysics.Factories;
using BBTA.Outils;
using BBTA.Menus;

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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Carte carte;        
        World monde = new World(new Vector2(0, 20));

        //Cercle de test
        private Texture2D _circleSprite;
        private Body _circleBody;

        Camera2d cam = new Camera2d();
        MouseState avant;
        MouseState now;
        private Accueil acc;

        //Acteur test
        private Texture2D _acteurSprite;
        private Body _acteurBody;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            this.IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            //acc = new Accueil(this);
            this.Components.Add(acc);
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
            cam.Pos = new Vector2(500.0f, 200.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            /*//Chargement du _cercle
            _circleSprite = Content.Load<Texture2D>(@"Ressources\circleSprite"); //  96px x 96px => 1.5m x 1.5m
            // Convert screen center from pixels to meters
            Vector2 circlePosition = new Vector2(17, 0);
            // Create the circle fixture
            _circleBody = BodyFactory.CreateCircle(monde, 96f / (2f * 40), 1f, circlePosition);
            _circleBody.BodyType = BodyType.Dynamic;
            // Give it some bounce and friction
            _circleBody.Restitution = 0.3f;
            _circleBody.Friction = 0.5f; /* */

            //Chargement du _acteur
            _acteurSprite = Content.Load<Texture2D>(@"Ressources\Acteur\ActeurRouge"); //40x40 acteurrouge
            // Convert screen center from pixels to meters
            Vector2 rectPosition = new Vector2(11.5f, 0);
            // Create the circle fixture
            _acteurBody = BodyFactory.CreateRectangle(monde, 1f, 1f, 1f, rectPosition);
            _acteurBody.BodyType = BodyType.Dynamic;
            // Give it some bounce and friction
            _acteurBody.Restitution = 0.1f;
            _acteurBody.Friction = 0.5f;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Chargement de la carte
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

            carte = new Carte(carte2, chargeurCarte.InformationCarte().NbColonne, Content.Load<Texture2D>(@"Ressources\HoraireNico"), Content.Load<Texture2D>(@"Ressources\test"), monde, 40);

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
            avant = now;
            now = Mouse.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            monde.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            if (avant.ScrollWheelValue < now.ScrollWheelValue)
            {
                cam.Zoom += 0.1f;
            }
            else if (avant.ScrollWheelValue > now.ScrollWheelValue)
            {
                cam.Zoom -= 0.1f;
            }
            if (now.X > GraphicsDevice.Viewport.Width - 50)
            {
                cam.Pos = new Vector2(cam.Pos.X + 2, cam.Pos.Y);
            }
            else if (now.X < 50)
            {
                cam.Pos = new Vector2(cam.Pos.X - 2, cam.Pos.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            /* Circle position and rotation
            // Convert physics position (meters) to screen coordinates (pixels)
            Vector2 circlePos = _circleBody.Position * 40;
            Vector2 circleOrigin = new Vector2(_circleSprite.Width / 2f, _circleSprite.Height / 2f);
            float circleRotation = _circleBody.Rotation; /* */

            /* Acteur position and rotation */
            // Convert physics position (meters) to screen coordinates (pixels)
            Vector2 rectPos = _acteurBody.Position * 40;
            Vector2 rectOrigin = new Vector2(_acteurSprite.Width / 2f, _acteurSprite.Height / 2f);
            float rectRotation = _acteurBody.Rotation;

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(GraphicsDevice /*Send the variable that has your graphic device here*/));
            carte.Draw(spriteBatch);

            //spriteBatch.Draw(_circleSprite, circlePos, null, Color.White, circleRotation, circleOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_acteurSprite, rectPos, null, Color.White, rectRotation, rectOrigin, 1f, SpriteEffects.None, 0f);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
