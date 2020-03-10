using Android.Views;
using SQLite;
using System;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
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
            //db = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "location.db3"));   
        }

         public static bool isTableExiste()
        {
            try
            {
                if (db.TableMappings.Any(x => x.MappedType == typeof(location)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                Messages.DisplayAlert(message: ex.Message);
                return false;
            }
            
        }
        public static void SaveLocation(location mlocation)
        {
            try
            {
               if (!isTableExiste())
                {
                    db.CreateTable<location>();
                }
               if(mlocation!=null)
                {
                    db.Insert(mlocation);
                    lasLocationId = mlocation._id;
                    Messages.ToastMessage("Insert successfully");
                }
                else { Messages.ToastMessage($"Something hapened: {mlocation} Not successfully add"); }

                //var con = new SQLiteAsyncConnection("", SQLiteOpenFlags.Create);
            }
            catch (Exception ex)
            {
                Messages.DisplayAlert(message: ex.Message);
            }
           
            

        }

        public static location GetLastLocation()
        {
            location lastLocation = null;
            try
            {
                if (TableNotEmpty())
                {
                    var query = db.Query<location>("select * from location order by  _id desc limit 1");
                    lastLocation = query[0];
                }
                
            }
            catch(SQLiteException ex)
            {
                Messages.DisplayAlert(message: ex.Message);
            }

            return lastLocation;
 
        }

        public static bool TableNotEmpty()
        {
            if (isTableExiste() && (db.Table<location>().Count() != 0)) return true;
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


        public static void UpdateTable(string locationPersonal="")
        {
            location lastLoc;
            Location currLocation;
            try
            {
                var time = DateTime.Now.ToLocalTime();
                currLocation = XamEssentialFeatures.currentLocation;
                var myLoc = new location() { address = XamEssentialFeatures.address, latitude = currLocation.Latitude, longitude = currLocation.Longitude, time = time, accuracy = (double)currLocation.Accuracy, personalName = locationPersonal };
                lastLoc = GetLastLocation();

                if (lastLoc!=null)
                {

                    Location location = new Location(lastLoc.latitude, lastLoc.longitude);
                
                    if (isDiff10(location, currLocation))
                    {

                        SaveLocation(myLoc);
                    }
                    else
                    {

                        Messages.ToastMessage("Not added:The last location is too near  the current location");
                    }
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
   
        }
    }
}