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
using BBTA.Interface;
using IndependentResolutionRendering;

namespace BBTA.Classe.Partie_de_Jeu
{
    public class PartieJeu : DrawableGameComponent
    {
        private const int TEMPS_TOUR_DEFAUT = 3000;
        private readonly int tempsTour;
        private SpriteBatch spriteBatch;
        MouseState avant;
        MouseState now;

        private World mondePhysique;
        private int tempsEcouler;

        private Camera2d camPartie;
        private BBTA_MapFileBuilder chargeurCarte;
        private int[] carte1;
        private int[] carte2;
        private int[] carte3;
        private ViseurVisuel vs;
        Carte carte;
        private Texture2D _circleSprite;
        private Body _circleBody;
        private JoueurHumain sp;

        public PartieJeu(Game jeu, int tempsParTour = TEMPS_TOUR_DEFAUT)
            : base(jeu)
        {
            this.tempsTour = tempsParTour;
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
            camPartie.Pos = new Vector2(500.0f, 200.0f);
            mondePhysique = new World(new Vector2(0, 20));
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            sp = new JoueurHumain(mondePhysique, Game.Content.Load<Texture2D>(@"Ressources\test"), new Vector2(17.5f, 0f), 100, 1, 1);

            vs = new ViseurVisuel(Game.Content.Load<Texture2D>(@"Ressources\InterfaceEnJeu\Viseur"));

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

            carte = new Carte(carte3, chargeurCarte.InformationCarte().NbColonne, Game.Content.Load<Texture2D>(@"Ressources\HoraireNico"), Game.Content.Load<Texture2D>(@"Ressources\test"), mondePhysique, 40);
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
            mondePhysique.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);


            if (avant.ScrollWheelValue < now.ScrollWheelValue)
            {
                camPartie.Zoom += 0.1f;
            }
            else if (avant.ScrollWheelValue > now.ScrollWheelValue)
            {
                camPartie.Zoom -= 0.1f;
            }
            if (nowPos.X > Resolution.getVirtualViewport().Width - 50 && Resolution.getVirtualViewport().TitleSafeArea.Contains(nowPos))
            {
                camPartie.Pos = new Vector2(camPartie.Pos.X + 2, camPartie.Pos.Y);
            }
            else if (nowPos.X < 50 - Resolution.getVirtualViewport().X && Resolution.getVirtualViewport().TitleSafeArea.Contains(nowPos))
            {
                camPartie.Pos = new Vector2(camPartie.Pos.X - 2, camPartie.Pos.Y);
            }
            sp.Update(gameTime);
            vs.AssocierAujoueur(sp);
            vs.Update(gameTime);
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
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix() * camPartie.get_transformation(GraphicsDevice));
            carte.Draw(spriteBatch);
            sp.Draw(spriteBatch);
            vs.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
