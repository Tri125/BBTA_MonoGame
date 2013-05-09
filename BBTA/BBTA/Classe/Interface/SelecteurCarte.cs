using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IndependentResolutionRendering;
using BBTA.Classe.Outils;
using BBTA.Carte;

namespace BBTA.Classe.Interface
{
    public class SelecteurCarte:DrawableGameComponent
    {
        private SpriteFont police;
        private Rectangle dimmensions;
        private CarteJeu carte;
        private List<string> nomCartes;
        private Texture2D blocs;
        private Texture2D arriereplan;
        private Texture2D parDessus;
        private SpriteBatch spriteBatch;
        private MouseState sourisAvant;
        private MouseState sourisMaintenant;
        private int numCarteEnCours = 0;
        private int deplacementHorizontalCarte = 0;
        private bool estChargee;


        public SelecteurCarte(Game jeu, Rectangle dimmensions)
            :base(jeu)
        {
            this.dimmensions = dimmensions;
            this.nomCartes = new List<string>();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            police = Game.Content.Load<SpriteFont>(@"PoliceIndicateur");
            blocs = Game.Content.Load<Texture2D>(@"Ressources\blocs");
            arriereplan = Game.Content.Load<Texture2D>(@"Ressources\HoraireNico");
            carte = new CarteJeu(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne, Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
            estChargee = true;
            parDessus = Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\Carte");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            sourisAvant = sourisMaintenant;
            sourisMaintenant = Mouse.GetState();
            nomCartes = Game1.chargeurCarte.NomCartes;

            if (sourisMaintenant.ScrollWheelValue < sourisAvant.ScrollWheelValue && numCarteEnCours < nomCartes.Count() - 1)
            {
                Game1.chargeurCarte.CarteSuivante();
                numCarteEnCours++;
                estChargee = false;
            }
            else if (sourisMaintenant.ScrollWheelValue > sourisAvant.ScrollWheelValue && numCarteEnCours > 0)
            {
                Game1.chargeurCarte.CartePrecedente();
                numCarteEnCours--;
                estChargee = false;
            }            

            if (sourisMaintenant.ScrollWheelValue == sourisAvant.ScrollWheelValue && estChargee == false)
            {
                deplacementHorizontalCarte = 0;
                carte = new CarteJeu(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne,
                    Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
                estChargee = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Viewport ecranEntier = GraphicsDevice.Viewport;
            float ratio = (float)ecranEntier.Width / IndependentResolutionRendering.Resolution.getVirtualViewport().Width;

            GraphicsDevice.Viewport = new Viewport(new Rectangle((int)(dimmensions.X * ratio + ecranEntier.X),(int)(dimmensions.Y * ratio + ecranEntier.Y),
                                                                 (int)(dimmensions.Width*ratio), (int)(dimmensions.Height*ratio)));
            float echelle = (float)dimmensions.Height / IndependentResolutionRendering.Resolution.getVirtualViewport().Height;
            Matrix redimmensionnemnet = Matrix.CreateTranslation(new Vector3(deplacementHorizontalCarte*ratio, 0, 0)) * Matrix.CreateScale(echelle) ;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix() * redimmensionnemnet);
            carte.Draw(spriteBatch, new Vector2(IndependentResolutionRendering.Resolution.getVirtualViewport().Width/ 2 - deplacementHorizontalCarte, 
                                                IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2));
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(parDessus, Vector2.Zero, Color.White);
            foreach (string nomCarte in nomCartes)
            {
                Vector2 position = new Vector2(dimmensions.X + dimmensions.Width / 2 - police.MeasureString(nomCarte).X / 2,
                            dimmensions.Height / 2 - police.MeasureString(nomCarte).Y / 2 + (nomCartes.IndexOf(nomCarte) - numCarteEnCours) * 50);
                spriteBatch.DrawString(police, nomCarte, position, nomCartes[numCarteEnCours] == nomCarte ? Color.Black : Color.White);
            }
            spriteBatch.End();

            GraphicsDevice.Viewport = ecranEntier;
            if (carte.ObtenirTailleCarte().Right + deplacementHorizontalCarte > IndependentResolutionRendering.Resolution.getVirtualViewport().Width/2)
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
