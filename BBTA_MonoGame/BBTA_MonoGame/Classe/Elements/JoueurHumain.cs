using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BBTA.Option;

namespace BBTA.Elements
{
    /// <summary>
    /// Un joueur humain est un acteur contrôlé par un joueur humain.  En somme, c'est le personnage que contrôleront les joueurs de BBTA.
    /// Le personnage peut se déplacer de gauche à droite à l'aide du clavier.
    /// Il peut sauter.
    /// Il peut faire valoir, par le biais d'un événement, qu'il désire tirer.
    /// </summary>
    public class JoueurHumain : Acteur
    {
        //Touches personnalisées de l'utilisateur------------------------------------------------------------------------------
        private Option.Option.ParametreTouche optionClavier = Game1.chargeurOption.OptionActive.InformationTouche;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel le joueur évoluera.</param>
        /// <param name="texture">Texture du personnage affichée à l'écran.</param>
        /// <param name="position">Position initiale du joueur lors de la partie.</param>
        /// <param name="nbColonnes">Nombre d'image de la spritesheet sur l'axe horizontal</param>
        /// <param name="nbRangees">Nombre d'image de la spritesheet sur l'axe vertical</param>
        /// <param name="milliSecParImage">Délai entre l'affichage de chaque image</param>
        public JoueurHumain(World mondePhysique, Texture2D texture, Vector2 position, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(mondePhysique, texture, position, nbColonnes, nbRangees, milliSecParImage)
        {
        }

        /// <summary>
        /// Met à jour le joueur.
        /// Tient compte des touches appuyée pour ordonner les actions du joueur.
        /// Tient compte de l'état du joueur (si c'est son tour ou s'il est en train de choisir une arme) pour décider s'il doit faire 
        /// bouger le joueur.  
        /// Anime le joueur en modifiant l'image de la spritesheet affichée
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState clavier = Keyboard.GetState(); //État actuel du clavier

            /*S'il effectue un clic alors qu'il n'est pas déjà en mode tir, qu'il est au sol et que c'est son tour, alors un déclanche un événement pour 
             *signifier notre désir de tirer.
             */
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && enModeTir == false  && monTour == true && estAuSol == true)
            {
                Tirer();
            }

            //Animation du joueur et immobilisation du joueur en vertu des décisions de déplacement prises lors du dernier "Update"
            base.Update(gameTime);

            //S'il n'est pas en train de tirer et que c'est son tour, il peut se déplacer.
            if (!enModeTir && monTour == true)
            {
                if (clavier.IsKeyDown(optionClavier.Droite))
                {
                    BougerADroite();
                }

                if (clavier.IsKeyDown(optionClavier.Gauche))
                {
                    BougerAGauche();
                }

                if (clavier.IsKeyDown(optionClavier.Saut) && estAuSol == true)
                {
                    Sauter();
                }
            }
        }
    }
}