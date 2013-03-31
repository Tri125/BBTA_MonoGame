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
        private BBTA_Map carte;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer = new XmlSerializer(typeof(BBTA_Map));
        private bool chargementReussis;
        #endregion

        public bool ChargementReussis { get { return chargementReussis; } }


        public BBTA_MapFileBuilder()
        {
        }

        public void EcritureCarte(string FichierSortie, BBTA_Map carte)
        {
            try
            {
                ecriveur = new XmlTextWriter(FichierSortie, null);
                //ecriveur.Formatting = Formatting.Indented;
                //ecriveur.Indentation = 4;
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

        public void LectureCarte(string FichierEntre)
        {

            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                carte = (BBTA_Map)serializer.Deserialize(lecteur);

                chargementReussis = true;
            }


            catch (Exception ex)
            {
                carte = null;
                chargementReussis = false;
                Console.WriteLine(ex.Message);
                return;
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


        //public List<TuileEditeur> TuileEditeur()
        //{
        //    if (chargementReussis == true)
        //    {
        //        return carte.ListTuile;
        //    }
        //    return null;
        //}
    }
}
