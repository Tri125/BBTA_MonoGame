using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace XNATileMapEditor
{
    public class BBTA_MapFileBuilder
    {
        #region Attribut
        private readonly uint MEMOIRE_TAMPON;
        private readonly string EXTENSION;
        private readonly string DOSSIER_CARTE;
        private List<string> chemin;
        private BBTA_Map carte;
        private BBTA_Map[] carteTampon;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer;
        private bool chargementReussis;
        #endregion

        public bool ChargementReussis { get { return chargementReussis; } }


        public BBTA_MapFileBuilder(uint memoireTampon, string extensionCarte, string dossierCarte)
        {
            this.MEMOIRE_TAMPON = memoireTampon;
            this.EXTENSION = extensionCarte;
            this.DOSSIER_CARTE = dossierCarte;
            carteTampon = new BBTA_Map[MEMOIRE_TAMPON];
            serializer = new XmlSerializer(typeof(BBTA_Map));
        }


        public void ChercheFichierCarte()
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


        //public BBTA_Map ChargementTampon()
        //{
        //    if (chemin != null && chemin.Count != 0)
        //    {

        //    }
        //}



        public void TesteFichierCarte()
        {
            foreach (string cheminCarte in chemin.ToList())
            {
                if (!LectureCarte(cheminCarte))
                {
                    chemin.Remove(cheminCarte);
                }
            }
        }

        public void EcritureCarte(string FichierSortie, BBTA_Map carte)
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

        public bool LectureCarte(string FichierEntre)
        {

            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                carte = (BBTA_Map)serializer.Deserialize(lecteur);
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

        public XNATileMapEditor.BBTA_Map.InfoCarte InformationCarte()
        {
            if (chargementReussis == true)
            {
                return carte.InformationCarte;
            }
            return null;
        }

        public List<int> InfoTuileListe()
        {
            if (chargementReussis == true)
            {
                List<int> identifiantTuile = new List<int>();
                foreach (TuileEditeur tuile in carte.ListTuile)
                {
                    identifiantTuile.Add(tuile.TileId);
                }
                return identifiantTuile;
            }
            return null;
        }


        public int[] InfoTuileTab()
        {
            if (chargementReussis == true)
            {
                List<int> identifiantTuile = new List<int>();
                foreach (TuileEditeur tuile in carte.ListTuile)
                {
                    identifiantTuile.Add(tuile.TileId);
                }
                return identifiantTuile.ToArray();
            }
            return null;
        }
    }
}
