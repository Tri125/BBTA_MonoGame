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
        private Option option;
        private XmlTextReader lecteur = null;
        private XmlTextWriter ecriveur = null;
        private XmlSerializer serializer = new XmlSerializer(typeof(Option));
        private bool chargementReussis;
        #endregion

        #region Option par Défaut
        private Option optionBase = new Option();
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

        public void LectureOption(string FichierEntre)
        {

            try
            {
                lecteur = new XmlTextReader(FichierEntre);
                this.option = (Option)serializer.Deserialize(lecteur);

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

