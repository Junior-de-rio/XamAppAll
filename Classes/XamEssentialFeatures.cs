using System;
using Android;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
namespace MyXamarinApp.Classes
{
    class XamEssentialFeatures 
    {
        Context context;
        public Location currentLocation;
        public Boolean locationReady;
        public static Messages messages;
        public accessData db;
        public static Activity Activity; 
        public XamEssentialFeatures(Context context)
        {
            this.context = context;
            locationReady = false;
            currentLocation = new Location(0, 0);
            db = new accessData("location.db3",context);
        }
        public  async void GetLocation(TextView textview,View myView=null,View progressView=null,Button btn=null)
        {
            
            
            
           
            
                var alert = new AlertDialog.Builder(context);
                messages = new Messages(this.context);

                alert.SetPositiveButton("OK", (s, e) => { });

                alert.SetTitle("Exception:");


                var request = new GeolocationRequest(GeolocationAccuracy.Medium, new TimeSpan(0, 0, 5));

                try
                {
                    if (progressView != null && progressView.Visibility != ViewStates.Visible) progressView.Visibility = ViewStates.Visible;
                    var location = await Geolocation.GetLocationAsync(request, new System.Threading.CancellationToken(true)).ConfigureAwait(true);
                    if (location != null)
                    {
                        
                        currentLocation = location;
                        textview.Text = $"Latitude:{location.Latitude}\nLongitude:{location.Longitude}\nAccuraty:{location.Accuracy}";
                        accessData.db.CreateTable<location>();
                        var time = DateTime.Now.ToLocalTime();
                        var myLoc = new location() { latitude = location.Latitude, longitude = location.Longitude, time = time, accuracy = (double)location.Accuracy };
                        accessData.db.Insert(myLoc);
                        
                    }

                    else textview.Text = this.context.Resources.GetText(Resource.String.locationMsg);
                }
                catch (FeatureNotEnabledException ex)
                {
                    Messages.DisplayAlert(context.GetText(Resource.String.featureNotEnabled), ex.Message);
                    alert.SetMessage($"{ex.Message}");
                    alert.Show();
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

                    this.locationReady = true;
                    Messages.ToastMessage(this.context.GetText(Resource.String.ready));
                    var time = $"{DateTime.Now.ToLocalTime()}";
                    if (progressView != null) progressView.Visibility = ViewStates.Gone;
                    if (myView != null && myView.Visibility != ViewStates.Visible) myView.Visibility = ViewStates.Visible;
                    if (btn != null) btn.Enabled = true;

                }


        }

        public async void navigateToMap()
        {

            var mapOption = new MapLaunchOptions { Name = "", NavigationMode=NavigationMode.Driving};
            await Map.OpenAsync(this.currentLocation,mapOption).ConfigureAwait(true);

        }


        
    }
}