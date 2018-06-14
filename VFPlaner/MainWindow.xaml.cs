using System.Windows;
using System.Xml;
using VFPlaner.Klassen;
using Microsoft.Win32;
using System.Data;
using System;
using Controller.Grid;
using DevExpress.XtraGrid.Localization;

namespace VFPlaner
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private bool _loaded;
        private int _arbeiterzahl;
        DataSet Festschema;

        private static string _path;
        public MainWindow()
        {
            InitializeComponent();
            GridLocalizer.Active = new CustomGridLocalizer();
            //Festschema = 
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            var of = new OpenFileDialog();
            if (of.ShowDialog() != true) return;
            _loaded = true;
            var doc = Core.SelectNodeMitarbeiter(of.FileName);

            XmlNode root = doc.DocumentElement;
            var list = root?.SelectNodes("//Output/ServiceTeam/Mitarbeiter");

            _path = of.FileName;
            //Grid.ItemsSource = GetDataFromXml(of.FileName);
            Grid.ItemsSource = GetDataFromXml(@"C:\Users\vogla\Documents\Projects\VFPlaner\VFPlaner\VFPlaner\config\fest.xsd");
            
        }

        private static DataTable GetDataFromXml(string path)
        {
            var ds = new DataSet();
            ds.ReadXml(path);
            return ds.Tables[0];
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            PostDataToXml();
        }

        public void PostDataToXml()
        {
            if (_loaded)
            {
                ((DataTable)Grid.ItemsSource).DataSet.WriteXml(_path);
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
            if (Anzahl.Text.Equals(""))
            {
                MessageBox.Show("Tragen sie eine gültige Anzahl ein!", "Warnung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _arbeiterzahl = Int32.Parse(Anzahl.Text);
                var counter = 0;
                for (var i = 0; i < (Grid.VisibleRowCount - 1); i++)
                {
                    var test = (string)Grid.GetCellValue(i, "DiesesJahr");
                    if (test.Equals("true") || test.Equals("True"))
                    {
                        counter++;
                    }
                }
                if (counter != _arbeiterzahl)
                {
                    MessageBox.Show($"Sie haben {counter} eingetragen, benötigen aber {_arbeiterzahl} Servicekräfte", "Warnung!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //TO DO Something


                }
            }
        }

        private void SchmeißLadeFehler()
        {
            MessageBox.Show("Laden Sie zuerst eine Datei", "Fehler", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }




        //private int FindMax (int i)
        //{

        //}
    }
}
