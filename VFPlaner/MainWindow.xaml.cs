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


        private static string _path;
        public MainWindow()
        {
            InitializeComponent();
            GridLocalizer.Active = new CustomGridLocalizer();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            var of = new OpenFileDialog();
            if (of.ShowDialog() != true) return;
            _loaded = true;
            var doc = Core.SelectNodeMitarbeiter(of.FileName);

            XmlNode root = doc.DocumentElement;
            var list = root?.SelectNodes("//Output/ServiceTeam/Mitarbeiter");


            if (list != null)
                foreach (XmlNode mitarbeiter in list)
                {
                    DataContext = new Mitarbeiter()
                    {
                        Vorname = (root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Vorname"))?.InnerText,
                        Name = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Nachname")?.InnerText,
                        Telefonnumer =
                            root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Telefonnumer")?.InnerText,
                        Tage = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Tage")?.InnerText,
                        ErstesJahr = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/ErstesJahr")?.InnerText,
                        AnzahlJahre =
                            Int32.Parse(
                                root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/AnzahlJahre")?.InnerText ?? throw new Exception("Jahreszahl ist keine Zahl!")),
                    };
                }

            _path = of.FileName;
            Grid.ItemsSource = GetDataFromXml(of.FileName);
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
    }
}
