using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyXamarinApp.Classes;
using Xamarin.Essentials;

namespace MyXamarinApp.Services
{
    class LocationServiceConnection:Java.Lang.Object,IServiceConnection
    {
        public LocationBinder locationBinder { get; private set; }

        public Activity activity;

        public bool isConnected { get; private set; }

        string Tag = typeof(LocationServiceConnection).FullName;

        public LocationServiceConnection(Activity mactivity)
        {
            activity = mactivity;
            isConnected = false;
            locationBinder = null;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Messages.ToastMessage($"Connection...\t client:{name.ClassName}");

            locationBinder =service as LocationBinder;

            isConnected = locationBinder != null;

            if (isConnected)
            {
                Messages.ToastMessage("Conected successfully");
                
                locationBinder.locationService.StartLocationUpdate();
            }
            else
            {
                
                Messages.ToastMessage("Can't connect");
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            isConnected = false;
            locationBinder = null;
            Messages.ToastMessage("Disconnection!!");
        }

        static async Task<string> GetAddressAsync(Xamarin.Essentials.Location location)
        {
            var address = string.Empty;
           
           
            var placemarks = await Geocoding.GetPlacemarksAsync(location).ConfigureAwait(true);
            if (placemarks != null)
            {
                var placemark = placemarks?.FirstOrDefault();

                address = $"{placemark.CountryName},{placemark.FeatureName},{placemark.SubAdminArea},{placemark.SubThoroughfare},{placemark.Thoroughfare}";
            }
            return address;
        }

    }
}