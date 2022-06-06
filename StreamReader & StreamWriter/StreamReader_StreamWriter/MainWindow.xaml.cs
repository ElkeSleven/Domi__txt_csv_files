using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.IO;   //<-- deze moet je toevoegen om gebruik te kunnen maken van StreamReader en StreamWritter
using System.Data; //<-- deze moet je toevoegen om gerbuik te kunnen maken van DataTable , DataColun , DataSet , ..


namespace StreamReader_StreamWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //**DOMI(text.txt): opslaan en lezen van tekst.txt bestand met het gebruike van een List 
        List<string> listVoornamenAchternamen_txt = new List<string>();
        int id;
        string listReadHeader;

        //**DOMI(kommafile.csv): opslaan en lezen van een csv bestand met gerbuik van een List
        List<string> listVoornamenAchternamen_csv = new List<string>();



        //**
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Standaardwaarde();
        }


        //**
        public void Standaardwaarde()
        {

            //**DOMI(text.txt): voor het opslaan en lezen van tekst.txt bestand met het gebruike van een List 
            id = 0;
            listReadHeader = "";
            txtLijstVoornamenAchternamen.Text = "lijstMetPersonenen:";



            //**DOMI(kommafile.csv): opslaan en lezen van een csv bestand met gerbuik van een List
            //datatableVoornamenAchternamen = null;



        }


        //**tekst toevoegen aan list en ingevoerde text weergeven in de textbox 
        private void btnVoegRijToe_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtVoornaam.Text) && !string.IsNullOrEmpty(txtAchternaam.Text))
            {

                id++;
                txtLijstVoornamenAchternamen.Text += $"\n{id.ToString()}: {txtVoornaam.Text} {txtAchternaam.Text}";




                //**DOMI(text.txt): tekst toevoegen aan list en ingevoerde text weergeven in de textbox 
                listVoornamenAchternamen_txt.Add($"{id.ToString()}: {txtVoornaam.Text} {txtAchternaam.Text}");



                //**DOMI(kommafile.csv): 
                listVoornamenAchternamen_csv.Add($"{id.ToString()};{txtVoornaam.Text};{txtAchternaam.Text};");



            }
        }


        //**DOMI(text.txt): //** lezen van een tekst.txt bestand 
        private void btnLeesBestand_Click(object sender, RoutedEventArgs e)
        {
            string pad = "tekst.txt";
            FileInfo fi = new FileInfo(pad);
            if (fi.Exists) // enkel bestand lezen als het bestaat
            {
                using (StreamReader sr = fi.OpenText()) // maak StreamReader en open bestand
                {
                    listVoornamenAchternamen_txt.Clear();  //** lijst leegmaken 
                    while (!sr.EndOfStream)
                    {
                        /*
                         * "lijstMetPersonen"
                         * ""
                         * "1: vnaam anaam"
                         * "2: vnaam anaam" 
                         * "3: vnaam anaam" 
                         * "4: vnaam anaam" 
                         */
                        
                        string lijn = sr.ReadLine();

                        if (!string.IsNullOrEmpty(lijn))
                        {
                            if(lijn.Contains(" "))
                            {
                                listVoornamenAchternamen_txt.Add(lijn);
                            }
                            else
                            {
                                listReadHeader = lijn;                           
                            }
                        
                        }
                       
                        else
                        {
                            sr.ReadLine();
                        }


                    }
                    txtLijstVoornamenAchternamen.Clear();
                    txtLijstVoornamenAchternamen.Text = $"{listReadHeader}\n";


                    foreach (string persoon in listVoornamenAchternamen_txt)
                    {
                        txtLijstVoornamenAchternamen.Text += $"\n{persoon}";
                    }
                }
            }
            else
            {
                MessageBox.Show("file niet gevonden");
            }

        }


        //**DOMI(text.txt): //** opslaan van een tekst.txt bestand  
        private void btnBestandOpslaan_Click(object sender, RoutedEventArgs e)
        {

            string pad = "tekst.txt";
            FileInfo fi = new FileInfo(pad);
            if (!fi.Exists) // controleer of bestand nog niet bestaat
            {
                // maak StreamWriter en maak bestand aan
                using (StreamWriter sw = fi.CreateText())
                {
                    // schrijf tekst naar bestand
                    sw.WriteLine("lijstMetPersonen\n");

                    foreach(string persoon in listVoornamenAchternamen_txt)
                    {
                        sw.WriteLine(persoon);
                    }

                }
                MessageBox.Show("klaar");
            }
            else //** file bestaad al 
            {
                
                MessageBoxResult resaltaat = MessageBox.Show("het lijkt erop dat de file al bestaat wil je het vervangen ? ", "vervangen ? ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (MessageBoxResult.Yes == resaltaat)
                {
                    fi.Delete();
                    pad = "tekst.txt";
                    fi = new FileInfo(pad);
                    if (!fi.Exists) // controleer of bestand nog niet bestaat
                    {
                        // maak StreamWriter en maak bestand aan
                        using (StreamWriter sw = fi.CreateText())
                        {
                            // schrijf tekst naar bestand
                            sw.WriteLine("lijstMetPersonen\n");

                            foreach (string persoon in listVoornamenAchternamen_txt)
                            {
                                sw.WriteLine(persoon.ToString());
                            }
                        }
                        MessageBox.Show("klaar");
                    }
                    else
                    {
                        MessageBox.Show("er is iets fout gelopen bij het vervangen van het tekst.txt bestand\n" +
                            " probeer het handmatig te verwijderen en probeer opnieuw",
                            "onverwachte errow");
                    }
                }
            }


        }






        //**DOMI(kommafile.csv):
        private void btnBestandOpslaan_csv_Click(object sender, RoutedEventArgs e)
        {
            string pad = "kommafile.csv";
            FileInfo fi = new FileInfo(pad);
            if (!fi.Exists) // controleer of bestand nog niet bestaat
            {
                // maak StreamWriter en maak bestand aan
                using (StreamWriter sw = fi.CreateText())
                {
                    // schrijf tekst naar bestand
                    //sw.WriteLine("lijstMetPersonen\n"); niet doen bij een csv !

                    foreach (string persoon in listVoornamenAchternamen_csv)
                    {
                        sw.WriteLine(persoon);
                    }
                }
                MessageBox.Show("klaar");
            }
            else //** file bestaad al 
            {

                MessageBoxResult resaltaat = MessageBox.Show("het lijkt erop dat de file al bestaat wil je het vervangen ? ", "vervangen ? ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (MessageBoxResult.Yes == resaltaat)
                {
                    fi.Delete();
                    pad = "kommafile.csv";
                    fi = new FileInfo(pad);
                    if (!fi.Exists) // controleer of bestand nog niet bestaat
                    {
                        // maak StreamWriter en maak bestand aan
                        using (StreamWriter sw = fi.CreateText())
                        {
                            // schrijf tekst naar bestand
                            //sw.WriteLine("lijstMetPersonen\n"); niet doen bij een csv !

                            foreach (string persoon in listVoornamenAchternamen_csv)
                            {
                                sw.WriteLine(persoon);
                            }
                        }
                        MessageBox.Show("klaar");
                    }
                    else
                    {
                        MessageBox.Show("er is iets fout gelopen bij het vervangen van het tekst.txt bestand\n" +
                            " probeer het handmatig te verwijderen en probeer opnieuw",
                            "onverwachte errow");
                    }
                }
            }
        }


        //**DOMI(kommafile.csv):
        private void btnLeesBestand_csv_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    /** tips
     * 
     *//***FileInfo class***
     * *
     * fi.Exists         Testen of een bestand bestaat
     * fi.AppendText()   Maakt een StreamWriter aan en voegt tekst toe
     * fi.CreateText()   Maakt een StreamWriter aan en schrijft tekst weg
     * fi.OpenText()     Maakt een StreamReader aan en leest tekst
     * fi.OpenRead()     Maakt een readonly bestand
     * fi.OpenWrite      Maakt een write-only bestand
     * fi.Delete()       Verwijdert het bestand.
     * 
     * 
     * //***File class**
     * *
     * File.Append(pad)                   opent en voegt toe aan bestaand bestand
     * File.Create(pad)                   creëert en overschrijft het bestand in opgegeven pad
     * File.CreateText(pad)               creëert en overschrijft een tekstbestand (UTF-8 encoded)
     * File.Delete(pad)                   verwijdert bestand
     * File.Copy(pad)                     kopieert een bestand
     * File.Exists(pad)                   test of bestand bestaat (true/false)
     * File.OpenText(pad)                 opent een bestaand tekstbestand (UTF-8 encoded)
     * File.OpenWrite(pad)                opent een bestaand tekstbestand om weg te schrijven
     * File.OpenWrite(pad)                opent een bestaand tekstbestand om weg te schrijven
     * File.ReadAllLines(pad)             opent, lees alle lijnen in array en sluit
     * File.ReadAllLines(pad)             Length geeft aantal regels in bestand
     * File.ReadAllText(pad)              leest het volledige bestand
     * File.WriteAllText(pad)             schrijft het volledige bestand weg
     * File.AppendAllText(pad, string)    voegt tekst aan bestand toe
     * 
     * 

     * 
     * 
     * 
     * 
       *  txtLijstVoornamen.Text = listVoornamen.FirstOrDefault();     //** geeft alleen de eerst waarde van de list terug 
       *    txtLijstVoornamen.Text = listVoornamen.Count.ToString();   //** geet het aantal elementen in de list als een string  
       *        var a = listVoornamenAchternamen.ToArray();            //** set de list om naar een array 
       
       */

}
