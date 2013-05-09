using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EditeurCarteXNA
{
    /// <summary>
    ///La classe TuileEditeur est utilisé pour l'éditeur de niveau dans le but de créer un fichier carte.
    ///Chaque tuile a un  int qui identifie le type de la tuile et un Rectangle pour cliquer dessus et l'afficher dans l'éditeur.
    /// </summary>
    public class TuileEditeur
    {
        private Rectangle bloc;
        private int idTuile;

        public int IdTuile
        {
            get
            {
                return idTuile;
            }
            set
            {
                idTuile = value;
            }
        }

        public Rectangle Bloc
        {
            get
            {
                return bloc;
            }
        }

        /// <summary>
        /// Constructeur de base de TuileEditeur.
        /// La classe contient un identifiant et un Rectangle. Utilisé dans l'editeur de niveau.
        /// </summary>
        public TuileEditeur()
        {
            idTuile = 0;
        }

        /// <summary>
        /// Constructeur de TuileEditeur.
        /// La classe contient un identifiant et un Rectangle. Utilisé dans l'editeur de niveau.
        /// </summary>
        /// <param name="bloc">
        /// Rectangle bloc définie la zone occupé par la tuile dans l'éditeur de niveau.
        /// </param>
        /// <param name="idTuile">
        /// int idTuile est l'identifiant numérique représentant le type de la tuile.
        /// </param>
        public TuileEditeur(Rectangle bloc, int tileID = 0)
        {
            this.bloc = bloc;
            this.idTuile = tileID;
        }
    }
}
