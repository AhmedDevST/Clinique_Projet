using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using System.Threading.Tasks;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour home.xaml
    /// </summary>
    public partial class home : UserControl
    {
        static bool isClicked = false;
        static bool isClicked1 = false;
        ObservableCollection<RendezVous> p,p1;
        public DispatcherTimer timer;
        Utilisateur_Class user;


        public SqlTableDependency<DMRendezVous> RendezVousSqlTableDependency;
        public home(Utilisateur_Class user)
        {
           
            try
            {
                InitializeComponent();

                this.user = user;
                // si est un medcine ou  admin
                if (Convert.ToInt32( user.Role_user) == 1)
                {
                    nouveau_rendezVous.Visibility = Visibility.Collapsed;     
                }

                waleft.Click += Ajouter_Click;
               
                load_sticky_note();
                aujourdhui.BorderBrush = Brushes.Violet;
                boxShadow(aujourdhui, 0);
                isClicked1 = false;
                isClicked = true;

            }
            catch (Exception)
            { 
                MessageBox.Show("Opération d'entrée innatendu N M007 !!"); 
            }   
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => Start_TableRendezVous_Dependency() );
            }
            catch (Exception)
            {
            }
        }

        private  async void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
              await Task.Run(() =>  Stop_TableRendezVous_Dependency() );
            }
            catch (Exception)
            {
            }
        }

        //button nouveau render vous 
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Nouveau_RendezVous test = new Nouveau_RendezVous(user);
                Acceuil? acceuil = Window.GetWindow(this) as Acceuil;
                if (acceuil != null)
                {
                    acceuil.ChangerControleur(test);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M007 !!");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Hide the element
            Erreur.Visibility = Visibility.Hidden;

            // Stop the timer
            timer.Stop();
        }

        //button : les rendez vous d'aujourdhui
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //new_list(sticky_note, 3).Background = Set_Image("C:\\Users\\pcNB2\\source\\repos\\WpfApp6\\WpfApp6\\Image\\sticky-notes-vector-3670939.jpg");
                sticky_note.Children.Clear();
                load_sticky_note();
                liste.BorderBrush = Brushes.Gray;
                aujourdhui.BorderBrush = Brushes.Violet;
                boxShadow(aujourdhui, 0);
                boxShadow(liste, 1);
                isClicked1 = false;
                isClicked = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button : list des tous  rendez vous 
        private void rendezvous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Liste_RendezVous liste_RendezVous = new Liste_RendezVous(user);
                aucun_rendezVous.Visibility = Visibility.Hidden;
                sticky_note.Children.Clear();
                sticky_note.Children.Add(liste_RendezVous);
                liste.BorderBrush = Brushes.Violet;
                aujourdhui.BorderBrush = Brushes.Gray;

                // Créer l'effet de drop shadow
                boxShadow(liste, 0);
                boxShadow(aujourdhui, 1);
                isClicked1 = true;
                isClicked = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void liste_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isClicked1)
                    liste.BorderBrush = Brushes.Violet;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void liste_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isClicked1)
                    liste.BorderBrush = Brushes.Gray;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void aujourdhui_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isClicked)
                    aujourdhui.BorderBrush = Brushes.Violet;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void aujourdhui_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isClicked)
                    aujourdhui.BorderBrush = Brushes.Gray;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS  -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        //----------- start  sqlTable Dependecy : rendez Vous --------
        private void Start_TableRendezVous_Dependency()
        {
            try
            {
                if (RendezVousSqlTableDependency == null)
                {
                    RendezVousSqlTableDependency = new SqlTableDependency<DMRendezVous>(ConnectDb.GetConnectionstring(), "RendezVous");
                    RendezVousSqlTableDependency.OnChanged += Changed_RendezVous_Dependency;
                    RendezVousSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {       
            }
        }
        private void Stop_TableRendezVous_Dependency()
        {
            try
            {
                RendezVousSqlTableDependency?.Stop();
            }
            catch (Exception)
            {
            }
        } 
        private void Changed_RendezVous_Dependency(object sender, RecordChangedEventArgs<DMRendezVous> e)
        {
            try
            {
                if (e.ChangeType != ChangeType.None)
                {
                    Dispatcher.Invoke(() =>
                    {
                        sticky_note.Children.Clear();
                        load_sticky_note();
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        //----------- end  sqlTable Dependecy : rendez Vous --------

        public  void load_sticky_note()
        {
            p = RendezVous.DisplayRendezVous(-2);
            int i = 0;
            while (p.Count > i)
            {
                new_list(sticky_note, p[i].id, p[i].date.ToString("dddd dd MMMM ", new CultureInfo("fr-FR")),
                     p[i].heure.ToString(), p[i].type, p[i].nom, p[i].prenom).Style = Application.Current.Resources["BackgroundImageStyle"] as Style;

                //  new_list(sticky_note, p[i].id, p[i].date.ToString("dddd dd MMMM ", new CultureInfo("fr-FR")), p[i].heure.ToString(), p[i].type, p[i].nom, p[i].prenom).Background = Set_Image("C:\\Users\\LENOVO\\Documents\\Clinique_Projet\\Clinique_Projet\\Clinique_Projet\\images\\sticky-notes.png");
                i++;               
            }
            if (i == 0)
            {
                aucun_rendezVous.Visibility = Visibility.Visible;
            }
            else aucun_rendezVous.Visibility = Visibility.Collapsed;
        }
       
        public ImageBrush Set_Image1(string resourceName)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(resourceName, UriKind.Relative));
            return brush;
        }
   
        public ImageBrush Set_Image(string path)
        {
            // Créer une nouvelle instance de BitmapImage
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            // Créer une nouvelle instance de ImageBrush avec l'image
            return new ImageBrush(bitmap);

        }
     
        public ListView new_list(WrapPanel test, int id,string date,string time,string type,string Nom,string Prenom)
        {
            ListView list = new ListView();
            ListViewItem item = new ListViewItem();
            
            Nom = Nom + " " + Prenom;
            list.Items.Add(Nom);
            list.Items.Add(date);
            list.Items.Add(time);
            list.Items.Add(type);
            Thickness margin = new Thickness();
            margin.Left = 0; margin.Top = 0; margin.Right = 0; margin.Bottom = 0;
            list.BorderThickness = margin;
            list.Items.Add(item);
            //list.Items.Add("12:10");
            item.Content = id;
            item.Visibility = Visibility.Collapsed;
            list.Foreground = Brushes.Black;
            list.FontSize = 13;
            list.FontWeight = FontWeights.SemiBold;
            list.FontStyle = FontStyles.Italic;
            list.HorizontalContentAlignment = HorizontalAlignment.Center;
            // label.Padding = Conver;
            list.Width = 200;
            list.Height = 185;
            list.MouseDown += (sender, e) =>
            {
                try
                {
                   detail_rendezVous detail_RendezVous1 = new detail_rendezVous((int)item.Content, user);
                    detail_RendezVous1.ShowDialog();
                    if(detail_RendezVous1.IsLoad_fiche_patient)
                    {
                        Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                        if (acceuil != null)
                        {
                            acceuil.ChangerControleur(new Fiche_Patient_Controlle(detail_RendezVous1.Patient_ID,user));
                        }
                    }
                    else
                    {
                        sticky_note.Children.Clear();
                        load_sticky_note();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Opération d'entrée innatendu !!");
                }
            };
            test.Children.Add(list);
            return list;
        }
  
        private void boxShadow(Border border,int opacity)
        {
            try
            {
                DropShadowEffect dropShadowEffect = new DropShadowEffect();
                dropShadowEffect.ShadowDepth = 0;
                dropShadowEffect.Color = Colors.Black;
                if (opacity == 0)
                    dropShadowEffect.Opacity = 0.8;
                else
                    dropShadowEffect.Opacity = 0;
                dropShadowEffect.BlurRadius = 10;

                // Appliquer l'effet au bouton
                border.Effect = dropShadowEffect;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
          
        }

        //-----------------------  END  METHODES   ------------------------------------ ----

    }
}
