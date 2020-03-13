using System;

using Android.Locations;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using XLocation = Xamarin.Essentials.Location;
using MyXamarinApp.Classes;
using Android.Runtime;
using Android.Widget;
using System.Threading.Tasks;
using Android.Support.V7.App;
using MyXamarinApp.Classes.Repositories;

namespace MyXamarinApp.Services
{
    [Service(Name ="com.xamarin.LocationService",Exported =true)]
    [IntentFilter(new string[] {"com.xamarin.LocationService" })]
    class LocationService:Service,ILocationListener
    {
        public IBinder Binder { get; set; }
        public DateTime date;
        public readonly string Tag = typeof(LocationService).FullName;
        static string locationProvider;
        public static LocationManager locationManager;

        public override void OnCreate()
        {
            base.OnCreate();
            
            date = DateTime.UtcNow;
            
            Messages.ToastMessage("Service now created");
        }
        public override IBinder OnBind(Intent intent)
        {
           
            Log.Info(Tag, "OnBind");
            Messages.ToastMessage("Bind now to service");
            Binder = new LocationBinder(this);

            return Binder;  
        }

        public override bool OnUnbind(Intent intent)
        {
            Log.Info(Tag, "OnUnbind");

            Messages.ToastMessage("UnBind now to service");

            return base.OnUnbind(intent);
        }

        public override void OnDestroy()
        {
            Binder = null;
            Log.Info(Tag, "OnDestroy");

            Messages.ToastMessage("Service destroyed");

            base.OnDestroy();
        }

        public string GetFormatedDate()
        {
            var CurrentDate = DateTime.UtcNow.Subtract(date);

            return $"Service started at:{date}({CurrentDate} ago)";
        }

        

        public void OnLocationChanged(Location location)
        {
            
            var address = string.Empty;
            if (location != null)
            {
                XLocation currentLoc = new XLocation(location.Latitude, location.Longitude);
                Activity activity = new Activity();
              Task.Run(async () =>
                {                   
                    address = await XamEssentialFeatures.LocationToAddressAsync(currentLoc).ConfigureAwait(true);
                                       
                    string msg = $"Location has changed\nLat={location.Latitude}\nLong={location.Longitude}\n" +
                    $"Accuracy={location.Accuracy}\nProvider={locationProvider}\nAddress={address}";                                       
                    
                     activity.RunOnUiThread(async () =>
                     {
                         
                         Toast.MakeText(this, msg, ToastLength.Long).Show();
                         await LocationsRepository.UpdateTable(mlocations: location).ConfigureAwait(false);
                     });
                   
                });
              
               
            }
            
           
        }

        public void OnProviderDisabled(string provider)
        {
            
            Toast.MakeText(this, "{provider} HAS BEEN DESABLED", ToastLength.Long).Show();
        }

        public void OnProviderEnabled(string provider)
        {
            
            Toast.MakeText(this,$"{provider} IS ENANBLED", ToastLength.Long).Show();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
            Toast.MakeText(this, $"{provider} STATUS HAS CHANGED", ToastLength.Long).Show();
        }

        public  void StartLocationUpdate()
        {
            Toast.MakeText(this, "In the location Update...", ToastLength.Long).Show();

            Criteria criteria = new Criteria { Accuracy = Accuracy.Medium, PowerRequirement = Power.Medium };

            locationProvider = locationManager.GetBestProvider(criteria, true);

            locationManager.RequestLocationUpdates(locationProvider, 1000, 0.5f, this);
        }

        
    }
}