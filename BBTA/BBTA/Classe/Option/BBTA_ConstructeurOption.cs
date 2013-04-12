using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;


using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Classe.Option
{
    public class BBTA_ConstructeurOption
    {
        #region Attribut
        private Option optionUtilisateur;
        private Option optionDefaut;
        bool mauvaisUtilisateur;
        bool mauvaisDefaut;
        bool presentUtilisateur;
        bool presentDefaut;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer = new XmlSerializer(typeof(Option));
        private bool chargementReussis;
        #endregion

        #region Option Usine
        private Option optionBase = new Option();
        private string nomDefaut = "defautConfig.xml";
        private string nomUtilisateur = "utilisateurConfig.xml";
        #endregion

        public bool ChargementReussis { get { return chargementReussis; } }


        private void OptionBase()
        {
            this.optionBase.InformationSonore.musique = 100;
            this.optionBase.InformationSonore.effetSonore = 100;

            this.optionBase.InformationTouche.Gauche = Keys.A;
            this.optionBase.InformationTouche.Droite = Keys.D;
            this.optionBase.InformationTouche.Saut = Keys.Space;
            this.optionBase.InformationTouche.Tir = Keys.Space;
            this.optionBase.InformationTouche.Pause = Keys.P;
        }

        private void ChercheFichierConfig()
        {
            List<string> fichiers = new List<string>();

            foreach (var path in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                fichiers.Add(System.IO.Path.GetFileName(path));
            }

            foreach (string fichier in fichiers)
            {
                if (fichier == nomDefaut)
                {
                    presentDefaut = true;
                }
                else if (fichier == nomUtilisateur)
                {
                    presentUtilisateur = true;
                }
            }
        }

        private void TesterFichier()
        {
            if (presentDefaut)
            {
                LectureOption(nomDefaut, optionDefaut);
                if (chargementReussis == false)
                {
                    mauvaisDefaut = true;
                }
            }
            else if (!presentDefaut)
            {
                mauvaisDefaut = true;
            }

            if (presentUtilisateur)
            {
                LectureOption(nomUtilisateur, optionUtilisateur);
                if (chargementReussis == false)
                {
                    mauvaisUtilisateur = true;
                }
            }
            else if (!presentUtilisateur)
            {
                mauvaisUtilisateur = true;
            }
        }

        public void Initialisation()
        {
            ChercheFichierConfig();
            TesterFichier();
            Console.WriteLine("Fichier utilisateur trouvé : " + presentUtilisateur);
            Console.WriteLine("Fichier defaut trouvé : " + presentDefaut);
            Console.WriteLine("Fichier utilisateur mal chargé : " + mauvaisUtilisateur);
            Console.WriteLine("Fichier defaut mal chargé : " + mauvaisDefaut);
        }

        public void Reparation()
        {

        }


        public BBTA_ConstructeurOption()
        {
            //On charge l'objet Option de base qui contient les paramètres d'usine du jeu.
            OptionBase();
        }

        public void EcritureOption(string FichierSortie, Option option)
        {
            try
            {
                ecriveur = new XmlTextWriter(FichierSortie, null);
                //ecriveur.Formatting = Formatting.Indented;
                //ecriveur.Indentation = 4;
                ecriveur.WriteStartDocument();

                serializer.Serialize(ecriveur, option);


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

        public void LectureOption(string FichierEntre, Option option)
        {

            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                option = (Option)serializer.Deserialize(lecteur);

                chargementReussis = true;
            }


            catch (Exception ex)
            {
                option = null;
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

        //public XNATileMapEditor.BBTA_Map.InfoCarte InformationCarte()
        //{
        //    if (chargementReussis == true)
        //    {
        //        return carte.InformationCarte;
        //    }
        //    return null;
        //}
    }
}

