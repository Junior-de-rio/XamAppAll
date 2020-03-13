using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MyXamarinApp.Classes;
using Android.Content;
using MyXamarinApp.Services;
using MyXamarinApp.Activities;
using Java.Lang;
using Android.Support.V4.Content;
using Android;


//using Android.Content.Res;

namespace MyXamarinApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        
       
        XamEssentialFeatures xamEssF;
        bool isConnected;
        Intent serviceToStart;
        Intent intent;
        LocationServiceConnection locationServiceConnection;
        MyTexts allText;
        Button locationMode, serviceBinderBtn;
       


        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            xamEssF = new XamEssentialFeatures(this);

            locationMode = FindViewById<Button>(Resource.Id.mlocationMode);

            serviceBinderBtn = FindViewById<Button>(Resource.Id.serviceBinderBtn);

            allText = new MyTexts(this);
            locationServiceConnection = new LocationServiceConnection(this);
            //locationServiceConnection.locationBinder.locationService.man
            locationMode.Click += (s, e) => 
            {
                
                intent = new Intent(this, typeof(LocationActivity));

                StartActivity(intent);
            };
            serviceBinderBtn.Click += delegate
            {
                if (locationServiceConnection.isConnected)
                {
                    Toast.MakeText(this, "Connected successfully", ToastLength.Short).Show();
                }
                if (locationServiceConnection != null)
                {
                    UnbindService(locationServiceConnection);
                    Toast.MakeText(this, "Unbund successfully", ToastLength.Short).Show();
                }
            };

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != 0 || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != 0)
            {

                RequestPermissions(new string[]{
                    Manifest.Permission.AccessCoarseLocation,
                    Manifest.Permission.AccessFineLocation}, 2);
            }

        }
        protected override void OnStart()
        {
            base.OnStart();
            
           
            //BindToService();
        }
        protected override void OnResume()
        {
            base.OnResume();
         
        }
        public void BindToService()
        {
            if (isConnected) Messages.ToastMessage("Already connected");
            else
            {

                serviceToStart = new Intent(this, typeof(LocationService));

                isConnected = BindService(serviceToStart, locationServiceConnection, Bind.AutoCreate);
            }
        }

        public void UnBindToService()
        {
            UnbindService(locationServiceConnection);
        }
    }
 }