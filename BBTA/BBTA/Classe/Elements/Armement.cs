using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Interface;

namespace BBTA.Classe.Elements
{
    /// <summary>
    /// Armement détenu par une équipe.  Retient en mémoire le nombre de munitions restantes pour chaque arme.
    /// </summary>
    public class Armement
    {
        //Nombre de munitions dans chaque arme
        private int nbGrenades;
        private int nbMines;
        private int nbRoquettes;

        //Indexer pour pouvoir avoir accès facilement au nombre de munitions de l'arme désirée
        public int this[Armes arme]
        {
            get
            {
                switch (arme)
                {
                    case Armes.Grenade:
                        return nbGrenades;
                    case Armes.Mine:
                        return nbMines;
                    case Armes.Roquette:
                        return nbRoquettes;
                    default:
                        return 0;
                }                
            }
            set
            {
                switch (arme)
                {
                    case Armes.Grenade:
                        nbGrenades = value;
                        break;
                    case Armes.Mine:
                        nbMines = value;
                        break;
                    case Armes.Roquette:
                        nbRoquettes = value;
                        break;
                }  
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="nbGrenades">Nombre de grenades au départ</param>
        /// <param name="nbMines">Nombre de mines au départ</param>
        /// <param name="nbRoquettes">Nombre de roquettes au départ</param>
        public Armement(int nbGrenades = 8, int nbMines = 5, int nbRoquettes = 20)
        {
            this.nbGrenades = nbGrenades;
            this.nbMines = nbMines;
            this.nbRoquettes = nbRoquettes;
        }

        /// <summary>
        /// Permet d'utiliser une grenade et donc, réduit le nombre de grenades d'un.
        /// </summary>
        /// <returns>(Oui) -> Il y a suffisemment de grenades pour tirer. (Non) -> le stock de grenades est épuisé.</returns>
        public bool GrenadeUtilisee()
        {
            if (nbGrenades == 0)
            {
                return false;
            }
            else
            {
                nbGrenades--;
                return true;
            }
        }

        /// <summary>
        /// Permet d'utiliser une mine et donc, réduit le nombre de mines d'un.
        /// </summary>
        /// <returns>(Oui) -> Il y a suffisemment de mines pour tirer. (Non) -> le stock de mines est épuisé.</returns>
        public bool MineUtilisee()
        {
            if (nbMines == 0)
            {
                return false;
            }
            else
            {
                nbMines--;
                return true;
            }
        }

        /// <summary>
        /// Permet d'utiliser une roquette et donc, réduit le nombre de roquettes d'un.
        /// </summary>
        /// <returns>(Oui) -> Il y a suffisemment de roquettes pour tirer. (Non) -> le stock de roquettes est épuisé.</returns>
        public bool RoquetteUtilisee()
        {
            if (nbRoquettes == 0)
            {
                return false;
            }
            else
            {
                nbRoquettes--;
                return true;
            }
        }

        /// <summary>
        /// Retourne le nombre de grenades restantes
        /// </summary>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbGrenadesRestantes()
        {
            return nbGrenades;
        }

        /// <summary>
        /// Retourne le nombre de mines restantes
        /// </summary>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbMinesRestantes()
        {
            return nbMines;
        }

        /// <summary>
        /// Retourne le nombre de roquettes restantes
        /// </summary>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbRoquettesRestantes()
        {
            return nbRoquettes;
        }
    }
}
