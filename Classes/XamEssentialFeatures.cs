using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace MyXamarinApp.Classes
{
    class XamEssentialFeatures 
    {
        Context context;
        public static Location currentLocation;
        public Boolean locationReady;
        public static Location lastLocation;
        public static string address;

        public XamEssentialFeatures(Context context)
        {
            this.context = context;
            locationReady = false;
            currentLocation = new Location(0, 0);
            address = string.Empty;
        }
        public  async Task GetLocation(TextView textview,View myView=null,View progressView=null,Button btn=null)
        {


                FusedLocationAPI f = new FusedLocationAPI(context);

                var request = new GeolocationRequest(GeolocationAccuracy.High, new TimeSpan(0, 0, 10));
            if (progressView != null && progressView.Visibility != ViewStates.Visible) progressView.Visibility = ViewStates.Visible;
            try
                {
                   
                    //lastLocation = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(false);
                    var location = await Geolocation.GetLocationAsync(request, new System.Threading.CancellationToken(true)).ConfigureAwait(false);
                    //Location FusedLoc = null;
                    var FusedLoc =await FusedLocationAPI.GetLastLocationFromDevice().ConfigureAwait(true);
                    string fusedLocToString = string.Empty;
                    string fusedLocAddress = string.Empty;
              if (FusedLoc != null)
                {
                    fusedLocToString = $"Latitude:{FusedLoc.Latitude}\nLongitude:{FusedLoc.Longitude}\nAccuracy:{FusedLoc.Accuracy}";
                    fusedLocAddress = await FusedLocationAPI.FusedLocationToAddress().ConfigureAwait(true);
                }
                if (location != null)
                {

                    var accuracy = context.Resources.GetText(Resource.String.accuracy);
                    currentLocation = location;
                    if (lastLocation == null) lastLocation = new Location(0, 0);

                    var placemarkers = await Geocoding.GetPlacemarksAsync(currentLocation.Latitude, currentLocation.Longitude).ConfigureAwait(true);
                    var placemark = placemarkers?.FirstOrDefault();
                    address = $"{placemark.Locality},{placemark.SubLocality},{placemark.SubThoroughfare},{placemark.Thoroughfare}";
                    textview.Text = $"Address:{address}\nLatitude:{location.Latitude}\nLongitude:{location.Longitude}\n" +
                    $"{accuracy}:{location.Accuracy} m" + $"\n--FusedLocation--\n {fusedLocToString}\nAddress:{fusedLocAddress}";
                    return;
                }

                else { 
                    textview.Text = context.Resources.GetText(Resource.String.failedMsg);
                    return;
                }
                
                }
                catch (FeatureNotEnabledException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.featureNotEnabled), ex.Message);
                return;

                 }
                catch (PermissionException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.permissionException), ex.Message);
                }
                catch (FeatureNotSupportedException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.featureNotSupported), ex.Message);
                return;
                 }
                catch (Exception ex)
                {
                    Messages.DisplayAlert(ex.Message);
                return;
                }
                finally
                {

                    locationReady = true;
                    Messages.ToastMessage(MyTexts.ready); 
                    if (progressView != null) progressView.Visibility = ViewStates.Gone;
                    if (myView != null && myView.Visibility != ViewStates.Visible) myView.Visibility = ViewStates.Visible;
                    if (btn != null) btn.Enabled = true;
                
                }
            
           

        }

        public async void navigateToMap()
        {

            var mapOption = new MapLaunchOptions { Name = "", NavigationMode=NavigationMode.Driving};
            await Map.OpenAsync(currentLocation,mapOption).ConfigureAwait(true);

        }

        public static async Task<string> LocationToAddress(Location location)
        {
            address = string.Empty;
            try
            {
                var placemarkers = await Geocoding.GetPlacemarksAsync(location).ConfigureAwait(true);
                if (placemarkers != null) 
                {
                var placemark = placemarkers?.FirstOrDefault();
                address = $"{placemark.Locality},{placemark.SubLocality},{placemark.SubThoroughfare},{placemark.Thoroughfare}";
                }
               
            }
            catch(Exception ex)
            {
                Messages.DisplayAlert("Geocoding Exception", ex.Message);
            }
            return address;
        }

    }
}