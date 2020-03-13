using Android.Views;
using SQLite;
using System;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using MyXamarinApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyXamarinApp.Classes.Repositories
{
    public class LocationsRepository
    {
        static SQLiteConnection db = null;
        static int lasLocationId;
        LocationsRepository locationsRepository;

        static LocationsRepository()
        {
            
            db = new Lazy<SQLiteConnection>(()=>new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "location.db3"))).Value;
            lasLocationId = 0;
            db.CreateTable<Mlocation>();
            //db = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "location.db3"));   
        }

         public static bool isTableExiste()
         {
            bool tableExiste = false;
            try
            {
                
                if (db.TableMappings.Any(x => x.MappedType == typeof(Mlocation)))
                {
                    tableExiste = true;
                }
                
            }
            catch(Exception ex)
            {
                Messages.DisplayAlert(message: $"isTableExiste:{ex.Message}");              
            }
            return tableExiste;
        }
        public static void SaveLocation(Mlocation mlocation)
        {

            try
            {
               if (!isTableExiste())
                {
                    db.CreateTable<Mlocation>();
                }
               if(mlocation!=null)
                {
                    db.Insert(mlocation);
                    lasLocationId = mlocation._id;
                    Messages.ToastMessage("Inserted successfully");
                }
                else { Messages.ToastMessage($"Something hapened: {mlocation} Not successfully add"); }

                //var con = new SQLiteAsyncConnection("", SQLiteOpenFlags.Create);
            }
            catch (Exception ex)
            {
                Messages.DisplayAlert(message: $"isTableExiste:{ex.Message}");
            }
           
            

        }

        public static Mlocation GetLastLocation()
        {
            Mlocation lastLocation = null;
            try
            {
                if (TableNotEmpty())
                {
                    var query = db.Query<Mlocation>("select * from MLocations order by  _id desc limit 1");
                    lastLocation = query[0];
                }
                
            }
            catch(SQLiteException ex)
            {
                Messages.DisplayAlert(message: $"isTableExiste:{ex.Message}");
            }

            return lastLocation;
 
        }

        public static bool TableNotEmpty()
        {
            if (isTableExiste() && (db.Table<Mlocation>().Count() > 0)) return true;
            else return false;
        }

        public static bool isDiff10(Location current, Location last)
            {

                var is100 = false;
                double distance = -1;
                try
                {
                    distance = Math.Abs(Location.CalculateDistance(current, last, DistanceUnits.Miles));

                    if (distance > 0.01)
                    {
                        is100 = true;
                        Messages.ToastMessage("Greater than 10m");
                    };

                }
                catch (Exception ex)
                {
                    Messages.DisplayAlert("Distance calculation Exception", ex.Message);
                }
                finally
                {
                    Messages.ToastMessage($"Distance:{distance}");
                }

                return is100;
            }


        public static async Task UpdateTable(string locationPersonal="",Android.Locations.Location mlocations=null)
        {
            Mlocation lastLoc;
            Android.Locations.Location currLocation;
            try
            {
                var time = DateTime.Now.ToLocalTime();
                
                currLocation = mlocations;
                Location location2 = new Location(currLocation.Latitude, currLocation.Longitude);
                string address = await XamEssentialFeatures.LocationToAddressAsync(location2).ConfigureAwait(true);
                var myLoc = new Mlocation() { address = XamEssentialFeatures.address, latitude = currLocation.Latitude, longitude = currLocation.Longitude, time = time, accuracy = (double)currLocation.Accuracy, personalName = locationPersonal };
                lastLoc = GetLastLocation();

                if (lastLoc!=null)
                {
                    SaveLocation(myLoc);
                    /* Location location = new Location(lastLoc.latitude, lastLoc.longitude);

                     if (isDiff10(location, location2))
                     {



                     }
                     else
                     {

                         Messages.ToastMessage("Not added:The last location is too near  the current location");

                     }*/
                }
                else
                {
                    myLoc._id = 1;
                    SaveLocation(myLoc);
                }
              
            }
            catch(Exception ex)
            {
                Messages.DisplayAlert(message: ex.Message);
            }
            return;
        }

        public static List<string> DisplayTableContent()
        {
            var query = new List<Mlocation>();
            List<string> list = new List<string>();

            if (isTableExiste())
            {
                query = db.Table<Mlocation>().ToList();
            }

            foreach (var result in query)
            {

                list.Add($"{result}");

            }

            return list;
        }

        public static void DeleteContent()
        {
            try
            {

                var res = db.Table<Mlocation>();
                if (res.Count() == 0) Messages.ToastMessage("Countenu deja supprimer");
                else
                {

                    // db.Query(,"select max(_id) from location", null);
                    db.DeleteAll<Mlocation>();

                    if (db.Table<Mlocation>().Count() == 0) Messages.ToastMessage(MyTexts.successMsg);

                    else Messages.ToastMessage(MyTexts.failedMsg);
                }

            }
            catch (Exception e)
            {
                Messages.DisplayAlert(message: e.Message);
            }

        }
    }
}