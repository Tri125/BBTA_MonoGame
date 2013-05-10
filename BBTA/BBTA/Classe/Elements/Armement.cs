using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.Elements
{
    public enum Armes
    {
        Roquette = 0,
        Grenade,
        Mine
    }

    /// <summary>
    /// Armement détenu par une équipe.  Retient en mémoire le nombre de munitions restantes pour chaque arme.
    /// </summary>
    public class Armement
    {
        //Nombre de munitions dans chaque arme
        private int[] armes = new int[Enum.GetNames(typeof(Armes)).Length];

        //Indexer pour pouvoir avoir accès facilement au nombre de munitions de l'arme désirée
        public int this[Armes arme]
        {
            get
            {
                return armes[(int)arme];           
            }
            set
            {
                armes[(int)arme] = value;
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
            this.armes[(int)Armes.Grenade] = nbGrenades;
            this.armes[(int)Armes.Mine] = nbMines;
            this.armes[(int)Armes.Roquette] = nbRoquettes;
        }

        /// <summary>
        /// Permet d'utiliser une grenade et donc, réduit le nombre de grenades d'un.
        /// </summary>
        /// <returns>(Oui) -> Il y a suffisemment de grenades pour tirer. (Non) -> le stock de grenades est épuisé.</returns>
        public bool GrenadeUtilisee()
        {
            if (armes[(int)Armes.Grenade] == 0)
            {
                return false;
            }
            else
            {
                armes[(int)Armes.Grenade]--;
                return true;
            }
        }

        /// <summary>
        /// Permet d'utiliser une mine et donc, réduit le nombre de mines d'un.
        /// </summary>
        /// <returns>(Oui) -> Il y a suffisemment de mines pour tirer. (Non) -> le stock de mines est épuisé.</returns>
        public bool MineUtilisee()
        {
            if (armes[(int)Armes.Mine] == 0)
            {
                return false;
            }
            else
            {
                armes[(int)Armes.Mine]--;
                return true;
            }
        }

        /// <summary>
        /// Permet d'utiliser une roquette et donc, réduit le nombre de roquettes d'un.
        /// </summary>
        /// <returns>(Oui) -> Il y a suffisemment de roquettes pour tirer. (Non) -> le stock de roquettes est épuisé.</returns>
        public bool RoquetteUtilisee()
        {
            if (armes[(int)Armes.Roquette] == 0)
            {
                return false;
            }
            else
            {
                armes[(int)Armes.Roquette]--;
                return true;
            }
        }

        /// <summary>
        /// Retourne le nombre de grenades restantes
        /// </summary>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbGrenadesRestantes()
        {
            return armes[(int)Armes.Grenade];
        }

        /// <summary>
        /// Retourne le nombre de mines restantes
        /// </summary>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbMinesRestantes()
        {
            return armes[(int)Armes.Mine];
        }

        /// <summary>
        /// Retourne le nombre de roquettes restantes
        /// </summary>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbRoquettesRestantes()
        {
            return armes[(int)Armes.Roquette];
        }
    }
}
