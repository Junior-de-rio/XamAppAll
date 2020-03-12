using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MyXamarinApp.Classes;
using Android.Content;
using MyXamarinApp.Services;
using MyXamarinApp.Activities;
using Java.Lang;


//using Android.Content.Res;

namespace MyXamarinApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        
       
        XamEssentialFeatures xamEssF;

        Intent serviceToStart;
        Intent intent;
        LocationServiceConnection locationServiceConnection;
        MyTexts allText;
        Button locationMode;
       


        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            xamEssF = new XamEssentialFeatures(this);

            locationMode = FindViewById<Button>(Resource.Id.mlocationMode);

            

            allText = new MyTexts(this);

            locationMode.Click += (s, e) => 
            {

                intent = new Intent(this, typeof(LocationActivity));

                StartActivity(intent);
            };

            

        }
            
    }
 }