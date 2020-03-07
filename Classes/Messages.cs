using Android.App;
using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace MyXamarinApp.Classes
{
    class Messages
    {
        public static Context mcontext { get; set; }

        public Messages(Context context)
        {
            mcontext = context;
        }

        public static void ToastMessage(string message)
        {
            try
            {
                
                Toast.MakeText(mcontext, message, ToastLength.Short).Show();

            }

            catch(Exception e)
            {
                DisplayAlert(message: e.Message);
            }
           
        }

        public static void DisplayAlert(string title = "Exception", string message = "")
        {
            var alert = new AlertDialog.Builder(mcontext);

            alert.SetTitle(title);

            alert.SetMessage(message);

            alert.Show();
           
        }

        
    }

    
}