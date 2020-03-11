
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using MyXamarinApp.Classes;
using Android.Content;
using Android;
using Android.Support.V4.Content;
using Android.Views;
using AlertDialog = Android.App.AlertDialog;
using Android.Util;
using MyXamarinApp.Services;
using MyXamarinApp.Classes.Repositories;
using Android.Content.PM;

//using Android.Content.Res;

namespace MyXamarinApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class LocationActivity : Activity
    {

        bool isConnected;

        LayoutInflater inflater;

        AlertDialog dialog;
        bool permissionIsEnabled = false;
        XamEssentialFeatures xamEssF;
        DataAccess db;
        LinearLayout locationLayout, progessLayout;
        TextView locationDisplayer, formatedTime;
        Button btnGetLocation, mapLauncher, todayLocations, logIn, saveLocation;
        EditText email, passeword, locationPersonalName;
        Messages messages;
        Intent serviceToStart;
        Intent intent;
        LocationServiceConnection locationServiceConnection;
        MyTexts allText;

        View auth;


        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Location_Activity);

            xamEssF = new XamEssentialFeatures(this);

            messages = new Messages(this);

            inflater = LayoutInflater.From(this);

            auth = inflater.Inflate(Resource.Layout.Authentication, null);

            logIn = auth.FindViewById<Button>(Resource.Id.logIn);

            Messages.ToastMessage(message: $"{logIn.Text}");

            allText = new MyTexts(this);

            email = auth.FindViewById<EditText>(Resource.Id.email);

            locationPersonalName = FindViewById<EditText>(Resource.Id.locationPersonalName);

            passeword = auth.FindViewById<EditText>(Resource.Id.passeword);

            dialog = new AlertDialog.Builder(this).Create();

            dialog.SetView(auth);

            db = new DataAccess("location.db3", this);

            locationPersonalName.TextChanged += delegate
            {
                if (string.IsNullOrEmpty(locationPersonalName.Text)) { locationPersonalName.Background = GetDrawable(Resource.Drawable.EditTextBackground); }
                else
                {
                    locationPersonalName.Background = GetDrawable(Resource.Drawable.TextEditing);
                }

            };


            locationDisplayer = FindViewById<TextView>(Resource.Id.locationDisplayer);

            formatedTime = FindViewById<TextView>(Resource.Id.formatedTime);

            btnGetLocation = FindViewById<Button>(Resource.Id.getLocation);

            todayLocations = FindViewById<Button>(Resource.Id.todayLocations);

            mapLauncher = FindViewById<Button>(Resource.Id.mapLauncher);

            saveLocation = FindViewById<Button>(Resource.Id.saveLocation);

            locationLayout = FindViewById<LinearLayout>(Resource.Id.locationLayout);

            progessLayout = FindViewById<LinearLayout>(Resource.Id.progessLayout);

            
            //Messages.ToastMessage($"{accessData.isConnected}");

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != 0 || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != 0)
            {

                RequestPermissions(new string[]{
                    Manifest.Permission.AccessCoarseLocation,
                    Manifest.Permission.AccessFineLocation}, 2);
            }

            saveLocation.Click += (s, e) =>
            {

                LocationsRepository.UpdateTable(locationPersonalName.Text);


            };

            btnGetLocation.Click += (s, e) =>
            {

                btnGetLocation.Enabled = false;
                xamEssF.GetLocation(locationDisplayer, locationLayout, progessLayout, btnGetLocation);

            };

            mapLauncher.Click += delegate
            {

                xamEssF.navigateToMap();
            };
            todayLocations.Click += delegate
            {
                if (LocationsRepository.GetLastLocation() != null)
                {
                    Messages.ToastMessage($"Last location id={LocationsRepository.GetLastLocation()._id}");
                }
                else
                {
                    Messages.ToastMessage("No location in the location table");
                }

                intent = new Intent(this, typeof(locationActivity));

                StartActivity(intent);

            };


            passeword.TextChanged += delegate { passeword.Background = GetDrawable(Resource.Drawable.TextEditing); };
            email.TextChanged += delegate { email.Background = GetDrawable(Resource.Drawable.TextEditing); };

            logIn.Click += (s, e) => {

                if (db.isLogedIn(email.Text, passeword.Text))
                {

                    dialog.Dismiss();
                    Messages.ToastMessage("LogIn successfully");

                }

                else
                {

                    email.Background = GetDrawable(Resource.Drawable.ErrorBackground);
                    passeword.Background = GetDrawable(Resource.Drawable.ErrorBackground);
                    Log.Info("info", "LogIn Error");
                    Messages.ToastMessage("Email or passeword incorrect");
                };

            };




        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 2)
            {
                if (grantResults != null && grantResults.Length == 2)
                {
                    if (grantResults[0] != (int)Permission.Granted && grantResults[1] != (int)Permission.Granted)
                    {
                        btnGetLocation.Enabled = false;
                    }
                }

            }
            if (grantResults != null) Messages.ToastMessage($"resuestCode={requestCode} Length={grantResults.Length} Grant={grantResults[0]}--{grantResults[1]}");
            else Messages.ToastMessage($"Reult is null");

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (locationServiceConnection == null) locationServiceConnection = new LocationServiceConnection(this);
            // BindToService();
        }

        protected override void OnResume()
        {
            base.OnResume();

            /* Messages.ToastMessage($"isConnected:{locationServiceConnection.isConnected}");
             if (locationServiceConnection.isConnected) Messages.ToastMessage("Connected");
             */
            //else Messages.ToastMessage("Service not Connected");

        }

        protected override void OnDestroy()
        {

            base.OnDestroy();
            Messages.ToastMessage("Activity destroyed");
            xamEssF = null;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {

            base.OnSaveInstanceState(outState);

            //outState.PutBoolean("isLogedIn", isLogedIn);   
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