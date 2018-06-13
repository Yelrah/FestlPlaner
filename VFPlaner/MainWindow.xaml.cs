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
    public partial class MainWindow : Window
    {

        private bool _loaded = false;
        int _arbeiterzahl;


        static string _path;
        public MainWindow()
        {
            InitializeComponent();
            GridLocalizer.Active = new CustomGridLocalizer();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() == true)
            {
                _loaded = true;
                doc = Core.SelectNodeMitarbeiter(of.FileName);

                XmlNode root = doc.DocumentElement;
                XmlNodeList list = root.SelectNodes("//Output/ServiceTeam/Mitarbeiter");


                foreach (XmlNode mitarbeiter in list)
                {
                    DataContext = new Mitarbeiter()
                    {
                        Vorname = (root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Vorname"))?.InnerText,
                        Name = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Nachname")?.InnerText,
                        Telefonnumer = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Telefonnumer")?.InnerText,
                        Tage = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/Tage")?.InnerText,
                        ErstesJahr = root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/ErstesJahr")?.InnerText,
                        AnzahlJahre = Int32.Parse(root.SelectSingleNode("//Output/ServiceTeam/Mitarbeiter/AnzahlJahre")?.InnerText),

                    };
                }
                _path = of.FileName;
                grid.ItemsSource = GetDataFromXML(of.FileName);
            }
        }
        public static DataTable GetDataFromXML(string path)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(path);
            return ds.Tables[0];
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            PostDataToXML();
        }

        public void PostDataToXML()
        {
            ((DataTable)grid.ItemsSource).DataSet.WriteXml(_path);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_loaded)
            {
                MAUbernahme();
            }
            else { MessageBox.Show("Laden Sie zuerst eine Datei", "Fehler", MessageBoxButton.OK); }
        }

        private void MAUbernahme()
        {
            if (Anzahl.Text.Equals(""))
            {
                MessageBox.Show("Tragen sie eine gültige Anzahl ein!", "Warnung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _arbeiterzahl = Int32.Parse(Anzahl.Text);
                int counter = 0;
                for (int i = 0; i < (grid.VisibleRowCount - 1); i++)
                {
                    string test = (string)grid.GetCellValue(i, "DiesesJahr");
                    if (  test.Equals("true") || test.Equals("True"))
                    {
                        counter++;
                    }
                }
                if (counter == _arbeiterzahl)
                {
                    //ToDo Something
                }
                else
                {
                    MessageBox.Show(string.Format("Sie haben {0} benötigen aber {1} Servicekräfte", counter, _arbeiterzahl), "Warnung!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
