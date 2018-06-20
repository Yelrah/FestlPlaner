using Controller.Grid;
using DevExpress.XtraGrid.Localization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Xml;

namespace VFPlaner
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private bool _loaded;
        private int _arbeiterZahl;
        private int _anzahlTeams;
        public string xsdLocation = @"C:\Users\vogla\Documents\Projects\VFPlaner\VFPlaner\VFPlaner\config\fest.xsd";

        private static string _path;
        public MainWindow()
        {
            InitializeComponent();
            GridLocalizer.Active = new CustomGridLocalizer();
            //Festschema = 
        }

        private void CreateNewVF(object sender, RoutedEventArgs e)
        {
            _loaded = true;
            Grid.ItemsSource = GetDataFromXsdXml(xsdLocation);
        }

        private void Load(object sender, RoutedEventArgs e)
        {

            var of = new OpenFileDialog();
            if (of.ShowDialog() != true) return;
            _loaded = true;

            try
            {
                if (Core.ValidationOfXml(xsdLocation, of.FileName))
                {

                    var doc = Core.SelectNodes(of.FileName, "//Volksfest");

                    XmlNode root = doc.DocumentElement;
                    var list = root?.SelectNodes("//Output/ServiceTeam/Mitarbeiter");

                    _path = of.FileName;
                    //Grid.ItemsSource = GetDataFromXml(of.FileName);
                    Grid.ItemsSource = GetDataFromXsdXml(_path);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Fehler beim Validieren: " + exc.Message);
            }
        }



        private static DataTable GetDataFromXsdXml(string path)
        {
            var ds = new DataSet();
            ds.ReadXml(path);
            return ds.Tables[0];
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            if (_loaded)
            {
                SaveFileDialog sf = new SaveFileDialog
                {
                    FileName = "_" + DateTime.Now.Year + ".xml"
                };

                if (sf.ShowDialog() == true)
                {

                    ((DataTable)Grid.ItemsSource).DataSet.WriteXml(sf.FileName);
                }
            }
            else { SchmeißLadeFehler(); }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_loaded)
            {
                MaUbernahme();
            }
            else
            {
                SchmeißLadeFehler();
            }
        }


        private void MaUbernahme()
        {
            if (Anzahl.Text.Equals(string.Empty))
            {
                MessageBox.Show("Tragen sie eine gültige Anzahl ein!", "Warnung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                int counter = CompareInputWithNeededWorkers();
                if (counter != _arbeiterZahl)
                {
                    MessageBox.Show(string.Format("Sie haben {0} eingetragen, benötigen aber {1} Servicekräfte", counter, _arbeiterZahl), "Warnung!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //TO DO Something
                    Teams.Text = CountTeams();


                }
            }
        }

        private int CompareInputWithNeededWorkers()
        {
            _arbeiterZahl = Int32.Parse(Anzahl.Text);
            var counter = 0;
            for (var i = 0; i < (Grid.VisibleRowCount - 1); i++)
            {
                var test = (string)Grid.GetCellValue(i, "DiesesJahr");
                if (test.Equals("true") || test.Equals("True"))
                {
                    counter++;
                }
            }

            return counter;
        }

        private string CountTeams()
        {
            HashSet<int> CountedTeams = new HashSet<int>();
            for (var i = 0; i < Grid.VisibleRowCount - 1; i++)
            {
                CountedTeams.Add(Int32.Parse(Grid.GetCellValue(i, "Team").ToString()));
            }
            _anzahlTeams = CountedTeams.Count;
            return CountedTeams.Count.ToString();
        }

        private void SchmeißLadeFehler()
        {
            MessageBox.Show("Laden oder erstellen Sie zuerst eine Datei", "Fehler", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void PrintGrid(object sender, RoutedEventArgs e)
        {
            try
            {
                gridview.Print();
            }
            catch
            {
                SchmeißLadeFehler();
            }
        }

        private void PrintLose(object sender, RoutedEventArgs e)
        {

        }

        private void Create_Lose(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32.Parse(Teams.Text);

            }

            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }

        }
    }
}
