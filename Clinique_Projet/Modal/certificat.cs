using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
   public class certificat
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cin { get; set; }
        public string apt { get; set; }
        public int arrete { get; set; }
        public string date_debut { get; set; }
        public string date_fin { get; set;}
        public DateTime date_it { get; set;}
        public string type { get; set; }
        public string description { get; set; }
        public string garant { get; set; }
        public string date_it1 { get; set; }
        public certificat() { }
        public certificat(string nom, int arrete, string date_debut, string date_fin, DateTime date_it, string type)
        {
            
            this.arrete = arrete;
            this.date_debut = date_debut;
            this.date_fin = date_fin;
            this.type = type;
            this.name = nom;
            this.date_it = date_it;
           

        }

        public certificat(int id, string nom, int arrete, string date_debut, string date_fin, DateTime date_it, string type)
        {
            this.id = id;
            this.arrete = arrete;
            this.date_debut = date_debut;
            this.date_fin = date_fin;
            this.type = type;
            this.name = nom;
            this.date_it = date_it;


        }

        // constructeur des certificats d'aptitude physique
        public certificat(string nom,DateTime date_it,string cin,string apt,string description)
        {
            this.name = nom;
            this.date_it = date_it;
            this.cin = cin;
            this.apt = apt;
            this.description = description;
            
        }
        public certificat(int id,string nom, DateTime date_it, string cin, string apt, string description)
        {
            this.id = id;
            this.name = nom;
            this.date_it = date_it;
            this.cin = cin;
            this.apt = apt;
            this.description = description;

        }

        //Constructeurs des certificats de mariage 
        public certificat(string nom, string garant, string cin, DateTime date_it, string description)
        {
            this.name = nom;
            this.date_it = date_it;
            this.garant = garant;
            this.cin= cin;
            this.description= description;
        }
        public certificat(int id,string nom, string garant, string cin, DateTime date_it, string description)
        {
            this.id = id;
            this.name = nom;
            this.date_it = date_it;
            this.garant = garant;
            this.cin = cin;
            this.description = description;
        }
       /* public certificat( string nom, string garant, string cin, string date_it, string description)
        {
            this.id = id;
            this.name = nom;
            this.date_it1 = date_it;
            // si garant=="0" alores c'est un certificat de aptitude de mariage h/f
            this.garant = garant;
            this.cin = cin;
            this.description = description;
        }*/

        // ajouter une certificat medical normal
        public  bool add_certificat()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {

                        string requste = "INSERT INTO certificat(nom,date_it,date_debut,date_fin,type,arrete)" +
                            " values(@name,@date,@date_debut,@date_fin,@type,@arrete)";

                        cmd.Connection = con;
                        cmd.CommandText = requste;
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@date", date_it);
                        cmd.Parameters.AddWithValue("@date_debut", date_debut);
                        cmd.Parameters.AddWithValue("@date_fin", date_fin);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@arrete", Convert.ToInt32(arrete));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
       
        // afficher un certificat medical normale
        public static ObservableCollection<certificat> display_certificat()
        {
            ObservableCollection<certificat> p = new ObservableCollection<certificat>();
            using (var con= ConnectDb.GetConnection())
            {
                con.Open();
                using(var cmd = new SqlCommand()) {
                    string requeste = "SELECT * FROM certificat";
                    cmd.Connection = con;
                    cmd.CommandText = requeste;
                   
                    var reader =cmd.ExecuteReader();
                    while (reader.Read())
                    {
                          p.Add(new certificat((int)reader[0], (string)reader[1], (int)reader[6], (string)reader[3],
                             (string)reader[4], (DateTime)reader[2], (string)reader[5]));
                    }
                    reader.Close();
                    con.Close();
                    return p;
                }
            }
        }
      
        
        // Effacer un certificat normal
        public static void Delete_certificat(int id,string type)
        {
            using(var con = ConnectDb.GetConnection())
            {
                con.Open();
                using(var cmd=new SqlCommand())
                {
                    string sql = "DELETE FROM "+type +" WHERE Id=@id";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        // ajouter un certificat d'aptitude physique
        public bool ajouter_apte()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "INSERT INTO apt_physique(date,nom,cin,aptitude,description) VALUES(@date,@nom,@cin,@aptitude,@description)";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@nom", name);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@date", date_it);
                        cmd.Parameters.AddWithValue("@cin", cin);
                        cmd.Parameters.AddWithValue("@aptitude", apt);
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                return true;
            }catch (Exception)
            {
                return false;
            }
        }
        
        // afficher de scertificats d'aptitudes physiques
        public static ObservableCollection<certificat> display()
        {
           ObservableCollection<certificat> c = new ObservableCollection<certificat>();
            using(var con= ConnectDb.GetConnection())
            {
                con.Open();
                using(var cmd= new SqlCommand())
                {
                    string sql;
                  
                        sql = "SELECT * FROM apt_physique";
                    cmd.Connection = con;
                    cmd.CommandText=sql;
                   var reader= cmd.ExecuteReader();
                    while(reader.Read())
                    {
                       c.Add(new certificat((int)reader[0],(string)reader[2], (DateTime)(reader[1]), (string)(reader[3]), 
                           (string)(reader[4]), (string)(reader[5])));
                    }
                    return c;
                }
            }
        }
    
        // ajouter un certificat de mariage
        public bool ajouter_mariage()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "INSERT INTO mariage(date,nom,nom_w,cin,description) VALUES(@date,@nom,@nom_w,@cin,@description)";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@nom", name);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@date", date_it);
                        cmd.Parameters.AddWithValue("@cin", cin);
                        cmd.Parameters.AddWithValue("@nom_w", garant);
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
     
        // affichage
        public static ObservableCollection<certificat> display_certificat_mariage(int A)
        {
            ObservableCollection<certificat> c=new ObservableCollection<certificat>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    string requeste;
                    if (A==0)
                        requeste = "SELECT * FROM mariage WHERE nom_w != '0' ";
                    else
                         requeste = "SELECT * FROM mariage WHERE nom_w='0' ";

                    cmd.Connection = con;
                    cmd.CommandText = requeste;
                 //   cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        c.Add(new certificat((int)reader[0], (string)reader[2],(string)reader[3], (string)reader[4],
                           (DateTime)reader[1],  (string)reader[5]));
                    }
                    reader.Close();
                    con.Close();
                    return c;
                }
            }
        }
        
    }
   
}
