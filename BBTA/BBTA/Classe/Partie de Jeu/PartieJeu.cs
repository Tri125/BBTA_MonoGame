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
using IndependentResolutionRendering;

namespace BBTA.Classe.Partie_de_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {
        private const int TEMPS_TOUR_DEFAUT = 3000;
        private World mondePhysique;
        private int tempsEcoule;
        private int tempsTour;

        private Camera2d camPartie;
        private BBTA_MapFileBuilder chargeurCarte;
        private int[] carte1;
        private int[] carte2;
        private SpriteBatch spriteBatch;
        
        Carte carte;
        World monde;
        private Texture2D _circleSprite;
        private Body _circleBody;
        MouseState avant;
        MouseState now;

        public PartieJeu(Game jeu)
            : base(jeu)
        {
          
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            
            // TODO: Add your initialization logic here
            camPartie = new Camera2d();
            camPartie.Pos = new Vector2(500.0f,200.0f);
            monde = new World(new Vector2(0, 20));
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //Instantiation du chargeur de carte
            _circleSprite = Game.Content.Load<Texture2D>(@"Ressources\circleSprite"); //  96px x 96px => 1.5m x 1.5m
            /* Circle */
            // Convert screen center from pixels to meters
            Vector2 circlePosition = new Vector2(17,0);

            // Create the circle fixture
            _circleBody = BodyFactory.CreateCircle(monde, 96f / (2f * 40), 1f,circlePosition);
            _circleBody.BodyType = BodyType.Dynamic;

            // Give it some bounce and friction
            _circleBody.Restitution = 0.3f;
            _circleBody.Friction = 0.5f;

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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            carte = new Carte(carte2, chargeurCarte.InformationCarte().NbColonne, Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\test"), monde, 40);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            avant = now;
            now = Mouse.GetState();
            Point nowPos = Resolution.MouseHelper.CurrentMousePosition;
            // TODO: Add your update logic here
            monde.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            if (avant.ScrollWheelValue < now.ScrollWheelValue)
            {
                camPartie.Zoom += 0.1f;
            }
            else if (avant.ScrollWheelValue > now.ScrollWheelValue)
            {
                camPartie.Zoom -= 0.1f;
            }
            if (nowPos.X > Resolution.getVirtualViewport().Width - 50)
            {
                camPartie.Pos = new Vector2(camPartie.Pos.X + 2, camPartie.Pos.Y);
            }
            else if (nowPos.X < 50)
            {
                camPartie.Pos = new Vector2(camPartie.Pos.X - 2, camPartie.Pos.Y);
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            /* Circle position and rotation */
            // Convert physics position (meters) to screen coordinates (pixels)
            Vector2 circlePos = _circleBody.Position * 40;
            Vector2 circleOrigin = new Vector2(_circleSprite.Width / 2f, _circleSprite.Height / 2f);
            float circleRotation = _circleBody.Rotation;
            // TODO: Add your drawing code here
            //spriteBatch.Begin(SpriteSortMode.BackToFront,
            //            BlendState.AlphaBlend,
            //            null,
            //            null,
            //            null,
            //            null,
            //            camPartie.get_transformation(GraphicsDevice /*Send the variable that has your graphic device here*/));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            carte.Draw(spriteBatch);
            spriteBatch.Draw(_circleSprite, circlePos, null, Color.White, circleRotation, circleOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
