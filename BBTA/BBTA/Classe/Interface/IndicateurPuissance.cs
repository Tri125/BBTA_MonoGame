using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Interface
{
    public class IndicateurPuissance:MenuDeployable
    {
        //Constantes-----------------------------------------
        private const int HAUTEUR_PANNEAU = 221;
        private const int HAUTEUR_INDICATEUR = 22;
        private const int PALIER_ENERGIE = 1;
        private const int TEMPS_PALIER = 150;
        private const int FORCE_MAXIMALE = 20;
        private const int POSITION_INITIALE_INDICATEUR = -17;

        //Variables-----------------------------------------
        private Vector2 positionIndicateur;
        private int tempsEcouleDepuisDernierPalier;
        private bool tirEstCommence;
        private int forceDepart;

        //Événements-----------------------------------------
        public delegate void delegueForceFinaleDeterminee(int forceFinale);
        public event delegueForceFinaleDeterminee ForceFinaleDeterminee;

        /// <summary>
        /// Constructeur 
        /// </summary>
        /// <param name="texture">
        /// SpriteSheet de l'indicateur contentenant l'arrière-plan et l'indicateur mobile.
        /// Panneau: 221 px de hauteur, Indicateur: 22 px de hauteur, 1 px de séparation.
        /// </param>
        public IndicateurPuissance(Texture2D texture)
            : base(texture, new Rectangle(0, 0, texture.Width, HAUTEUR_PANNEAU), 200)
        {
            tirEstCommence = false;
            forceDepart = 0;
            tempsEcouleDepuisDernierPalier = 0;
            positionIndicateur = new Vector2(0, POSITION_INITIALE_INDICATEUR);
        }

        /// <summary>
        /// Met à jour le composant. 
        /// Gère l'animation lors du déploiement du menu
        /// Gère la pression de la touche "Espace" pour déclancher la procédure de sélection de force.
        /// Ferme automatiquement le composant lorsque la force est sélectionné.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); //Animation du déploiement gérée par la classe mère

            //Le processus de sélection de la puissane ne démarre que si le joueur appuie sur "Espace".  Il doit maintenir cette touche jusqu'à
            //temps qu'il ait obtenu la force désirée.  S'il la relâche, le composant se ferme.
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && tirEstCommence == false)
            {
                tirEstCommence = true;
            }

            /*La force à être déployée n'augmente que si le processus de sélection est commencé
             *et que l'utilisateur de la classe a ouvert le panneau.  Cette condition évite la situation où
             *l'utilisateur de la classe n'affiche pas le composant, mais le met à jour.
             */
            if (tirEstCommence == true && estOuvert == true)
            {
                tempsEcouleDepuisDernierPalier += gameTime.ElapsedGameTime.Milliseconds;
                //La force déployée est déterminée à l'aide de palier pré-déterminés
                if (estDeploye == true && tempsEcouleDepuisDernierPalier > TEMPS_PALIER && forceDepart < FORCE_MAXIMALE)
                {
                    tempsEcouleDepuisDernierPalier -= TEMPS_PALIER;
                    forceDepart += 1;
                    positionIndicateur = new Vector2(0, positionIndicateur.Y - 8);
                }

                /*Lorsque l'utilisateur cesse d'appuyer sur la touche "Espace", un événement est généré.
                 * Le composant est fermé automatiquement et les variables qui doivent être remises à leur valeur initiale le sont.*/
                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    if (ForceFinaleDeterminee != null)
                    {
                        ForceFinaleDeterminee(forceDepart);
                    }
                    tirEstCommence = false;
                    estOuvert = false;
                    forceDepart = 0;
                    tempsEcouleDepuisDernierPalier = 0;
                }
            }

            if (estDeploye == false)
            {
                //La remise à zéro ne s'effectue pas avec les autres variables pour de simples raisons esthétiques.
                positionIndicateur = new Vector2(0, POSITION_INITIALE_INDICATEUR); 
            }
        }

        /// <summary>
        /// Dessine à l'écran ledit menu
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); //Dessine l'arrière-plan
            //Dessine l'indicateur.
            spriteBatch.Draw(texturePanneau, Position+positionIndicateur, 
                             new Rectangle(0, HAUTEUR_PANNEAU+1, (int)texturePanneau.Width, HAUTEUR_INDICATEUR),
                             Color.White, 0, new Vector2(texturePanneau.Width/2, HAUTEUR_INDICATEUR), 
                             progressionDeploiement, SpriteEffects.None, 0);
        }

    }
}
