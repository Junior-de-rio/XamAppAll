using System;
using System.Linq;
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
        public  async void GetLocation(TextView textview,View myView=null,View progressView=null,Button btn=null)
        {

                


                var request = new GeolocationRequest(GeolocationAccuracy.High, new TimeSpan(0, 0, 5));

                try
                {
                    if (progressView != null && progressView.Visibility != ViewStates.Visible) progressView.Visibility = ViewStates.Visible;
                    //lastLocation = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(true);
                    var location = await Geolocation.GetLocationAsync(request, new System.Threading.CancellationToken(true)).ConfigureAwait(true);
                    
                    if (location != null)
                    {

                        var accuracy = context.Resources.GetText(Resource.String.accuracy);
                        currentLocation = location;
                        if (lastLocation == null) lastLocation = new Location(0,0);

                        var placemarkers = await Geocoding.GetPlacemarksAsync(currentLocation.Latitude, currentLocation.Longitude).ConfigureAwait(true);
                        var placemark = placemarkers?.FirstOrDefault();           
                        address = $"{placemark.Locality},{placemark.SubLocality},{placemark.SubThoroughfare},{placemark.Thoroughfare}";
                        textview.Text = $"Address:{address}\nLatitude:{location.Latitude}\nLongitude:{location.Longitude}\n{accuracy}:{location.Accuracy} m";    
                    }

                    else textview.Text = context.Resources.GetText(Resource.String.failedMsg);
                }
                catch (FeatureNotEnabledException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.featureNotEnabled), ex.Message);
                    
                }
                catch (PermissionException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.permissionException), ex.Message);
                }
                catch (FeatureNotSupportedException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.featureNotSupported), ex.Message);
                }
                catch (Exception ex)
                {
                    Messages.DisplayAlert(ex.Message);
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

    }
}