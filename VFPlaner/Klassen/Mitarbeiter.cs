using System.ComponentModel;

namespace VFPlaner.Klassen
{
    public class Mitarbeiter
    {
        [Category("Mitarbeiter")]
        public int ID { get; set; }
        [Category("Mitarbeiter")]
        public string Vorname { get; set; }
        [Category("Mitarbeiter")]
        public string Name { get; set; }
        [Category("Mitarbeiter")]
        public string Telefonnumer { get; set; }
        [Category("Mitarbeiter")]
        public string Tage { get; set; }
        [Category("Mitarbeiter")]
        public string ErstesJahr { get; set; }
        [Category("Mitarbeiter")]
        public int AnzahlJahre { get; set; }
        [Category("Mitarbeiter")]
        public bool DiesesJahr { get; set; }
        [Category("Mitarbeiter")]
        public string Team { get; set; }
    }
}
