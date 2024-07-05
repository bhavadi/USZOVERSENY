using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace uszoverseny
{
    public partial class Form1 : Form
    {
        private List<Versenyzo> versenyzok;

        public Form1()
        {
            InitializeComponent();
            versenyzok = new List<Versenyzo>();
        }

        class Versenyzo
        {
            public string Rajtszam { get; private set; }
            public string Nev { get; private set; }
            public DateTime SzulDatum { get; private set; }
            public string Orszag { get; private set; }
            public TimeSpan IdoEredmeny { get; private set; }

            public Versenyzo(string rajtszam, string nev, DateTime szulDatum,
                string orszag, TimeSpan idoEredmeny)
            {
                this.Rajtszam = rajtszam;
                this.Nev = nev;
                this.SzulDatum = szulDatum;
                this.Orszag = orszag;
                this.IdoEredmeny = idoEredmeny;
            }

            public override string ToString()
            {
                return Nev;
            }
        }

        private void AdatBeolvasas()
        {
            StreamReader olvasoCsatorna = new StreamReader("uszok.txt");
            string adat = olvasoCsatorna.ReadLine();
            while (adat != null)
            {
                Feldolgoz(adat);
                adat = olvasoCsatorna.ReadLine();
            }
            olvasoCsatorna.Close();
        }

        private void Feldolgoz(string adat)
        {
            string[] tordelt = adat.Split(';');
            string rajtSzam = tordelt[0];
            string nev = tordelt[1];
            DateTime szulDatum = DateTime.Parse(tordelt[2]);
            string orszag = tordelt[3];
            TimeSpan idoEredmeny = TimeSpan.Parse("0:" + tordelt[4]);
            Versenyzo versenyzo = new Versenyzo(rajtSzam, nev, szulDatum, orszag, idoEredmeny);
            versenyzok.Add(versenyzo);
            lstVersenyzok.Items.Add(versenyzo);
        }


        private void btnAdatBe_Click(object sender, EventArgs e)
        {
            lstVersenyzok.Items.Clear();
            versenyzok.Clear();
            AdatBeolvasas();
            btnAdatBe.Enabled = false;
            btnGyoztes.Enabled = true;
        }

        private void lstVersenyzok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVersenyzok.SelectedIndex != -1)
            {
                Versenyzo versenyzo = versenyzok[lstVersenyzok.SelectedIndex];
                txtRajtszam.Text = versenyzo.Rajtszam;
                txtOrszag.Text = versenyzo.Orszag;
                txtIdoEredmeny.Text = new DateTime(versenyzo.IdoEredmeny.Ticks).ToString("mm:ss.ff");
                txtEletKor.Text = (DateTime.Now.Year - versenyzo.SzulDatum.Year) + " év";
            }
        }

        private void btnGyoztes_Click(object sender, EventArgs e)
        {
            if (versenyzok.Count > 0)
            {
                TimeSpan min = versenyzok[0].IdoEredmeny;
                foreach (var versenyzo in versenyzok)
                {
                    if (versenyzo.IdoEredmeny < min)
                    {
                        min = versenyzo.IdoEredmeny;
                    }
                }
                txtGyoztesIdo.Text = new DateTime(min.Ticks).ToString("mm:ss:ff");
                rchTxtGyoztes.Clear();
                foreach (var versenyzo in versenyzok)
                {
                    if (versenyzo.IdoEredmeny == min)
                    {
                        rchTxtGyoztes.AppendText(versenyzo + "\n");
                    }
                }
            }
        }
    }
}
