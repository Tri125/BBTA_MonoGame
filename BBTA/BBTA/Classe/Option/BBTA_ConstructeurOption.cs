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
    /// <summary>
    /// La classe BBTA_ConstructeurOption gère la génération des fichiers XML pour enregistrer les paramètres de jeu de l'utilisateur, pour charger
    /// les fichiers de configuration déjà présent, détecter les erreurs et effectuer les remplacements néccéssaires.
    /// </summary>
    public class BBTA_ConstructeurOption
    {
        #region Attribut
        private Option optionUtilisateur;
        private Option optionDefaut;
        private Option optionActive;
        private bool mauvaisUtilisateur;
        private bool mauvaisDefaut;
        private bool presentUtilisateur;
        private bool presentDefaut;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer = null;
        private bool chargementReussis;
        #endregion

        #region Option Usine
        private Option optionBase;
        private const string nomDefaut = "defautConfig.xml";
        private const string nomUtilisateur = "utilisateurConfig.xml";

        private const int volMusiqueBase = 100;
        private const int volEffetSonoreBase = 100;
        private const Keys toucheGaucheBase = Keys.A;
        private const Keys toucheDroitBase = Keys.D;
        private const Keys toucheSautBase = Keys.Space;
        private const Keys toucheTirBase = Keys.Space;
        private const Keys touchePauseBase = Keys.P;
        #endregion

        public bool ChargementReussis { get { return chargementReussis; } }
        public Option OptionActive { get { return optionActive; } }
        public Option OptionDefaut { get { return optionDefaut; } }

        /// <summary>
        /// Constructeur de base de BBTA_ConstructeurOption.
        /// </summary>
        public BBTA_ConstructeurOption()
        {
            optionUtilisateur = new Option();
            optionDefaut = new Option();
            optionActive = new Option();
            optionBase = new Option();
            serializer = new XmlSerializer(typeof(Option));
            //On charge l'objet Option de base qui contient les paramètres d'usine du jeu.
            OptionBase();
        }

        /// <summary>
        /// Charge les paramètres d'usine dans un objet Option.
        /// Les paramètres d'usine sont utilisés lors de la réparation et de la création de fichiers XML des paramètres de jeu.
        /// </summary>
        private void OptionBase()
        {
            this.optionBase.InformationSonore.Musique = volMusiqueBase;
            this.optionBase.InformationSonore.EffetSonore = volEffetSonoreBase;

            this.optionBase.InformationTouche.Gauche = toucheGaucheBase;
            this.optionBase.InformationTouche.Droite = toucheDroitBase;
            this.optionBase.InformationTouche.Saut = toucheSautBase;
            this.optionBase.InformationTouche.Tir = toucheTirBase;
            this.optionBase.InformationTouche.Pause = touchePauseBase;
        }

        /// <summary>
        /// ChercheFichierConfig recherche les fichiers de configuration par défaut et de l'utilisateur
        /// d'après le nom des fichiers. Un attribut bool signale si oui ou non les fichiers sont présents.
        /// </summary>
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


        /// <summary>
        /// TesterFichier teste les fichiers de configuration s'ils ont été signalés comme étant présent.
        /// Si la lecture provoque une erreur le fichier est marqué.
        /// </summary>
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

        /// <summary>
        /// Initialise le chargement et le teste des fichiers de configuration.
        /// Vérifie si les fichiers ont été marqués comme étant défectueux.
        /// Remplace les mauvais fichiers.
        /// </summary>
        public void Initialisation()
        {
            ChercheFichierConfig();
            TesterFichier();
            Console.WriteLine("Initialisation des fichiers de configuration");
            Console.WriteLine("Fichier utilisateur trouvé ? : " + presentUtilisateur);
            Console.WriteLine("Fichier defaut trouvé ? : " + presentDefaut);
            Console.WriteLine("Fichier utilisateur mal chargé ? : " + mauvaisUtilisateur);
            Console.WriteLine("Fichier defaut mal chargé ? : " + mauvaisDefaut);
            if (mauvaisUtilisateur || mauvaisDefaut)
            {
                Reparation();
                Console.WriteLine("RÉPARATION");
                Console.WriteLine("Fichier utilisateur trouvé ?: " + presentUtilisateur);
                Console.WriteLine("Fichier defaut trouvé ? : " + presentDefaut);
                Console.WriteLine("Fichier utilisateur mal chargé ? : " + mauvaisUtilisateur);
                Console.WriteLine("Fichier defaut mal chargé ? : " + mauvaisDefaut);
            }
            Console.WriteLine("Fin Initialisation");

            //Si, malgré la réparation, un des fichiers est défectueux, alors on charge l'objet Option avec les paramètres d'usine.
            if (mauvaisUtilisateur || mauvaisDefaut)
            {
                Console.WriteLine("Incapable de réparer");
                RetourBase();

            }
            else
            {
                //Chargement du fichier de configuration de l'utilisateur.
                RetourUtilisateur();
            }
        }

        /// <summary>
        /// Reparation remplace l'objet Option et le fichier XML approprié par les paramètres d'usine.
        /// Vérifie que la réparation c'est bien effectuée.
        /// </summary>
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


        /// <summary>
        /// Lecture d'un fichier XML.
        /// </summary>
        /// <param name="FichierEntre">Nom du fichier.</param>
        /// <param name="option">Objet Option où les paramètres seront enregistrés.</param>
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

        /// <summary>
        /// Écriture d'un fichier XML à partir d'un objet Option.
        /// </summary>
        /// <param name="FichierSortie">Nom du fichier de sortie.</param>
        /// <param name="option">Objet Option qui sera écrit transformé en fichier XML.</param>
        private void EcritureOption(string FichierSortie, Option option)
        {
            try
            {
                ecriveur = new XmlTextWriter(FichierSortie, null);
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


        /// <summary>
        /// Enregistre un nouveau fichier de configuration de l'utilisateur à partir d'un objet Option et lance son chargement.
        /// </summary>
        public void EnregistrementUtilisateur(ref Option option)
        {
            EcritureOption(nomUtilisateur, option);
            Initialisation();
        }

        /// <summary>
        /// Change les options utilisés vers les paramètres par défaut.
        /// </summary>
        public void RetourDefaut()
        {
            if (mauvaisDefaut == false && presentDefaut == true)
            {
                optionActive = optionDefaut;
            }
            else
            {
                //Les paramètres par défaut sont défectueux, on utilise les paramètres d'usine.
                RetourBase();
            }
        }

        /// <summary>
        /// Change les options utilisés vers les paramètres de l'utilisateur.
        /// </summary>
        private void RetourUtilisateur()
        {
            if (mauvaisUtilisateur == false && presentUtilisateur == true)
            {
                optionActive = optionUtilisateur;
            }
            else
            {
                //Les paramètres de l'utilisateur sont défectueux, on utilise les paramètres d'usine.
                RetourBase();
            }
        }

        /// <summary>
        /// Change les options utilisés vers les paramètres de d'usine.
        /// </summary>
        private void RetourBase()
        {
            optionActive = optionBase;
        }

    }
}

