using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Clinique_Projet.Modal;
using System.Collections.ObjectModel;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Dashbord_Interface.xaml
    /// </summary>
    public partial class Dashbord_Interface : UserControl
    {
        public DateTime d1, d2;
        public SeriesCollection SeriesCollection_Consultation { get; set; }
        public SeriesCollection SeriesCollection_RDV { get; set; }

        public Func<ChartPoint, string> PointLabel { get; set; }
        public ObservableCollection<string> myList_labels;
        public Func<double, string> Formatter { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Dashbord_Interface()
        {
            try
            {
                InitializeComponent();
                PointLabel = chartPoint =>
                string.Format("{0}", chartPoint.Y, chartPoint.Participation);
                load_combobx();
                Nombre_Consultation.Text = Dashbord_Class.Count_Consultation().ToString();
                Nombre_Patient.Text = Dashbord_Class.Count_Patient().ToString();
                Nombre_RDV.Text = Dashbord_Class.Count_RendezVous().ToString();
                Nombre_Certificat.Text = Dashbord_Class.Count_Certificates().ToString();
                load_chart_consultation(DateTime.Today.AddDays(-7), DateTime.Now);
                load_chart_RDV(DateTime.Today.AddDays(-7), DateTime.Now);
                load_chart_asssurance();
                load_chart_GroupSang();
                Load_chart_CategorieSex();
                load_chart_age_Patient();
                Load_chart_Categorie_Certificate();
                DataContext = this;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

        //event combobox chart consultation 
        private void combobox_chart_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combobox_chart.SelectedValue != null)
                {
                    int value = Convert.ToInt32(combobox_chart.SelectedValue);
                    SeriesCollection_Consultation.Clear();
                    switch (value)
                    {
                        //last 7 days
                        case 1:
                            load_chart_consultation(DateTime.Today.AddDays(-7), DateTime.Now);
                            break;
                        //this month
                        case 2:
                            load_chart_consultation(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Now);
                            break;
                        //today
                        case 3:
                            load_chart_consultation(DateTime.Today, DateTime.Now);
                            break;
                        //this years
                        case 4:
                            load_chart_consultation(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
                            break;
                        //last 7 days
                        default:
                            load_chart_consultation(DateTime.Today.AddDays(-7), DateTime.Now);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //event combobox chart rdv
        private void combobox_chart_rdv_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combobox_chart_rdv.SelectedValue != null)
                {
                    int value = Convert.ToInt32(combobox_chart_rdv.SelectedValue);
                    SeriesCollection_RDV.Clear();
                    switch (value)
                    {
                        //last 7 days
                        case 1:
                            load_chart_RDV(DateTime.Today.AddDays(-7), DateTime.Now);
                            break;
                        //this month
                        case 2:
                            load_chart_RDV(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Now);
                            break;
                        //today
                        case 3:
                            load_chart_RDV(DateTime.Today, DateTime.Now);
                            break;
                        //this years
                        case 4:
                            load_chart_RDV(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
                            break;
                        //last 7 days
                        default:
                            load_chart_RDV(DateTime.Today.AddDays(-7), DateTime.Now);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //event click : chart patient par  Sex
        private void PieChart_Sex_DataClick(object sender, ChartPoint chartPoint)
        {
            try
            {
                Method_DataClick(chartPoint);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //event click : chart patient par age
        private void PieChart_agePatient_DataClick(object sender, ChartPoint chartPoint)
        {
            try
            {
                Method_DataClick(chartPoint);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        //event click : chart  patient par categorie  Group de Sang
        private void PieChart_Sang_DataClick(object sender, ChartPoint chartPoint)
        {
            try
            {
                Method_DataClick(chartPoint);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //event click : chart  patient par categorie  assurance
        private void PieChart_Assurannce_DataClick(object sender, ChartPoint chartPoint)
        {
            try
            {
                Method_DataClick(chartPoint);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //event click : chart Certificat
        private void PieChart_Certificate_DataClick(object sender, ChartPoint chartPoint)
        {
            try
            {
                Method_DataClick(chartPoint);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        //load chart :  patient par  Sex
        private void Load_chart_CategorieSex()
        {
            List<KeyValuePair<int, string>> List = Dashbord_Class.Categorie_Sex();

            if (List != null && List.Count > 0)
            {
                // Initialize the Series collection with new PieSeries instances
                PieChart_Sex.Series = new SeriesCollection();
                foreach (var item in List)
                {
                    var series = new PieSeries();
                    series.Title = item.Value;
                    series.LabelPoint = PointLabel;
                    series.DataLabels = true;
                    series.Values = new ChartValues<int> { item.Key };
                    PieChart_Sex.Series.Add(series);
                }
            }
        }

        //load chart pie : patient par categorie  assurance
        private void load_chart_asssurance()
        {
            List<KeyValuePair<int, string>> List = Dashbord_Class.Categorie_Assurance();
         
            if (List != null && List.Count > 0)
            {
                // Initialize the Series collection with new PieSeries instances
                PieChart_Assurannce.Series = new SeriesCollection();
                foreach (var item in List)
                {
                    var series = new PieSeries();
                    series.Title = item.Value;
                    series.LabelPoint = PointLabel;
                    series.DataLabels = true;
                    series.Values = new ChartValues<int> { item.Key };
                    PieChart_Assurannce.Series.Add(series);
                }
            }
         }

        //load chart pie : patient par categorie  Group de Sang
        private void load_chart_GroupSang()
        {
            List<KeyValuePair<int, string>> List = Dashbord_Class.Categorie_GroupSang();
            if (List != null && List.Count > 0)
            {
                // Initialize the Series collection with new PieSeries instances
                PieChart_Sang.Series = new SeriesCollection();
                foreach (var item in List)
                {
                    var series = new PieSeries();
                    series.Title = item.Value;
                    series.LabelPoint = PointLabel;
                    series.DataLabels = true;
                    series.Values = new ChartValues<int> { item.Key };
                    PieChart_Sang.Series.Add(series);
                }
            }
         }

        //load chart :  patient par age
        private void load_chart_age_Patient()
        {
            List<KeyValuePair<int, string>> List = Dashbord_Class.Categorie_Patient();
            if (List != null && List.Count > 0)
            {
                // Initialize the Series collection with new PieSeries instances
                PieChart_agePatient.Series = new SeriesCollection();
                foreach (var item in List)
                {
                    var series = new PieSeries();
                    series.Title = item.Value;
                    series.LabelPoint = PointLabel;
                    series.DataLabels = true;
                    series.Values = new ChartValues<int> { item.Key };

                    PieChart_agePatient.Series.Add(series);
                }
            }
        }

        // Combobox : filtre pr periodes : mois, semaine , ans , Ajourdui
        private void load_combobx()
        {
           
            List<KeyValuePair<int, string>> List_btn = new List<KeyValuePair<int, string>>();
            List_btn.Add(new KeyValuePair<int, string>(1, "ce semaine"));
            List_btn.Add(new KeyValuePair<int, string>(2, "ce mois"));
            List_btn.Add(new KeyValuePair<int, string>(4, "cette ans"));
            List_btn.Add(new KeyValuePair<int, string>(3, "Ajourdui"));
            combobox_chart.ItemsSource = List_btn;
            combobox_chart.DisplayMemberPath = "Value";
            combobox_chart.SelectedValuePath = "Key";
            combobox_chart.SelectedIndex = 0;
            combobox_chart_rdv.ItemsSource = List_btn;
            combobox_chart_rdv.DisplayMemberPath = "Value";
            combobox_chart_rdv.SelectedValuePath = "Key";
            combobox_chart_rdv.SelectedIndex = 0;

        }
           
        private void Method_DataClick(ChartPoint chartPoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        //load chart :  nombre consultation par date  
        private void load_chart_consultation(DateTime date1,DateTime date2)
        {
            try
            {                
                ChartValues<int> Values_charts = new ChartValues<int>();
                List<KeyValuePair<int, DateTime>> list = Dashbord_Class.Count_Consultation_ByDate(date1, date2);
                ObservableCollection<string> Labels = new ObservableCollection<string>();
                foreach (var item in list)
                {
                    Values_charts.Add(item.Key);
                    Labels.Add(item.Value.ToString("dd, MMM, yyyy"));  
                }

                 SeriesCollection_Consultation = new SeriesCollection
               {
                   new LineSeries
                   {
                       Title = "Consultation",
                       Values =Values_charts,
                        DataLabels = true,
                        LabelPoint = point => $"{point.Y.ToString("#0")}"
                   }

               };
              
                Chart_Consultation.Series= SeriesCollection_Consultation;
                axis_consult.Labels = Labels;
                YFormatter = value => value.ToString("0");
                ayis_consult.LabelFormatter = YFormatter;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load chart :  nombre  rendez vous  par date 
        private void load_chart_RDV(DateTime date1, DateTime date2)
        {
            try
            {
                ChartValues<int> Values_charts = new ChartValues<int>();
                List<KeyValuePair<int, DateTime>> list = Dashbord_Class.Count_RDV_ByDate(date1, date2);
                ObservableCollection<string> Labels = new ObservableCollection<string>();
                foreach (var item in list)
                {
                    Values_charts.Add(item.Key);
                    Labels.Add(item.Value.ToString("dd, MMM, yyyy"));
                }

                SeriesCollection_RDV = new SeriesCollection
               {
                   new LineSeries
                   {
                       Title = "Rendez vous",
                       Values =Values_charts,
                        DataLabels = true,
                        LabelPoint = point => $"{point.Y.ToString("#0")}"
                   }

               };

                Chart_RDV.Series = SeriesCollection_RDV;
                axis_rdv.Labels = Labels;
                YFormatter = value => value.ToString("0");
                ayis_rdv.LabelFormatter = YFormatter;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load chart certificate
        private void Load_chart_Categorie_Certificate()
        {
            List<KeyValuePair<int, string>> List = Dashbord_Class.Categorie_Certificate();
            if (List != null && List.Count > 0)
            {
                // Initialize the Series collection with new PieSeries instances
                PieChart_Certificate.Series = new SeriesCollection();
                foreach (var item in List)
                {
                    var series = new PieSeries();
                     series.Title = item.Value;
                     series.LabelPoint = PointLabel;
                     series.DataLabels = true;
                     series.Values = new ChartValues<int> { item.Key };

                     PieChart_Certificate.Series.Add(series);
                }
            }
        }

        //-----------------------  END  METHODES    ------------------------------------ ----

    }
}
      