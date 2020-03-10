using Android.Content;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace MyXamarinApp.Classes
{
    [Table("location")]
    public class location
    {    
        
        [PrimaryKey,AutoIncrement]
        public int _id { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double accuracy { get; set; }
        public string personalName { get; set; }
        public DateTime time { get; set; }


        public override string ToString()
        {
            return $"_id: {_id}\nAddress:{address}\nAssociateName:{personalName}\nLatitude: {latitude}\nLongitude: {longitude}\n{accessData.accuracy}: {accuracy} m\nTime: {time}";
        }

    }

    [Table("User")]
     class User
    {   
        [MaxLength(25)]
        public string email { get; set; }
        [MaxLength(10)]
        public string passeword { get; set;}


        
    }

    class accessData
    {
        public static Context mcontext;
        public static SQLiteConnection db;
        public static bool isConnected;
        public static string accuracy;
        public accessData(string dbName,Context context)
        {

            db= new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, dbName));
            accuracy = context.Resources.GetText(Resource.String.accuracy);
            db.CreateTable<location>();           
            mcontext = context;
            isConnected = false;

        }
        
        public List<string> displayTableContent()
        {
            
            List<string> list = new List<string>();
            var query = db.Table<location>();
            
            foreach(var result in query)
            {    
                
                list.Add($"{result}");     
            }

            return list;
        }

        public static void deleteAll()
        {
            try
            {
                
                var res = db.Table<location>();
                if(res.Count()==0) Messages.ToastMessage("Countenu deja supprimer");
                else
                {

                   // db.Query(,"select max(_id) from location", null);
                    db.DeleteAll<location>();

                    if (db.Table<location>().Count() == 0) Messages.ToastMessage(MyTexts.successMsg);

                    else Messages.ToastMessage(MyTexts.failedMsg);
                }
               
            }
            catch(Exception e)
            {
                Messages.DisplayAlert(message:e.Message);
            }
            
        }

        public bool UserInfo(string email="dodji", string passeword="dodji")
        {

            try
            {
                User user = new User()
                {
                    email = email,
                    passeword = passeword,
                };

                db.CreateTable<User>();
                db.Insert(user);
                isConnected = true;
                return true;

            }
            catch (Exception ex)
            {
                Messages.DisplayAlert(message: ex.Message);
                return false;
            }

        }

        public bool isLogedIn(string email,string passeword)
        {
            var logIn=false;
            try
            {
                
                var res = db.Get<User>(1);
                               
                if (res.email == email && res.passeword == passeword) logIn = true;
               
            }
            catch(Exception ex)
            {
                Messages.DisplayAlert(message: ex.Message);
                
            }

            return logIn;
            
            
        }

        public bool IsTableExiste()
        {
            bool exist= false;

            try
            {
                var res = db.Table<location>();
                if (res != null) exist= true;
            }
            catch(SQLiteException ex)
            {
                Messages.DisplayAlert("SQLiteException", ex.Message);
            }
            catch(Exception e)
            {
                Messages.DisplayAlert(message: e.Message);
            }
            return exist;
        }

    }


}