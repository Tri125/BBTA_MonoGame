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
        private Option optionUtilisateur = new Option();
        private Option optionDefaut = new Option();
        private Option optionActive = new Option();
        private bool mauvaisUtilisateur;
        private bool mauvaisDefaut;
        private bool presentUtilisateur;
        private bool presentDefaut;
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
        public Option OptionActive { get { return optionActive; } }

        public BBTA_ConstructeurOption()
        {
            //On charge l'objet Option de base qui contient les paramètres d'usine du jeu.
            OptionBase();
        }

        private void OptionBase()
        {
            this.optionBase.InformationSonore.Musique = 100;
            this.optionBase.InformationSonore.EffetSonore = 100;

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
                LectureOption(nomDefaut, ref optionDefaut);
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
                LectureOption(nomUtilisateur, ref optionUtilisateur);
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
            Console.WriteLine("TESTE");
            Console.WriteLine("Fichier utilisateur trouvé : " + presentUtilisateur);
            Console.WriteLine("Fichier defaut trouvé : " + presentDefaut);
            Console.WriteLine("Fichier utilisateur mal chargé : " + mauvaisUtilisateur);
            Console.WriteLine("Fichier defaut mal chargé : " + mauvaisDefaut);
            if (mauvaisUtilisateur || mauvaisDefaut)
            {
                Reparation();
                Console.WriteLine("RÉPARATION");
                Console.WriteLine("Fichier utilisateur trouvé : " + presentUtilisateur);
                Console.WriteLine("Fichier defaut trouvé : " + presentDefaut);
                Console.WriteLine("Fichier utilisateur mal chargé : " + mauvaisUtilisateur);
                Console.WriteLine("Fichier defaut mal chargé : " + mauvaisDefaut);
            }
            Console.WriteLine("FINI");

            if (mauvaisUtilisateur || mauvaisDefaut)
            {
                Console.WriteLine("INCAPABLE DE RÉPARER");
                RetourBase();

            }
            else
            {
                RetourUtilisateur();
            }
        }


        private void Reparation()
        {
            if (mauvaisDefaut)
            {
                optionDefaut = new Option();
                EcritureOption(nomDefaut, optionBase);
                mauvaisDefaut = false;
                presentDefaut = false;
                ChercheFichierConfig();
                TesterFichier();
            }

            if (mauvaisUtilisateur)
            {
                optionUtilisateur = new Option();
                EcritureOption(nomUtilisateur, optionDefaut);
                mauvaisUtilisateur = false;
                presentUtilisateur = false;
                ChercheFichierConfig();
                TesterFichier();
            }
        }



        private void LectureOption(string FichierEntre, ref Option option)
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



        private void EcritureOption(string FichierSortie, Option option)
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

        public void EnregistrementUtilisateur(ref Option option)
        {
            EcritureOption(nomUtilisateur, option);
            Initialisation();
        }

        public void RetourDefaut()
        {
            if (mauvaisDefaut == false && presentDefaut == true)
            {
                optionActive = optionDefaut;
            }
            else
            {
                optionActive = optionBase;
            }
        }


        private void RetourUtilisateur()
        {
            if (mauvaisUtilisateur == false && presentUtilisateur == true)
            {
                optionActive = optionUtilisateur;
            }
            else
            {
                optionActive = optionBase;
            }
        }


        private void RetourBase()
        {
            optionActive = optionBase;
        }

    }
}

