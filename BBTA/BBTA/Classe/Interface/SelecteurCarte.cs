using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using IndependentResolutionRendering;
using BBTA.Outils;
using BBTA.Carte;

namespace BBTA.Interface
{
    /// <summary>
    /// SecteurCarte est le composant qui permet au joueur de prévisualiser la carte qu'il s'apprète à choisir.
    /// Il affiche la carte choisie dans une zone précise désignée.
    /// Il affiche les noms des cartes.
    /// Il gère la sélection de carte à l'aide de la mollette.
    /// Le chargement de carte n'est pas gérer par lui, mais bien par le chargeur de carte dans la classe principale du jeu.
    /// </summary>
    public class SelecteurCarte:DrawableGameComponent
    {
        //Ressources utilisées pour l'affichage------------------------------------------------------------------------------------
        private SpriteBatch spriteBatch;
        private SpriteFont police;
        private Texture2D blocs;
        private Texture2D arriereplan;
        private Texture2D parDessus;

        //Interaction avec l'tulisateur--------------------------------------------------------------------------------------------
        private MouseState sourisAvant;
        private MouseState sourisMaintenant;

        //Variables reliées à l'affichage de la carte------------------------------------------------------------------------------
        private Rectangle dimensions;
        private CarteJeu carte;
        private int numCarteEnCours = 0;
        private int deplacementHorizontalCarte = 0;

        //Variables reliées au chargement des cartes-------------------------------------------------------------------------------
        private bool estChargee;
        private List<string> nomCartes;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="jeu">Classe principale du jeu</param>
        /// <param name="dimmensions">Dim</param>
        public SelecteurCarte(Game jeu, Rectangle dimmensions)
            :base(jeu)
        {
            this.dimensions = dimmensions;
            this.nomCartes = new List<string>();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            police = Game.Content.Load<SpriteFont>(@"Police\PoliceIndicateur");
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

            GraphicsDevice.Viewport = new Viewport(new Rectangle((int)(dimensions.X * ratio + ecranEntier.X),(int)(dimensions.Y * ratio + ecranEntier.Y),
                                                                 (int)(dimensions.Width*ratio), (int)(dimensions.Height*ratio)));
            float echelle = (float)dimensions.Height / IndependentResolutionRendering.Resolution.getVirtualViewport().Height;
            Matrix redimmensionnemnet = Matrix.CreateTranslation(new Vector3(deplacementHorizontalCarte*ratio, 0, 0)) * Matrix.CreateScale(echelle) ;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix() * redimmensionnemnet);
            carte.Draw(spriteBatch, new Vector2(IndependentResolutionRendering.Resolution.getVirtualViewport().Width/ 2 - deplacementHorizontalCarte, 
                                                IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2));
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(parDessus, Vector2.Zero, Color.White);
            foreach (string nomCarte in nomCartes)
            {
                Vector2 position = new Vector2(dimensions.X + dimensions.Width / 2 - police.MeasureString(nomCarte).X / 2,
                            dimensions.Height / 2 - police.MeasureString(nomCarte).Y / 2 + (nomCartes.IndexOf(nomCarte) - numCarteEnCours) * 50);
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
