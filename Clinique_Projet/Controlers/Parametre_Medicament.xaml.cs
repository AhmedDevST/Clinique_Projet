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
    /// Logique d'interaction pour Parametre_Medicament.xaml
    /// </summary>
    public partial class Parametre_Medicament : UserControl
    {
        ObservableCollection<Medicament_Class> List_Medicaments;
        ObservableCollection<Medicament_Class> List_module_Medicaments;
        public Medicament_Class ObjMedicament;
        public Categorie_Medicament Obj_CatMedicament;
        public Utilisateur_Class user;
        public Module_Ordonnonce_Class obj_module;
        public Parametre_Medicament(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Load_Medicament();
                Load_TypeMedicament();
                Load_TypeMedicament_search();    
                load_CatMedicament_DataGrid();
                initiliser_btn();
                loadCategorie_Medicament();
                load_module_medicament();
                this.user = user;
                 Droits_user();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

         // --------------- Gestion  medicament *-----------------

        //search by  combobox
        private void comboboxType_medicaments_Search_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                Load_Medicament();
                if (comboboxType_medicaments_Search.SelectedItem != null)
                {
                    string searchTerm = comboboxType_medicaments_Search.Text;//get text from combobox
                    if ((!string.IsNullOrEmpty(searchTerm)) && (searchTerm != "Tous")) // si le textbox n'est pas vide
                    {
                        ObservableCollection<Medicament_Class> filteredData = new ObservableCollection<Medicament_Class>(
                         List_Medicaments.Where(item => item.CatMedcament.Contains(searchTerm))
                            );
                        List_Medicaments = filteredData;
                        datagrid_medicaments.ItemsSource = filteredData;
                    }
                    else Load_Medicament();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //search by nom medicament
        private void Text_Search_medicaments_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchTerm = Text_Search_medicaments.Text.ToLower();   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = List_Medicaments.Where(item =>
                  item.NomMedcament.ToLower().Contains(searchTerm)
               );
                    datagrid_medicaments.ItemsSource = filteredData;  //affecter le result a datagrid
                }
                else datagrid_medicaments.ItemsSource = List_Medicaments; //si le textbox est vide on loaddatagrid

            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //add medicaemnent
        private void AddMedicaments_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(Nom_medicaments.Text)) && (comboboxType_medicaments.SelectedItem != null))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Ajouter ce medicament", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {

                        Medicament_Class medicament = new Medicament_Class(Medicament_Class.GetLast_Id() + 1, Nom_medicaments.Text, comboboxType_medicaments.SelectedValue.ToString());
                        if (medicament.AddMedicament())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs();
                            annuler_operation_module();
                        }
                    }
                }
                else MessageBox.Show("les champs sont  vides!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // edit medicament
        private void EditMedicaments_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(Nom_medicaments.Text)) && (comboboxType_medicaments.SelectedItem != null))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifier ce medicament", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        Medicament_Class medicament = new Medicament_Class(ObjMedicament.IdMedcament, Nom_medicaments.Text, comboboxType_medicaments.SelectedValue.ToString());
                        if (medicament.UpdateMedicament())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs();
                            annuler_operation_module();
                        }
                    }
                }
                else MessageBox.Show("les champs sont vides!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //delete medicament
        private void DeleteMedicaments_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce medicament", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (Medicament_Class.DeleteMedicament(ObjMedicament.IdMedcament))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs();
                        annuler_operation_module();
                    }
                    else MessageBox.Show("impossible de supprimer ce medicament");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler
        private void AnnulerMdicaments_btn_Click(object sender, RoutedEventArgs e)
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

        //show info medicaments
        private void show_medicaments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_medicaments.SelectedItem != null)
                {
                    ObjMedicament = (Medicament_Class)datagrid_medicaments.SelectedItem;
                    comboboxType_medicaments.Text = ObjMedicament.CatMedcament;
                    Nom_medicaments.Text = ObjMedicament.NomMedcament;
                    EditMedicaments_btn.IsEnabled = true;
                    DeleteMedicaments_btn.IsEnabled = true;
                    AddMedicaments_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //----------------- Gestion de type medicament ------------

        //show info cat medicament
        private void show_Typemedicaments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_TypeMedicaments.SelectedItem != null)
                {
                    Obj_CatMedicament = (Categorie_Medicament)datagrid_TypeMedicaments.SelectedItem;
                    NomCat_medicament.Text = Obj_CatMedicament.Nom_CatMedicament;
                    EditType_medicaments_btn.IsEnabled = true;
                    DeleteType_medicaments_btn.IsEnabled = true;
                    AddType_medicaments_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //add cat medicament
        private void AddType_medicaments_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NomCat_medicament.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voullez Ajouter ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        Categorie_Medicament categorie_Medicament = new Categorie_Medicament(Categorie_Medicament.GetLast_Id() + 1, NomCat_medicament.Text);
                        if (categorie_Medicament.AddCat_Medicament())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_CatMedicament();
                            annuler_operation_module();
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

        //edit cat medicaments
        private void EditType_medicaments_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NomCat_medicament.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voullez modifier ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        Categorie_Medicament categorie_Medicament = new Categorie_Medicament(Obj_CatMedicament.ID_CatMedicament, NomCat_medicament.Text);
                        if (categorie_Medicament.UpdateCat_Medicament())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_CatMedicament();
                            annuler_operation_module();
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

        //delete
        private void DeleteType_medicaments_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voullez Supprimer ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (Categorie_Medicament.DeleteCat_medicament(Obj_CatMedicament.ID_CatMedicament))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs_CatMedicament();
                        annuler_operation_module();
                    }
                    else MessageBox.Show("impossible de supprimer ce type de medicament");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //annuler
        private void AnnulerType_medicaments__btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initialiser_champs_CatMedicament();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------les modules de medicaments ----------
        
        //nouveau module
        private void btn_add_module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nom_module_medicamennt = "Nom Module";
                // get le nom de module from the window 
                Nom_Module_Window f = new Nom_Module_Window();
                f.ShowDialog();
                nom_module_medicamennt = f.name_module;
                if (nom_module_medicamennt != "")
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Enregistrer ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        // add the module 
                        Module_Ordonnonce_Class new_module = new Module_Ordonnonce_Class(0, nom_module_medicamennt);
                        if (new_module.Add_Module_Ordonnonce())
                        {
                            new succes().ShowDialog();
                            load_module_medicament();
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

        // afficher info de module
        private void btn_info_modue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Module_medicament.SelectedItem != null)
                {
                    obj_module = (Module_Ordonnonce_Class)datagrid_Module_medicament.SelectedItem;
                    Nom_module_textbox.Text = obj_module.Nom_Module_Ordonnonce;
                    nom_module.Text = obj_module.Nom_Module_Ordonnonce;
                    Border_info_module.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
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
      
        //supprimer le module
        private void Delete_module_medicament_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    // delete the module 
                    if (Module_Ordonnonce_Class.Delete_Module_Ordonnonce(obj_module.Id_Module_Ordonnonce))
                    {
                        new succes().ShowDialog();
                        load_module_medicament();
                        annuler_operation_module();
                    }
                    else MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //edit le nom de module
        private void Edit_module_medicament_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Valide_Nom_Module())
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifier ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        // edit the module 
                        Module_Ordonnonce_Class module = new Module_Ordonnonce_Class(obj_module.Id_Module_Ordonnonce, Nom_module_textbox.Text);
                        if (module.Update_Module_Ordonnonce())
                        {
                            new succes().ShowDialog();
                            load_module_medicament();
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

        //annuler
        private void Annuler_medicament_module_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                annuler_operation_module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        //supprimer medicament de module
        private void btn_delete_medicament_of_module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_medicament_of_module.SelectedItem != null)
                {
                    Medicament_Class medicament = (Medicament_Class)datagrid_medicament_of_module.SelectedItem;

                    MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce medicament", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (Module_Ordonnonce_Class.Delete_Medicament_on_module(obj_module.Id_Module_Ordonnonce, medicament.IdMedcament))
                        {
                            new succes().ShowDialog();
                            load_medicament_of_module();
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

        private void Annule_module_medicament_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                annuler_operation_module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
      
        // ajouter medicament dans le module
        private void Add_medicament_module_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (combobox_medicament_module.SelectedItem != null)
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez ajouter ce medicament", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        int id_medicament = Convert.ToInt32(combobox_medicament_module.SelectedValue);
                        if (!Medicament_deja_exist(id_medicament))
                        {
                            Module_Ordonnonce_Class.Add_Medicament_on_module(obj_module.Id_Module_Ordonnonce, id_medicament);
                            load_medicament_of_module();
                        }
                        else MessageBox.Show("le medicament " + combobox_medicament_module.Text + " est deja exist");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //filtre medicament by categorie de medicament
        private void SelectMedicament_ByCat()
        {
            try
            {
                if (comboboxType_medicament_module.SelectedValue != null)
                {
                    combobox_medicament_module.ItemsSource = null;
                    int id_Cat = (int)comboboxType_medicament_module.SelectedValue;
                    combobox_medicament_module.ItemsSource = Medicament_Class.Display_Medicament_ByCat(id_Cat);
                    combobox_medicament_module.SelectedValuePath = "IdMedcament";
                    combobox_medicament_module.DisplayMemberPath = "NomMedcament";
                    combobox_medicament_module.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        //selection les medicament d un module
        private void btn_medicament_module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Module_medicament.SelectedItem != null)
                {
                    obj_module = (Module_Ordonnonce_Class)datagrid_Module_medicament.SelectedItem;
                    nom_module_medicament.Text = "le module " + obj_module.Nom_Module_Ordonnonce;
                    Border_info_module.Visibility = Visibility.Collapsed;
                    border_module_list.Visibility = Visibility.Collapsed;
                    border_medicament_module.Visibility = Visibility.Visible;
                    load_medicament_of_module();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void comboboxType_medicament_module_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectMedicament_ByCat();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------



        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void initiliser_btn()
        {
            try
            {
                EditMedicaments_btn.IsEnabled = false;
                DeleteMedicaments_btn.IsEnabled = false;
                EditType_medicaments_btn.IsEnabled = false;
                DeleteType_medicaments_btn.IsEnabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Droits_user()
        {
            try
            {
                if (user.IsAdmin == 0)
                {
                    DeleteMedicaments_btn.Visibility = Visibility.Collapsed;
                    DeleteType_medicaments_btn.Visibility = Visibility.Collapsed;
                    Delete_module_medicament_btn.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // --------------- Gestion  medicament *-----------------

        //load data grid
        private void Load_Medicament()
        {
            try
            {
                List_Medicaments = Medicament_Class.Display_Medicament();
                datagrid_medicaments.ItemsSource = List_Medicaments;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load type medicament
        private void Load_TypeMedicament_search()
        {
            try
            {
                ObservableCollection<Categorie_Medicament> List_CatMedicament;
                List_CatMedicament = Categorie_Medicament.Display_CatMedicaments();
                List_CatMedicament.Add(new Categorie_Medicament { ID_CatMedicament = -1, Nom_CatMedicament = "Tous" });
                comboboxType_medicaments_Search.ItemsSource = List_CatMedicament;
                comboboxType_medicaments_Search.SelectedValuePath = "ID_CatMedicament";
                comboboxType_medicaments_Search.DisplayMemberPath = "Nom_CatMedicament";
                comboboxType_medicaments_Search.SelectedIndex = comboboxType_medicaments_Search.Items.Count - 1;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Load_TypeMedicament()
        {
            try
            {
                comboboxType_medicaments.ItemsSource = Categorie_Medicament.Display_CatMedicaments();
                comboboxType_medicaments.SelectedValuePath = "ID_CatMedicament";
                comboboxType_medicaments.DisplayMemberPath = "Nom_CatMedicament";
                comboboxType_medicaments.SelectedIndex = 0;
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
                Nom_medicaments.Text = "";
                EditMedicaments_btn.IsEnabled = false;
                DeleteMedicaments_btn.IsEnabled = false;
                AddMedicaments_btn.IsEnabled = true;
                comboboxType_medicaments.SelectedIndex = 0;
                Load_Medicament();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        //----------------- Gestion de type medicament ------------

        private void load_CatMedicament_DataGrid()
        {
            try
            {
                datagrid_TypeMedicaments.ItemsSource = Categorie_Medicament.Display_CatMedicaments();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void initialiser_champs_CatMedicament()
        {
            try
            {
                NomCat_medicament.Text = "";
                EditType_medicaments_btn.IsEnabled = false;
                DeleteType_medicaments_btn.IsEnabled = false;
                AddType_medicaments_btn.IsEnabled = true;
                load_CatMedicament_DataGrid();
                Load_TypeMedicament_search();
                Load_TypeMedicament();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------les modules de medicaments ----------

        //load les modules de medicaments
        private void load_module_medicament()
        {
            try
            {
                ObservableCollection<Module_Ordonnonce_Class> list = Module_Ordonnonce_Class.Display_Module_Ordonnonce();
                list.RemoveAt(0); //supprimer la premier item -> voir la fonction pour comprendere
                datagrid_Module_medicament.ItemsSource = list;
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

        private void annuler_operation_module()
        {
            try
            {
                Nom_module_textbox.Text = "";
                nom_module_medicament.Text = "";
                Border_info_module.Visibility = Visibility.Collapsed;
                border_medicament_module.Visibility = Visibility.Collapsed;
                border_module_list.Visibility = Visibility.Visible;
                nom_module.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load_type_medicament
        private void loadCategorie_Medicament()
        {
            try
            {
                comboboxType_medicament_module.ItemsSource = Categorie_Medicament.Display_CatMedicaments();
                comboboxType_medicament_module.SelectedValuePath = "ID_CatMedicament";
                comboboxType_medicament_module.DisplayMemberPath = "Nom_CatMedicament";
                comboboxType_medicament_module.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }   
        private void load_medicament_of_module()
        {
            try
            {
                List_module_Medicaments = Module_Ordonnonce_Class.Display_Medicament_of_module(obj_module.Id_Module_Ordonnonce);
                if(List_module_Medicaments.Count > 0)
                {
                    border_image_Aucun_medicament_on_module.Visibility = Visibility.Collapsed;
                }
                else border_image_Aucun_medicament_on_module.Visibility = Visibility.Visible;
                datagrid_medicament_of_module.ItemsSource = Module_Ordonnonce_Class.Display_Medicament_of_module(obj_module.Id_Module_Ordonnonce);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private bool Medicament_deja_exist(int id)
        {
            foreach (var item in List_module_Medicaments)
            {
                if (item.IdMedcament == id) return true;
            }
            return false;
        }
      
        //-----------------------  END  METHODES    ------------------------------------ ----

    }
}
