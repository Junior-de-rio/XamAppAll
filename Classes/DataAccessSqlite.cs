using System;
using System.Collections.Generic;
using System.IO;

using Android.Content;

using SQLite;
using Xamarin.Essentials;

namespace MyXamarinApp.Classes
{
    [Table("location")]
    class location
    {    
        
        [PrimaryKey,AutoIncrement]
        public int _id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double accuracy { get; set; }
        public DateTime time { get; set; }

    }

    class accessData
    {
        public static Context mcontext;
        public static SQLiteConnection db;

        public accessData(string dbName,Context context)
        {

            db= new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, dbName));
            db.CreateTable<location>();           
            mcontext = context;

        }
        
        public List<string> displayTableContent()
        {
            
            List<string> list = new List<string>();
            var query = db.Table<location>();
            
            foreach(var result in query)
            {

                var res= $"_id: {result._id}\nLatitude: {result.latitude}\nLongitude: {result.longitude}\nAccuraty: {result.accuracy}\ntime: {result.time}";
                list.Add(res);
                
            }

            return list;
        }

        public static void deleteAll()
        {
            try
            {
                db.DeleteAll<location>();
                if (db.Table<location>().Count() == 0) Messages.ToastMessage(mcontext.GetText(Resource.String.successMsg));

                else Messages.ToastMessage(mcontext.GetText(Resource.String.failedMsg));
            }
            catch(Exception e)
            {
                Messages.DisplayAlert(message:e.Message);
            }
            
        }
        
    }


}