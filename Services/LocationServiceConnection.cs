using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyXamarinApp.Classes;

namespace MyXamarinApp.Services
{
    class LocationServiceConnection:Java.Lang.Object,IServiceConnection
    {
        public LocationBinder locationBinder { get; private set; }

        public MainActivity activity;

        public bool isConnected { get; private set; }

        string Tag = typeof(LocationServiceConnection).FullName;

        public LocationServiceConnection(MainActivity mactivity)
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



    }
}