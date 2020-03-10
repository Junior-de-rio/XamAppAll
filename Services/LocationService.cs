using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Security;
using MyXamarinApp.Classes;
using static Java.Util.Jar.Attributes;

namespace MyXamarinApp.Services
{
    [Service(Name ="com.xamarin.LocationService",Exported =true)]
    [IntentFilter(new string[] {"com.xamarin.LocationService" })]
    class LocationService:Service
    {
        public IBinder Binder { get; set; }
        public DateTime date;
        public readonly string Tag = typeof(LocationService).FullName;


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
    }
}