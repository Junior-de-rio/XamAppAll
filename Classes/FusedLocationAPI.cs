using Android.Gms.Location;
using Android.Gms.Common;
using Android.Content;
using System.Threading.Tasks;
using ALocation=Android.Locations.Location;
using Xamarin.Essentials;
using System.Linq;
using System;

namespace MyXamarinApp.Classes
{
    class FusedLocationAPI
    {
        public static FusedLocationProviderClient fusedLocationProviderClient;

        static Context mcontex;

        public FusedLocationAPI(Context context)
        {
            mcontex = context;
           fusedLocationProviderClient = new FusedLocationProviderClient(context);
        }

        public static async Task<ALocation> GetLastLocationFromDevice()
        {
            ALocation location = null;
            Location XamLoc = null;
            try
            {
                if (isGoogleServicesExiste())
                {
                    location = await fusedLocationProviderClient.GetLastLocationAsync().ConfigureAwait(false);

                    //XamLoc=new Location(location.Longitude,location.Latitude)
                }
            }
            catch(Exception ex)
            {
                Messages.DisplayAlert(message: ex.Message);
            }
            
            

            return location;
        }

        public static bool isGoogleServicesExiste()
        {
            var query = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(mcontex);
            if (query == ConnectionResult.Success)
            {
                Messages.ToastMessage("Google play service exist on your device. Your can take benefit from fused location api");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(query))
            {
                var ErrorMsg = GoogleApiAvailability.Instance.GetErrorString(query);

                Messages.DisplayAlert("Google play service exception", $"There is a problem with google play service on your device {query}-{ErrorMsg}");
            }
            return false;
        }

        public async static Task<string> FusedLocationToAddress(Location location=null)
        {
            string address = string.Empty;

            ALocation mLocation= await GetLastLocationFromDevice().ConfigureAwait(false);

            if (location == null) { location = new Location(mLocation.Latitude,mLocation.Longitude); }

            if (location != null)
            {

                var placeMarkers =await Geocoding.GetPlacemarksAsync(location).ConfigureAwait(false);

                var placemark = placeMarkers?.FirstOrDefault();

                address= $"{placemark.Locality},{placemark.SubLocality},{placemark.SubThoroughfare},{placemark.Thoroughfare}";

            }

            return address;
            

        }
    }
}