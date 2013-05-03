using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Interface;

namespace BBTA.Classe.Elements
{

    public class Armement
    {
        private int nbGrenades;
        private int nbMines;
        private int nbRoquettes;

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

        public Armement(int nbGrenades = 8, int nbMines = 5, int nbRoquettes = 20)
        {
            this.nbGrenades = nbGrenades;
            this.nbMines = nbMines;
            this.nbRoquettes = nbRoquettes;
        }

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

        public int nbGrenadesRestantes()
        {
            return nbGrenades;
        }

        public int nbMinesRestantes()
        {
            return nbMines;
        }

        public int nbRoquettesRestantes()
        {
            return nbRoquettes;
        }
    }
}
