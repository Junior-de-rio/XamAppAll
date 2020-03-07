using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using MyXamarinApp.Classes;
using Android.Content;
using Android;
using Android.Support.V4.Content;
//using Android.Content.Res;

namespace MyXamarinApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        readonly string[] permissions = new string[]
        {
                    Manifest.Permission.AccessCoarseLocation,
                    Manifest.Permission.AccessFineLocation 
        };

        XamEssentialFeatures xamEssF;
        accessData db;
        LinearLayout locationLayout, progessLayout;
        TextView locationDisplayer;
        Button btnGetLocation, mapLauncher, todayLocations;
        Messages messages;
        Intent intent;
        
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            xamEssF = new XamEssentialFeatures(this);

            messages = new Messages(this);

            db = new accessData("location.db3",this);
            
            locationDisplayer = FindViewById<TextView>(Resource.Id.locationDisplayer);

            btnGetLocation = FindViewById<Button>(Resource.Id.getLocation);

            todayLocations = FindViewById<Button>(Resource.Id.todayLocations);
           
            mapLauncher = FindViewById<Button>(Resource.Id.mapLauncher);

            locationLayout = FindViewById<LinearLayout>(Resource.Id.locationLayout);

            progessLayout = FindViewById<LinearLayout>(Resource.Id.progessLayout);

            if (ContextCompat.CheckSelfPermission(this,Manifest.Permission.AccessCoarseLocation) != 0 || ContextCompat.CheckSelfPermission(this,Manifest.Permission.AccessFineLocation) != 0)
            {
                
                    RequestPermissions(new string[]{
                    Manifest.Permission.AccessCoarseLocation,
                    Manifest.Permission.AccessFineLocation}, 0);
            }
            btnGetLocation.Click += (s, e) =>
            {
               
                XamEssentialFeatures.Activity = this;
                btnGetLocation.Enabled = false;
                xamEssF.GetLocation(locationDisplayer,locationLayout,progessLayout, btnGetLocation);
                    
            };
            
            mapLauncher.Click += delegate
            {
                
                xamEssF.navigateToMap();
            };
            todayLocations.Click += delegate
            {
                intent = new Intent(this, typeof(locationActivity));

                StartActivity(intent);
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {

            base.OnDestroy();
            xamEssF = null;
            
           
               
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }


    }
}