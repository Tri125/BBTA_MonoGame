using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace EditeurCarteXNA
{
    //La classe BBTA_ConstructeurCarte est responsable de la lecture et de l'écriture des fichiers cartes dans le format XML,
    //elle utilise la sérialization pour ce faire. La classe teste les fichiers et détecte ceux invalides, elle permet de faire
    //une recherche de fichiers et de déterminer leur validité pour ensuite les chargés successivement.
    public class BBTA_ConstructeurCarte
    {
        #region Attribut
        //private readonly uint MEMOIRE_TAMPON;
        //Extension des fichiers qui seront recherchés.
        private readonly string EXTENSION;
        //Dossier recherchés pour les fichiers.
        private readonly string DOSSIER_CARTE;
        //Enregistre le path des fichiers
        private List<string> chemin;
        private List<string> nomCartes;
        //Contient la dernière carte chargé correctement. Null si aucun bon chargement.
        private BBTA_Carte carte;
        //private BBTA_Carte[] carteTampon;
        //Position actuel dans la liste de chemin.
        private int positionChemin = -1;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer;
        private bool chargementReussis;
        private bool initialiser;
        #endregion

        public bool ChargementReussis { get { return chargementReussis; } }
        public List<string> NomCartes { get { return nomCartes; } }

        /// <summary>
        /// Constructeur de BBTA_ConstructeurCarte.
        /// La classe contient les méthodes nécessaire à la lecture et l'écriture de fichiers BBTA_Carte
        /// </summary>
        /// <param name="memoireTampon">unsigned int de la taille voulue de la mémoire tampon.</param>
        /// <param name="extensionCarte">string extension des fichiers recherché</param>
        /// <param name="dossierCarte">string nom du dossier où la recherche des fichiers s'effectuera.</param>
        public BBTA_ConstructeurCarte(uint memoireTampon, string extensionCarte, string dossierCarte)
        {
            //this.MEMOIRE_TAMPON = memoireTampon;
            this.EXTENSION = extensionCarte;
            this.DOSSIER_CARTE = dossierCarte;
            //carteTampon = new BBTA_Carte[MEMOIRE_TAMPON];
            serializer = new XmlSerializer(typeof(BBTA_Carte));
        }

        /// <summary>
        /// Méthode pour initialiser la fonctionalité du chargement de fichiers successivement.
        /// </summary>
        public void LancementChargement()
        {
            initialiser = true;
            ChercheFichierCarte();
            TesteFichierCarte();
            //Pour charger la toute première carte
            CarteSuivante();
        }

        /// <summary>
        /// Cherche dans un dossier des fichiers avec une certaine extension et les enregistres.
        /// </summary>
        private void ChercheFichierCarte()
        {
            chemin = new List<string>();
            try
            {
                foreach (string fichier in Directory.GetFiles(DOSSIER_CARTE, EXTENSION))
                {
                    chemin.Add(fichier);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

        }


        //private void ChargementTampon()
        //{
        //    if (chemin != null && chemin.Count != 0)
        //    {
        //        for (int iBoucle = 0; iBoucle < MEMOIRE_TAMPON; iBoucle++)
        //        {
        //            if (chemin.Count > iBoucle)
        //            {
        //                if (LectureCarte(chemin[iBoucle]))
        //                {
        //                    carteTampon[iBoucle] = carte;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Pour le chargement successif, CarteSuivante() charge le prochain fichier contenue dans la liste des chemins.
        /// </summary>
        public void CarteSuivante()
        {
            if (!initialiser)
            {
                Console.WriteLine("Erreur BBTA_ConstructeurCarte::CarteSuivante: Non initialisé.");
                return;
            }
            if (((positionChemin + 1) < chemin.Count) && ((positionChemin + 1) > -1))
            {
                positionChemin++;
                //On charge la carte à la position. True si succès.
                if (LectureCarte(chemin[positionChemin]))
                {
                    return;
                }
                else
                {
                    //Si false alors la carte n'est pas valide.
                    //Supression du nom de la carte dans la liste et du chemin vers le fichier pour ne plus le considérer..
                    nomCartes.Remove(nomCartes[positionChemin]);
                    chemin.Remove(chemin[positionChemin]);
                    //On inverse la position du chemin.
                    positionChemin--;
                    //Récursive pour essayer de charger la prochaine carte dans la liste nouvellement modifié.
                    CarteSuivante();
                    return;
                }

            }
            else
            {
                //Lorsqu'on atteint la borne suppérieur.
                Console.WriteLine("BBTA_ConstructeurCarte:: CarteSuivante(): Maximum Atteint");
                return;
            }
        }


        /// <summary>
        /// Pour le chargement successif, CartePrecedente() charge la carte précédente dans la liste de chemin.
        /// </summary>
        public void CartePrecedente()
        {
            if (!initialiser)
            {
                Console.WriteLine("Erreur BBTA_ConstructeurCarte::CartePrecedente(): Non initialisé.");
                return;
            }
            if (((positionChemin - 1) < chemin.Count) && ((positionChemin - 1) >= 0))
            {
                positionChemin--;
                //Charge la carte. True si succès.
                if (LectureCarte(chemin[positionChemin]))
                {
                    return;
                }
                else
                {
                    //La carte a un problème. On supprime son nom et son chemin des listes pour ne plus la considérer.
                    nomCartes.Remove(nomCartes[positionChemin]);
                    chemin.Remove(chemin[positionChemin]);
                    //Opération inverse sur la position du chemin.
                    positionChemin++;
                    //Récursive pour essayer de charger la carte précédente de notre liste modifié.
                    CartePrecedente();
                    return;
                }

            }
            else
            {
                //Lorsqu'on atteint la borne inférieur.
                Console.WriteLine("BBTA_ConstructeurCarte:: CartePrecedente: Minimum Atteint");
                return;
            }
        }

        /// <summary>
        /// Teste la validité de chaque fichier carte et récupère leurs noms.
        /// </summary>
        private void TesteFichierCarte()
        {
            nomCartes = new List<string>();
            foreach (string cheminCarte in chemin.ToList())
            {
                //On essais de charger chaque fichier carte à partir du chemin vers le fichieré
                //On enlève le chemin vers le fichier si elle n'est pas valide.
                //On récupère le nom de la carte si elle est valide.
                if (!LectureCarte(cheminCarte))
                {
                    chemin.Remove(cheminCarte);
                }
                else
                {
                    nomCartes.Add(carte.InformationCarte.NomCarte);
                }
            }
        }

        /// <summary>
        /// Écrit un fichier XML avec le nom donné et un objet BBTA_Carte.
        /// </summary>
        /// <param name="FichierSortie">string nom du fichier qui sera enregistré</param>
        /// <param name="carte">BBTA_Carte objet qui sera enregistré</param>
        public void EcritureCarte(string FichierSortie, BBTA_Carte carte)
        {
            try
            {
                ecriveur = new XmlTextWriter(FichierSortie, null);
                ecriveur.WriteStartDocument();

                serializer.Serialize(ecriveur, carte);


                ecriveur.WriteEndDocument();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            finally
            {
                if (ecriveur != null)
                {
                    ecriveur.Close();
                }
            }

        }

        /// <summary>
        /// Lis un fichier XML BBTA_Carte et le charge.
        /// Retourne false si la lecture est incorrecte.
        /// </summary>
        /// <param name="FichierEntre">string nom du fichier qui sera lu</param>
        private bool LectureCarte(string FichierEntre)
        {

            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                carte = (BBTA_Carte)serializer.Deserialize(lecteur);
                chargementReussis = true;
                return true;
            }


            catch (Exception ex)
            {
                carte = null;
                chargementReussis = false;
                Console.WriteLine(ex.Message);
                return false;
            }

            finally
            {
                if (lecteur != null)
                {
                    lecteur.Close();
                }
            }



        }

        /// <summary>
        /// Si une carte est chargé, la méthode retourne l'objet InfoCarte contenue.
        /// </summary>
        public EditeurCarteXNA.BBTA_Carte.InfoCarte InformationCarte()
        {
            if (chargementReussis == true)
            {
                return carte.InformationCarte;
            }
            return null;
        }

        /// <summary>
        /// Si une carte est chargé, retourne une liste des identifiants de toutes les tuiles de la carte chargé.
        /// </summary>
        public List<int> InfoTuileListe()
        {
            if (chargementReussis == true)
            {
                List<int> identifiantTuile = new List<int>();
                foreach (TuileEditeur tuile in carte.ListTuile)
                {
                    identifiantTuile.Add(tuile.IdTuile);
                }
                return identifiantTuile;
            }
            return null;
        }


        /// <summary>
        /// Si une carte est chargé, retourne une tableau des identifiants de toutes les tuiles de la carte chargé.
        /// </summary>
        public int[] InfoTuileTab()
        {
            if (chargementReussis == true)
            {
                List<int> identifiantTuile = new List<int>();
                foreach (TuileEditeur tuile in carte.ListTuile)
                {
                    identifiantTuile.Add(tuile.IdTuile);
                }
                return identifiantTuile.ToArray();
            }
            return null;
        }
    }
}
