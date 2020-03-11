using Android.Content;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using MyXamarinApp.Models;

namespace MyXamarinApp.Classes
{


    [Table("User")]
     class User
    {   
        [MaxLength(25)]
        public string email { get; set; }
        [MaxLength(10)]
        public string passeword { get; set;}


        
    }

    class DataAccess
    {
        public static Context mcontext;
        public static SQLiteConnection db;
        public static bool isConnected;
        public static string accuracy;

        public DataAccess(string dbName,Context context)
        {

            db= new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, dbName));
            isConnected = false;

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

      /*  public bool IsTableExiste()
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
        }*/

    }


}