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
    /// <summary>
    /// SecteurCarte est le composant qui permet au joueur de prévisualiser la carte qu'il s'apprète à choisir.
    /// Il affiche la carte choisie dans une zone précise désignée.
    /// Il affiche les noms des cartes.
    /// Il gère la sélection de carte à l'aide de la mollette.
    /// La carte est générée en temps réel (ce n'est pas une simple image)
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
        private bool estChargee; //Indique si l'on doit chargé une carte. 
        private List<string> nomCartes;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="jeu">Classe principale du jeu</param>
        /// <param name="dimensions">Espace occupé par le composant à l'écran. (la taille de la carte sera maximizée sur la hauteur)</param>
        public SelecteurCarte(Game jeu, Rectangle dimensions)
            :base(jeu)
        {
            this.dimensions = dimensions;
            this.nomCartes = new List<string>();
        }

        /// <summary>
        /// Chargement du contenu nécessaire à l'affichage du composant.
        /// Création de la carte.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Cahrgement des ressources
            police = Game.Content.Load<SpriteFont>(@"PoliceIndicateur");
            blocs = Game.Content.Load<Texture2D>(@"Ressources\blocs");
            arriereplan = Game.Content.Load<Texture2D>(@"Ressources\HoraireNico");
            parDessus = Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\Carte");
            //Création de la carte.
            carte = new CarteJeu(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne, Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
            estChargee = true;
            base.LoadContent();
        }

        /// <summary>
        /// Charge les cartes à afficher.
        /// Permet de naviguer entre les différentes cartes à l'aide de la molette.
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            //L'état de la souris est conversé en mémoire pour vérifier l'immobilisation de la roulette.
            sourisAvant = sourisMaintenant;
            sourisMaintenant = Mouse.GetState();            

            nomCartes = Game1.chargeurCarte.NomCartes;

            //Selon le sens de roulement de la mollette, on charge les cartes appropirées.            
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

            //Lorsque la mollette est immbilisée, on crée la carte sélectionnée.
            if (sourisMaintenant.ScrollWheelValue == sourisAvant.ScrollWheelValue && estChargee == false)
            {
                deplacementHorizontalCarte = 0;
                carte = new CarteJeu(Game1.chargeurCarte.InfoTuileTab(), Game1.chargeurCarte.InformationCarte().NbColonne,
                    Game1.chargeurCarte.InformationCarte().NbRange, arriereplan, blocs, new FarseerPhysics.Dynamics.World(Vector2.Zero), 40);
                estChargee = true;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Affiche le composant à l'écran
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Draw(GameTime gameTime)
        {
            /* Explication de la procédure
             * ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
             * Pour dessiner une partie de la carte seulement, on modifie la taille et la position du Viewport en fonction de la valeur fournie dans la variable "dimensions".  Par 
             * la suite, on réduit la taille de la carte à l'aide de la matrice "redimmensionnement" pour que la hauteur totale de la carte corresponde à la hauteur du nouveau 
             * viewport.  Pour le reste, il s'agit de dessiner les éléments en tenant compte de l'indépendance de résolution et on remet à la fin le viewport d'origine 
             * ---------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            //Redimmensionnement de la carte selon les dimensions spécifiées.
            Viewport ecranEntier = GraphicsDevice.Viewport;

            //Ratio de réduction de taille en fonction des dimensions spécifiées
            float ratio = (float)ecranEntier.Width / IndependentResolutionRendering.Resolution.getVirtualViewport().Width;
            GraphicsDevice.Viewport = new Viewport(new Rectangle((int)(dimensions.X * ratio + ecranEntier.X),(int)(dimensions.Y * ratio + ecranEntier.Y),
                                                                 (int)(dimensions.Width*ratio), (int)(dimensions.Height*ratio)));

            //Ratio de réduction en focntion de l'indépendance de résolution (taille de l'écran)
            float echelle = (float)dimensions.Height / IndependentResolutionRendering.Resolution.getVirtualViewport().Height;
            Matrix redimmensionnemnet = Matrix.CreateTranslation(new Vector3(deplacementHorizontalCarte*ratio, 0, 0)) * Matrix.CreateScale(echelle) ;

            //Affichage de la carte
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Resolution.getTransformationMatrix() * redimmensionnemnet);
            carte.Draw(spriteBatch, new Vector2(IndependentResolutionRendering.Resolution.getVirtualViewport().Width/ 2 - deplacementHorizontalCarte, 
                                                IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2));
            spriteBatch.End();

            //Affichage des noms de cartes et des textures d'interface par dessus.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(parDessus, Vector2.Zero, Color.White);
            foreach (string nomCarte in nomCartes)
            {
                Vector2 position = new Vector2(dimensions.X + dimensions.Width / 2 - police.MeasureString(nomCarte).X / 2,
                            dimensions.Height / 2 - police.MeasureString(nomCarte).Y / 2 + (nomCartes.IndexOf(nomCarte) - numCarteEnCours) * 50);
                spriteBatch.DrawString(police, nomCarte, position, nomCartes[numCarteEnCours] == nomCarte ? Color.Black : Color.White);
            }
            spriteBatch.End();

            //Le viewport est remis à sa valeur d'origine
            GraphicsDevice.Viewport = ecranEntier;

            //Défilement horizontal de la carte pour la voir entièrement (s'il elle est trop longue pour le viewport
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
