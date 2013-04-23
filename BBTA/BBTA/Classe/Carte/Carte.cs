using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Elements;
using IndependentResolutionRendering;
using BBTA.Interfaces;

namespace BBTA
{

    public enum TypeBloc
    {
        Apparition = -1,
        Terre = 0,
        GazonHaut = 1,
        GazonCoinGauche = 2,
        GazonCoinDroite = 3,
        GazonCoinGaucheDroite = 4
    }
    /// <summary>
    /// La classe contient les blocs composant le relief ainsi que l'arrière-plan de la carte.
    /// -----------------------------------------------------------------------------------------------
    /// Affiche les blocs nécessaire.
    /// Affiche l'arrière-plan.
    /// Gère la destruction des blocs s'il y a lieu.
    /// -----------------------------------------------------------------------------------------------
    /// </summary>
    public class Carte
    {
        //Variables-----------------------------------------------------------------------------------------------
        private Texture2D textureArrierePlan;
        private Bloc[] blocs;
        private int largeur;
        private int hauteur;
        private List<Vector2> listeApparition;
        //Constantes----------------------------------------------------------------------------------------------
        private const float TAILLE_BLOC = 1f;

        public List<Vector2> ListeApparition { get { return listeApparition.ToList(); } }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="donneesBlocs">Données sur la nature des blocs</param>
        /// <param name="largeurCarte">Largeur de la carte (en blocs)</param>
        /// <param name="arrierePlan">Arrière-plan de la carte</param>
        /// <param name="textureBlocs">Texture des blocs</param>
        /// <param name="mondePhysique">World Farseer</param>
        /// <param name="MetrePixel">Valeur en pixel d'un metre</param>
        public Carte(int[] donneesBlocs, int largeurCarte, Texture2D arrierePlan, Texture2D textureBlocs, World mondePhysique, float metrePixel)
        {
            this.listeApparition = new List<Vector2>();
            this.textureArrierePlan = arrierePlan;
            this.largeur = largeurCarte;
            this.hauteur = donneesBlocs.Length / largeur * 40;
            blocs = new Bloc[donneesBlocs.Length];
            for (int compteurBlocs = 0; compteurBlocs < donneesBlocs.Length; compteurBlocs++)
            {
                //Par convention, une case avec "1" comme donnée signifie une case de terre pour notre énumérateur.
                if (donneesBlocs[compteurBlocs] == 1)
                {
                    //Position en mètres
                    Vector2 positionBloc = new Vector2((compteurBlocs % largeurCarte * TAILLE_BLOC) + (TAILLE_BLOC * 0.5f) + 5, (compteurBlocs / largeurCarte * TAILLE_BLOC) + (TAILLE_BLOC * 0.5f));
                    blocs[compteurBlocs] = new Bloc(mondePhysique, positionBloc, textureBlocs, TAILLE_BLOC, metrePixel, TypeDeBlocAGenerer(donneesBlocs, largeur, compteurBlocs));
                }
                else
                    //Par convention, une case avec "-1" comme donnée signifie un lieu d'apparition pour les joueurs.
                    if (donneesBlocs[compteurBlocs] == (int)TypeBloc.Apparition)
                    {
                        listeApparition.Add(new Vector2(metrePixel * ((compteurBlocs % largeurCarte * TAILLE_BLOC) + (TAILLE_BLOC * 0.5f) + 5), metrePixel * ((compteurBlocs / largeurCarte * TAILLE_BLOC) + (TAILLE_BLOC * 0.5f))));
                    }
            }
        }

        /// <summary>
        /// Détruit les blocs nécessaires suite à une explosion
        /// </summary>Lieu d'origine de l'explosion</param>
        /// <param name="energie">Énergie déployée par l'explosion</param>
        public void Explosion(Vector2 lieu, float energie)
        {
            for (int compteurBloc = 0; compteurBloc < blocs.Length; compteurBloc++)
            {
                if (blocs[compteurBloc] != null && blocs[compteurBloc].ExplosetIl(energie, lieu))
                {
                    //Destruction du bloc   
                    blocs[compteurBloc] = null;
                }
            }
        }

        /// <summary>
        /// Affiche les blocs nécessaires à l'écran
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 positionCamera)
        {
            spriteBatch.Draw(textureArrierePlan, new Vector2(positionCamera.X - IndependentResolutionRendering.Resolution.getVirtualViewport().Width / 2f,
                                                           positionCamera.Y - IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2f), Color.White);
            foreach (Bloc item in blocs)
            {
                if (item != null)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        private TypeBloc TypeDeBlocAGenerer(int[] blocs, int largeur, int identifiant)
        {
            if (identifiant < largeur)
            {
                return TypeBloc.Terre;
            }
            else
            {
                if (blocs[identifiant - largeur] <= 0)
                {
                    if (identifiant % largeur == 0)
                    {
                        return TypeBloc.GazonCoinGauche;
                    }
                    else if (identifiant % largeur == largeur - 1)
                    {
                        return TypeBloc.GazonCoinDroite;
                    }

                    else if (blocs[identifiant - 1] <= 0 && blocs[identifiant + 1] <= 0)
                    {
                        return TypeBloc.GazonCoinGaucheDroite;
                    }
                    else if (blocs[identifiant - 1] > 0 && blocs[identifiant + 1] <= 0)
                    {
                        return TypeBloc.GazonCoinDroite;
                    }
                    else if (blocs[identifiant - 1] <= 0 && blocs[identifiant + 1] > 0)
                    {
                        return TypeBloc.GazonCoinGauche;
                    }
                    else
                    {
                        return TypeBloc.GazonHaut;
                    }
                }

                else
                {
                    return TypeBloc.Terre;
                }
            }

        }

        public Rectangle ObtenirTailleCarte()
        {
            return new Rectangle(0, 0, largeur, hauteur);
        }
    }
}