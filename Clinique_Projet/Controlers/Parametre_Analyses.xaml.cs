using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Parametre_Analyses.xaml
    /// </summary>
    public partial class Parametre_Analyses : UserControl
    {
        ObservableCollection<AnalyseClass> ListAnalyses;
        ObservableCollection<AnalyseClass> ListAnalyses_modules;// list qui recupere les donnes de patients
        public AnalyseClass Obj_Analyse;
        public TypeAnalyse Obj_TypeAnalyse;
        public Module_Analyse_Class obj_module;
        public Utilisateur_Class user;
        public Parametre_Analyses(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Load_Analyse();
                Load_TypeAnalyse();
                Load_TypeAnalyse_search();
                load_TypeAnalyse_DataGrid();
                load_module_analyse();
                Load_TypeAnalyse_module();
                initiuliser_btn();
                this.user = user;
                Droits_user();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            } 
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        
        
        //---------- Gestion des Analyse ---------

        //search analyse
        private void Text_Search_Analyse_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchTerm = Text_Search_Analyse.Text.ToLower();   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = ListAnalyses.Where(item =>
                  item.NomAnalyse.ToLower().Contains(searchTerm) ||
                   item.TypeAnalyse.ToLower().Contains(searchTerm)
               );
                    datagrid_Analyse.ItemsSource = filteredData;  //affecter le result a datagrid
                }
                else datagrid_Analyse.ItemsSource = ListAnalyses; //si le textbox est vide on loaddatagrid
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //search combobox by type analyse
        private void comboboxType_Analyse_Search_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                Load_Analyse();
                if (comboboxType_Analyse_Search.SelectedItem != null)
                {
                    string searchTerm = comboboxType_Analyse_Search.Text;//get text from combobox
                    if ((!string.IsNullOrEmpty(searchTerm)) && (searchTerm != "Tous")) // si le textbox n'est pas vide
                    {
                        ObservableCollection<AnalyseClass> filteredData = new ObservableCollection<AnalyseClass>(
                        ListAnalyses.Where(item => item.TypeAnalyse.Contains(searchTerm)));
                        ListAnalyses = filteredData;
                        datagrid_Analyse.ItemsSource = filteredData;
                    }
                    else Load_Analyse();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //add analyse
        private void AddAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(Nom_Analyse.Text)) && (comboboxType_Analyse.SelectedItem != null))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Enregistrer ce Analyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        AnalyseClass analyse = new AnalyseClass(AnalyseClass.GetLast_Id() + 1, Nom_Analyse.Text, comboboxType_Analyse.SelectedValue.ToString());
                        if (analyse.AddAnalyse())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs();
                            Annuler_operation_module();
                        }
                    }
                }
                else MessageBox.Show("les champs  sont vides!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //update
        private void EditAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(Nom_Analyse.Text)) && (comboboxType_Analyse.SelectedItem != null))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifier ce Analyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        AnalyseClass analyse = new AnalyseClass(Obj_Analyse.IdAnalyse, Nom_Analyse.Text, comboboxType_Analyse.SelectedValue.ToString());
                        if (analyse.UpdateAnalyse())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs();
                            Annuler_operation_module();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("les champs sont  vides!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //show analyse
        private void show_analyse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Analyse.SelectedItem != null)
                {
                    Obj_Analyse = (AnalyseClass)datagrid_Analyse.SelectedItem;
                    comboboxType_Analyse.Text = Obj_Analyse.TypeAnalyse;
                    Nom_Analyse.Text = Obj_Analyse.NomAnalyse;
                    EditAnalyse_btn.IsEnabled = true;
                    DeleteAnalyse_btn.IsEnabled = true;
                    AddAnalyse_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //delete analyse
        private void DeleteAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce Ananlyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (AnalyseClass.DeleteAnalyse(Obj_Analyse.IdAnalyse))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs();
                        Annuler_operation_module();
                    }
                    else MessageBox.Show("impossible de supprimer ce Analyse");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler btn
        private void AnnulerAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initialiser_champs();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

       
        // ---------Gestion des  type  des Analyse ----------
      
        // show type analyse
        private void show_Typeanalyse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_TypeAnalyse.SelectedItem != null)
                {
                    Obj_TypeAnalyse = (TypeAnalyse)datagrid_TypeAnalyse.SelectedItem;
                    NomType_Analyse.Text = Obj_TypeAnalyse.Nom_TypeBilan;
                    EditType_Analyse_btn.IsEnabled = true;
                    DeleteType_Analyse_btn.IsEnabled = true;
                    AddType_Analyse_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // add new type analyse
        private void AddType_Analyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NomType_Analyse.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Enregistrer ce Type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        TypeAnalyse TypeAnalyse = new TypeAnalyse(TypeAnalyse.GetLast_Id() + 1, NomType_Analyse.Text);
                        if (TypeAnalyse.AddAnalyse())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_TypeAnalyse();
                            Annuler_operation_module();
                        }
                    }
                }
                else MessageBox.Show("le champs est vide!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // update  type analyse
        private void EditType_Analyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NomType_Analyse.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifier ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        TypeAnalyse TypeAnalyse = new TypeAnalyse(Obj_TypeAnalyse.ID_TypeBilan, NomType_Analyse.Text);
                        if (TypeAnalyse.UpdateAnalyse())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_TypeAnalyse();
                            Annuler_operation_module();
                        }
                    }
                }
                else MessageBox.Show("le champs est vide!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler btn type analyse
        private void AnnulerType_Analyse__btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initialiser_champs_TypeAnalyse();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //delete type anlyse
        private void DeleteType_Analyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (TypeAnalyse.DeleteAnalyse(Obj_TypeAnalyse.ID_TypeBilan))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs_TypeAnalyse();
                        Annuler_operation_module();
                    }
                    else MessageBox.Show("impossible de supprimer ce type d'Analyse");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // ------------- module analyse -------------
      
        // ajouter un nouveau module
        private void btn_add_module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nom_module_analyse = "Intitulé du Modéle";
                // get le nom de module from the window 
                Nom_Module_Window f = new Nom_Module_Window();
                f.ShowDialog();
                nom_module_analyse = f.name_module;
                if (nom_module_analyse != "")
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Enregistrer ce modéle", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        // add the module 
                        Module_Analyse_Class new_module_analyse = new Module_Analyse_Class(0, nom_module_analyse);
                        if (new_module_analyse.Add_Module_Analyse())
                        {
                            new succes().ShowDialog();
                            load_module_analyse();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu N A100!!");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N A101 !!");
            }
        }

        private void Nom_module_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Valide_Nom_Module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //selection un module: afficher son nom
        private void btn_info_modue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Module_analyse.SelectedItem != null)
                {
                    obj_module = (Module_Analyse_Class)datagrid_Module_analyse.SelectedItem;
                    Nom_module_textbox.Text = obj_module.Nom_Module_Analyse;
                    nom_module.Text = obj_module.Nom_Module_Analyse;
                    Border_info_module.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //modifier le nom de module
        private void Edit_moduleAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Valide_Nom_Module())
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifier ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        // edit the module 
                        Module_Analyse_Class new_module_analyse = new Module_Analyse_Class(obj_module.Id_Module_Analyse, Nom_module_textbox.Text);
                        if (new_module_analyse.Update_Module_Analyse())
                        {
                            new succes().ShowDialog();
                            load_module_analyse();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //supprimer le modele
        private void Delete_module_Analyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    // delete the module 
                    if (Module_Analyse_Class.Delete_Module_Analyse(obj_module.Id_Module_Analyse))
                    {
                        new succes().ShowDialog();
                        load_module_analyse();
                        Annuler_operation_module();
                    }
                    else MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // annuler 
        private void Annule_module_rAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Annuler_operation_module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // add analyse dans le module
        private void Add_analyse_module_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (combobox_analyse_module.SelectedItem != null)
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez ajouter ce analyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        int id_analyse = Convert.ToInt32(combobox_analyse_module.SelectedValue);
                        if (!Analyse_deja_exist(id_analyse))
                        {
                            Module_Analyse_Class.Add_Analyse_on_module(obj_module.Id_Module_Analyse, id_analyse);
                            load_analyse_of_module();
                        }
                        else MessageBox.Show("l'Analyse " + combobox_analyse_module.Text + " est deja exist");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // event combobox type analyse
        private void comboboxType_analyse_module_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectAnalyse_ByType();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        //supprimer analyse dans le module
        private void btn_delete_analyse_of_module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_analyse_of_module.SelectedItem != null)
                {
                    AnalyseClass analyse = (AnalyseClass)datagrid_analyse_of_module.SelectedItem;

                    MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce analyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (Module_Analyse_Class.Delete_Analyse_on_module(obj_module.Id_Module_Analyse, analyse.IdAnalyse))
                        {
                            new succes().ShowDialog();
                            load_analyse_of_module();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //selection les analyse de module
        private void btn_analyse_module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Module_analyse.SelectedItem != null)
                {
                    obj_module = (Module_Analyse_Class)datagrid_Module_analyse.SelectedItem;
                    nom_module_analyse.Text = "le modéle " + obj_module.Nom_Module_Analyse;
                    Border_info_module.Visibility = Visibility.Collapsed;
                    border_module_list.Visibility = Visibility.Collapsed;
                    border_analyse_module.Visibility = Visibility.Visible;
                    load_analyse_of_module();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void initiuliser_btn()
        {
            try
            {
                EditAnalyse_btn.IsEnabled = false;
                DeleteAnalyse_btn.IsEnabled = false;
                EditType_Analyse_btn.IsEnabled = false;
                DeleteType_Analyse_btn.IsEnabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Droits_user()
        {
            if (user.IsAdmin == 0)
            {
                DeleteAnalyse_btn.Visibility = Visibility.Collapsed;
                DeleteType_Analyse_btn.Visibility = Visibility.Collapsed;
                Delete_module_Analyse_btn.Visibility = Visibility.Collapsed;
            }
        }


        //---------- Gestion des Analyse ---------
        //load data grid
        private void Load_Analyse()
        {
            try
            {
                ListAnalyses = AnalyseClass.Display_Analyse_Type();
                datagrid_Analyse.ItemsSource = ListAnalyses;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load type Analyse
        private void Load_TypeAnalyse_search()
        {
            try
            {
                ObservableCollection<TypeAnalyse> List_TypeAnalyses;
                List_TypeAnalyses = TypeAnalyse.Display_TypesAnlyse();
                List_TypeAnalyses.Add(new TypeAnalyse { ID_TypeBilan = -1, Nom_TypeBilan = "Tous" });
                comboboxType_Analyse_Search.ItemsSource = List_TypeAnalyses;
                comboboxType_Analyse_Search.SelectedValuePath = "ID_TypeBilan";
                comboboxType_Analyse_Search.DisplayMemberPath = "Nom_TypeBilan";
                comboboxType_Analyse_Search.SelectedIndex = comboboxType_Analyse_Search.Items.Count - 1;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Load_TypeAnalyse()
        {
            try
            {
                comboboxType_Analyse.ItemsSource = TypeAnalyse.Display_TypesAnlyse(); ;
                comboboxType_Analyse.SelectedValuePath = "ID_TypeBilan";
                comboboxType_Analyse.DisplayMemberPath = "Nom_TypeBilan";
                comboboxType_Analyse.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //initialliser
        private void initialiser_champs()
        {
            try
            {
                Nom_Analyse.Text = "";
                EditAnalyse_btn.IsEnabled = false;
                DeleteAnalyse_btn.IsEnabled = false;
                AddAnalyse_btn.IsEnabled = true;
                comboboxType_Analyse.SelectedIndex = 0;
                Load_Analyse();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // ---------Gestion des  type  des Analyse ----------
        private void load_TypeAnalyse_DataGrid()
        {
            try
            {
                datagrid_TypeAnalyse.ItemsSource = TypeAnalyse.Display_TypesAnlyse();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void initialiser_champs_TypeAnalyse()
        {
            try
            {
                NomType_Analyse.Text = "";
                EditType_Analyse_btn.IsEnabled = false;
                DeleteType_Analyse_btn.IsEnabled = false;
                AddType_Analyse_btn.IsEnabled = true;
                load_TypeAnalyse_DataGrid();
                Load_TypeAnalyse();
                Load_TypeAnalyse_search();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // ------------- module analyse -------------

        private void load_module_analyse()
        {
            try
            {
                ObservableCollection<Module_Analyse_Class> list = Module_Analyse_Class.Display_Module_Analyse();
                list.RemoveAt(0); //supprimer la premier item -> voir la fonction pour comprendere
                datagrid_Module_analyse.ItemsSource = list;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private bool Valide_Nom_Module()
        {
            if (Nom_module_textbox.Text.Length == 0)
            {
                Remarque_nom.Text = "le champs est non valide";
                Remarque_nom.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                Remarque_nom.Text = "le champs est valide";
                Remarque_nom.Foreground = Brushes.Green;
                return true;
            }
        }

        //load type Analyse
        private void Load_TypeAnalyse_module()
        {
            try
            {
                comboboxType_analyse_module.ItemsSource = TypeAnalyse.Display_TypesAnlyse();
                comboboxType_analyse_module.SelectedValuePath = "ID_TypeBilan";
                comboboxType_analyse_module.DisplayMemberPath = "Nom_TypeBilan";
                comboboxType_analyse_module.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //filtre Analyse by type analyse
        private void SelectAnalyse_ByType()
        {
            try
            {
                if (comboboxType_Analyse.SelectedValue != null)
                {
                    combobox_analyse_module.ItemsSource = null;
                    int id_type = (int)comboboxType_analyse_module.SelectedValue;
                    combobox_analyse_module.ItemsSource = AnalyseClass.Display_Analyse_ByType(id_type);
                    combobox_analyse_module.SelectedValuePath = "IdAnalyse";
                    combobox_analyse_module.DisplayMemberPath = "NomAnalyse";
                    combobox_analyse_module.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        } 
        private void load_analyse_of_module()
        {
            try 
            {
                ListAnalyses_modules= Module_Analyse_Class.Display_Analyse_of_module(obj_module.Id_Module_Analyse);
                if (ListAnalyses_modules.Count <= 0) 
                {
                    border_image_Aucun_analyse_on_module.Visibility = Visibility.Visible;
                 }
                else 
                    border_image_Aucun_analyse_on_module.Visibility = Visibility.Collapsed;
                datagrid_analyse_of_module.ItemsSource = Module_Analyse_Class.Display_Analyse_of_module(obj_module.Id_Module_Analyse); 
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // annuler
        private void Annuler_analyse_module_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Annuler_operation_module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //
        private void Annuler_operation_module()
        {
            try
            {
                nom_module_analyse.Text = "";
                Nom_module_textbox.Text = "";
                nom_module.Text = "";
                Border_info_module.Visibility = Visibility.Collapsed;
                border_analyse_module.Visibility = Visibility.Collapsed;
                border_module_list.Visibility = Visibility.Visible;
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }    
        private bool Analyse_deja_exist(int id)
        {
            foreach (var item in ListAnalyses_modules)
            {
                if (item.IdAnalyse == id) return true;
            }
            return false;
        }


        //-----------------------  END  METHODES   ------------------------------------ ----
    }
}
