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
        public bool MunitionUtilisee(Armes typeArme)
        {
            if (armes[(int)typeArme] == 0)
            {
                return false;
            }
            else
            {
                armes[(int)typeArme]--;
                return true;
            }
        }

        /// <summary>
        /// Retourne le nombre de munitions restantes de l'arme passée en paramètre.
        /// </summary>
        /// <param name="typeArme">Type de l'arme dont on veut savoir les munitions</param>
        /// <returns>Nombre de munitions restantes</returns>
        public int nbMunitionsRestante(Armes typeArme)
        {
            return armes[(int)typeArme];
        }
    }
}
