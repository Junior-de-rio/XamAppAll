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

namespace MyXamarinApp.Services
{
    class LocationBinder:Binder
    {
        public LocationService locationService { get;private set; }

        public LocationBinder(LocationService mlocationService)
        {
            locationService =mlocationService;
        }

        public string GetFormatedDate()
        {
            return locationService.GetFormatedDate();
        }
    }
}