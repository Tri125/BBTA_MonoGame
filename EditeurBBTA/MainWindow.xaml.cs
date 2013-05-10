#region File Description
//-----------------------------------------------------------------------------
// MainWindow.xaml.cs
//
// Copyright 2011, Nick Gravelyn.
// Licensed under the terms of the Ms-PL: http://www.microsoft.com/opensource/licenses.mspx#Ms-PL
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System.Collections.Generic;
using XNATileMapEditor;

namespace EditeurCarteXNA
{
    public partial class MainWindow : Window
    {

        private BBTA_ConstructeurCarte constructeurCarte;


        private const int modVitesseCam = 10;

        private int rangee;
        private int colonne;
        private int minimumJoueur;
        private int maximumJoueur;
        private int tailleGrille;

        private string derniereSortie;
        private Color couleur;
        private SpriteFont FontScore;
        SpriteBatch spriteBatch;
        private Texture2D texture1px;
        private Camera camera;
        private bool montreLaGrille;

        // pour les lignes
        private List<Rectangle> ligneGrille;
        //pour les tuiles
        private List<TuileEditeur> tuiles;


        public MainWindow()
        {
            constructeurCarte = new BBTA_ConstructeurCarte(3, "*.xml", "Carte");

            ligneGrille = new List<Rectangle>();
            tuiles = new List<TuileEditeur>();

            couleur = Color.Orange;
            rangee = 10;
            colonne = 10;
            tailleGrille = 30;
            montreLaGrille = true;
            InitializeComponent();
        }
        /// <summary>
        /// Invoked after either control has created its graphics device.
        /// </summary>
        private void loadContent(object sender, GraphicsDeviceEventArgs e)
        {
            camera = new Camera(new Vector2(130, 130));
            ContentManager contentManager = new ContentManager(xnaControl1.Services, "Content");
            spriteBatch = new SpriteBatch(e.GraphicsDevice);

            contentManager.RootDirectory = "Content";
            texture1px = new Texture2D(e.GraphicsDevice, 1, 1);
            texture1px.SetData(new Color[] { Color.White });

            FontScore = contentManager.Load<SpriteFont>(@"Fonts\menufont");

            DefinirGrille();

        }





        // We use the left mouse button to do exclusive capture of the mouse
        private void xnaControl1_HwndLButtonDown(object sender, HwndMouseEventArgs e)
        {
            Vector2 mousePos = Vector2.Transform(new Vector2((float)e.Position.X, (float)e.Position.Y), Matrix.Invert(camera.ViewMatrix));
            Console.WriteLine(mousePos);

            foreach (TuileEditeur tuile in tuiles)
            {
                if (tuile.Bloc.Contains((int)mousePos.X, (int)mousePos.Y))
                {

                    Console.WriteLine("ID de la Tuile cliqué: " + tuile.IdTuile);
                    tuile.IdTuile++;
                    Console.WriteLine("Augmentation de ID: " + tuile.IdTuile);
                }
            }
            xnaControl1.CaptureMouse();
        }

        private void xnaControl1_HwndLButtonUp(object sender, HwndMouseEventArgs e)
        {
            xnaControl1.ReleaseMouseCapture();
        }


        private void xnaControl1_HwndRButtonDown(object sender, HwndMouseEventArgs e)
        {

            Vector2 mousePos = Vector2.Transform(new Vector2((float)e.Position.X, (float)e.Position.Y), Matrix.Invert(camera.ViewMatrix));
            Console.WriteLine(mousePos);

            foreach (TuileEditeur tuile in tuiles)
            {
                if (tuile.Bloc.Contains((int)mousePos.X, (int)mousePos.Y))
                {

                    Console.WriteLine("ID de la Tuile cliqué: " + tuile.IdTuile);
                    tuile.IdTuile--;
                    Console.WriteLine("Réduction de ID: " + tuile.IdTuile);
                }
            }
            xnaControl1.CaptureMouse();

        }

        private void xnaControl1_HwndRButtonUp(object sender, HwndMouseEventArgs e)
        {
            xnaControl1.ReleaseMouseCapture();
        }

        /// <summary>
        /// Invoked when our first control is ready to render.
        /// </summary>
        private void xnaControl1_RenderXna(object sender, GraphicsDeviceEventArgs e)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {

                camera.Zoom -= 0.01f;
            }
            else
                if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                {
                    camera.Zoom += 0.01f;
                }


            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camera.Move(new Vector2(-modVitesseCam, 0));
            }
            else
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    camera.Move(new Vector2(modVitesseCam, 0));
                }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camera.Move(new Vector2(0, -modVitesseCam));
            }
            else
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    camera.Move(new Vector2(0, modVitesseCam));
                }

            e.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.CreateTransformationMatrix(e.GraphicsDevice));

            if (montreLaGrille)
            {
                foreach (Rectangle ligne in ligneGrille)
                {
                    spriteBatch.Draw(texture1px, ligne, Color.Red);
                }

                foreach (TuileEditeur tuile in tuiles)
                {
                    spriteBatch.DrawString(FontScore, tuile.IdTuile.ToString(), new Vector2(tuile.Bloc.X, tuile.Bloc.Center.Y - 15), couleur);
                }
            }
            else
            {
                foreach (TuileEditeur tuile in tuiles)
                {
                    if (tuile.IdTuile != 0)
                    {
                        spriteBatch.DrawString(FontScore, tuile.IdTuile.ToString(), new Vector2(tuile.Bloc.X, tuile.Bloc.Center.Y - 15), couleur);
                    }
                }
            }

            spriteBatch.End();

        }

        private void AuthorItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Le code intégrant XNA 4.0 dans un projet WPF est de Nick Gravelyn pris à http://blogs.msdn.com/b/nicgrave/"
                + Environment.NewLine + "Le programme a été créé par Tristan Savaria (tristan@twisted-ip.com)");
        }

        private void btnSetSize_Click(object sender, RoutedEventArgs e)
        {
            bool resultatLongueur;
            bool resultatHauteur;
            int convertie;
            resultatLongueur = int.TryParse(mapWidth.Text, out convertie);

            resultatHauteur = int.TryParse(mapHeight.Text, out convertie);
            if (resultatLongueur && resultatHauteur)
            {
                rangee = int.Parse(mapHeight.Text);
                colonne = int.Parse(mapWidth.Text);
                DefinirGrille();
            }
            else
            {
                MessageBox.Show("Erreur de réglages sur le nombre de colonnes et de rangees de la grille"
                    + Environment.NewLine + "Avez-vous écris des chiffres?", "Vous avez détruis l'univers!");
            }
        }

        private void chkbxGrid_Checked(object sender, RoutedEventArgs e)
        {
            montreLaGrille = true;
        }

        private void chkbxGrid_Unchecked(object sender, RoutedEventArgs e)
        {
            montreLaGrille = false;
        }

        private void DefinirGrille()
        {
            ligneGrille.Clear();
            tuiles.Clear();

            //vertical
            for (float x = 0; x <= colonne; x++)
            {
                ligneGrille.Add(new Rectangle((int)(x * tailleGrille), 0, 1, (tailleGrille * rangee)));
            }
            //horizontal
            for (float y = 0; y <= rangee; y++)
            {
                ligneGrille.Add(new Rectangle(0, (int)(y * tailleGrille), (tailleGrille * colonne), 1));

            }

            for (int i = 0; i < rangee; i++)
            {
                for (int j = 0; j < colonne; j++)
                {

                    tuiles.Add(new TuileEditeur(new Rectangle((int)(j * tailleGrille) + 1, (int)(i * tailleGrille) + 1, tailleGrille - 2, tailleGrille - 2)));
                }

            }
            Console.WriteLine("Nombre de Tuiles:" + tuiles.Count());

        }



        private void btnOutPutXML_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(outputPath.Text))
            {
                MessageBox.Show("Nom de sortie vide.", "Ne peux pas enregistrer le fichier");
                return;
            }
            if (TesteDeJoueurMinimum() == true)
            {
                derniereSortie = outputPath.Text;
                constructeurCarte.EcritureCarte(derniereSortie, new BBTA_Carte(mapName.Text, colonne, rangee, minimumJoueur, maximumJoueur, tuiles));
                constructeurCarte.LectureCarte(derniereSortie);


                BBTA_Carte.InfoCarte info = constructeurCarte.InformationCarte();
            }
        }

        private bool TesteDeJoueurMinimum()
        {
            bool MinResult;
            bool MaxResult;
            int testValue;
            MinResult = int.TryParse(minPlayer.Text, out testValue);

            MaxResult = int.TryParse(maxPlayer.Text, out testValue);
            if (MinResult && MaxResult)
            {
                minimumJoueur = int.Parse(minPlayer.Text);
                maximumJoueur = int.Parse(maxPlayer.Text);
                if (minimumJoueur > maximumJoueur)
                {
                    int iTemp = maximumJoueur;
                    maximumJoueur = minimumJoueur;
                    minimumJoueur = iTemp;

                    minPlayer.Text = minimumJoueur.ToString();
                    maxPlayer.Text = maximumJoueur.ToString();
                }

                return true;
            }
            else
            {
                MessageBox.Show("Erreur sur le nombre de joueurs minimum et maximum"
                    + Environment.NewLine + "Avez-vous écris des chiffres?", "Vous avez détruit l'univers!");
                return false;
            }
        }

        private void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Utilisé les touches Haut, Bas, Gauche et Droit du clavier pour déplacer la caméra."
            + Environment.NewLine + "Appuyez sur PageHaut pour avancer la caméra ou bien PageBas pour reculer la caméra"
                + Environment.NewLine + "Appuyez sur le bouton gauche de la souris sur une tuile pour incrémenté son identifiant"
                     +"ou appuyez sur le bouton de droit pour le décrémenter.", "Contrôle");
        }

        private void btnInPutXML_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(inputPath.Text))
            {
                MessageBox.Show("Nom d'entrée vide", "Ne peux pas charger le fichier");
                return;
            }

            constructeurCarte.LectureCarte(inputPath.Text);

            if (constructeurCarte.ChargementReussis)
            {
                rangee = constructeurCarte.InformationCarte().NbRange;
                mapHeight.Text = constructeurCarte.InformationCarte().NbRange.ToString();
                colonne = constructeurCarte.InformationCarte().NbColonne;
                mapWidth.Text = constructeurCarte.InformationCarte().NbColonne.ToString();
                minimumJoueur = constructeurCarte.InformationCarte().NbJoueurMin;
                minPlayer.Text = constructeurCarte.InformationCarte().NbJoueurMin.ToString();
                maximumJoueur = constructeurCarte.InformationCarte().NbJoueurMax;
                maxPlayer.Text = constructeurCarte.InformationCarte().NbJoueurMax.ToString();
                mapName.Text = constructeurCarte.InformationCarte().NomCarte;
                DefinirGrille();

                for (int iBoucle = 0; iBoucle < tuiles.Count(); iBoucle++)
                {
                    tuiles[iBoucle].IdTuile = constructeurCarte.InfoTuileListe()[iBoucle];
                }
            }

            else
            {
                MessageBox.Show("Le fichier carte est d'un format invalide ou le nom est éronné.", "Ne peux pas charger le fichier");
            }
        }




    }
}
