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
        private Carte carte;
        private Texture2D blocs;
        private Texture2D arriereplan;
        private Texture2D parDessus;
        private SpriteBatch spriteBatch;
        private string[] cheminsAcces;
        private string[] cheminsAffiches;
        private MouseState sourisAvant;
        private MouseState sourisMaintenant;
        private int numCarteEnCours = 0;
        private int deplacementHorizontalCarte = 0;


        public SelecteurCarte(Game jeu, Rectangle dimmensions, string[] chemins)
            :base(jeu)
        {
            this.dimmensions = dimmensions;
            this.cheminsAcces = chemins;
            cheminsAffiches = new string[cheminsAcces.Length];
            int derniereBarreOblique = 0;
            for (int nbChemins = 0; nbChemins < cheminsAffiches.Length; nbChemins++)
            {
                for (int nbCaracteres = 0; nbCaracteres < cheminsAcces[nbChemins].Length; nbCaracteres++)
                {
                    if (cheminsAcces[nbChemins][nbCaracteres].Equals('\\'))
                    {
                        derniereBarreOblique = nbCaracteres;
                    }
                }
                cheminsAffiches[nbChemins] = cheminsAcces[nbChemins].Substring(derniereBarreOblique+1, cheminsAcces[nbChemins].Length-5-derniereBarreOblique);
            }
            Game1.chargeurCarte.LectureCarte(cheminsAcces[numCarteEnCours]);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            police = Game.Content.Load<SpriteFont>(@"PoliceIndicateur");
            blocs = Game.Content.Load<Texture2D>(@"Ressources\blocs");
            arriereplan = Game.Content.Load<Texture2D>(@"Ressources\HoraireNico");
            carte = new Carte(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne, Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
            parDessus = Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\Carte");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            sourisAvant = sourisMaintenant;
            sourisMaintenant = Mouse.GetState();
            if(sourisMaintenant.ScrollWheelValue < sourisAvant.ScrollWheelValue && numCarteEnCours < cheminsAcces.Length-1)
            {
                numCarteEnCours++;
                Game1.chargeurCarte.LectureCarte(cheminsAcces[numCarteEnCours]);
                deplacementHorizontalCarte = 0;
                carte = new Carte(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne,
                    Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
            }
            else if (sourisMaintenant.ScrollWheelValue > sourisAvant.ScrollWheelValue && numCarteEnCours > 0)
            {
                numCarteEnCours--;
                Game1.chargeurCarte.LectureCarte(cheminsAcces[numCarteEnCours]);
                deplacementHorizontalCarte = 0;
                carte = new Carte(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne,
                    Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
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
            foreach (string item in cheminsAffiches)
            {
                int nbCompteur = 0;
                while(cheminsAffiches[nbCompteur] != item)
                {
                    nbCompteur++;
                }
                Vector2 position = new Vector2(dimmensions.X + dimmensions.Width / 2 - police.MeasureString(item).X / 2,
                            dimmensions.Height / 2 - police.MeasureString(item).Y / 2 + (nbCompteur - numCarteEnCours) * 50);
                spriteBatch.DrawString(police, item, position, cheminsAffiches[numCarteEnCours] == item ? Color.Black : Color.White);
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
