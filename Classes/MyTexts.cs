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

namespace MyXamarinApp.Classes
{
     class MyTexts
    {
        //public static string app_name { get; }
        public static string ready { get; set; }
        public static string successMsg { get; set; }
        public static string failedMsg { get; set; }
       /* public static string ready { get; }
        public static string ready { get; }
        public static string ready { get; }
        public static string ready { get; }
        public static string ready { get; }
        public static string ready { get; }*/

        public  MyTexts(Context context)
        {
            ready = context.Resources.GetText(Resource.String.ready);
            successMsg = context.Resources.GetText(Resource.String.successMsg);
            failedMsg = context.Resources.GetText(Resource.String.failedMsg);
        }
    }
}