using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IndependentResolutionRendering;
using BBTA.Classe.Outils;

namespace BBTA.Classe.Interface
{
    public class SelecteurCarte:DrawableGameComponent
    {
        private SpriteFont police;
        private Rectangle dimmensions;
        private Texture2D fond;
        private Texture2D textureSelectionnee;
        private Carte carte;
        private Texture2D blocs;
        private Texture2D arriereplan;
        private SpriteBatch spriteBatch;
        private string[] cheminsAcces;
        private int tempsDepuisSlection;
        private const int TEMPS_TRANSITION = 100;
        private int numCarteEnCours = 0;
        private int deplacementHorizontalCarte = 0;


        public SelecteurCarte(Game jeu, Rectangle dimmensions, string[] chemins)
            :base(jeu)
        {
            this.dimmensions = dimmensions;
            this.cheminsAcces = chemins;
            Game1.chargeurCarte.LectureCarte(cheminsAcces[numCarteEnCours]);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            police = Game.Content.Load<SpriteFont>(@"PoliceIndicateur");
            blocs = Game.Content.Load<Texture2D>(@"Ressources\blocs");
            arriereplan = Game.Content.Load<Texture2D>(@"Ressources\HoraireNico");
            carte = new Carte(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne, Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            tempsDepuisSlection += gameTime.ElapsedGameTime.Milliseconds;
            if (tempsDepuisSlection >= TEMPS_TRANSITION)
            {
                tempsDepuisSlection -= TEMPS_TRANSITION;
                if(Keyboard.GetState().IsKeyDown(Keys.Up) && numCarteEnCours < cheminsAcces.Length-1)
                {
                    numCarteEnCours++;
                    Game1.chargeurCarte.LectureCarte(cheminsAcces[numCarteEnCours]);
                    deplacementHorizontalCarte = 0;
                    carte = new Carte(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne,
                      Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down) && numCarteEnCours > 0)
                {
                    numCarteEnCours--;
                    Game1.chargeurCarte.LectureCarte(cheminsAcces[numCarteEnCours]);
                    deplacementHorizontalCarte = 0;
                    carte = new Carte(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne,
                      Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Viewport ecranEntier = GraphicsDevice.Viewport;
            GraphicsDevice.Viewport = new Viewport(dimmensions);
            float echelle = (float)dimmensions.Height / ecranEntier.Height;
            Matrix redimmensionnemnet = Matrix.CreateTranslation(new Vector3(+deplacementHorizontalCarte, 0, 0)) * Matrix.CreateScale(echelle) ;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix() * redimmensionnemnet);
            carte.Draw(spriteBatch, new Vector2(ecranEntier.Width / 2 - deplacementHorizontalCarte, ecranEntier.Height / 2));
            spriteBatch.End();
            GraphicsDevice.Viewport = ecranEntier;
            if (deplacementHorizontalCarte + ecranEntier.Width / 2 >= carte.ObtenirTailleCarte().Width)
            {
                deplacementHorizontalCarte--;
            }
            else
            {
                deplacementHorizontalCarte = 0;
            }
            base.Draw(gameTime);
        }



    }
}
