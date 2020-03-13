using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System;


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
                Activity activity = new Activity();
                var toast = Toast.MakeText(mcontext, message, ToastLength.Short);
                activity.RunOnUiThread(() =>
                {
                   
                    toast.Show();
                });
               
            }

            catch(Exception e)
            {
                DisplayAlert(message: e.Message);
            }
           
        }

        public static void DisplayAlert(string title = "Exception", string message = "",ViewStates viewStates=ViewStates.Invisible)
        {
            var inflater = LayoutInflater.From(mcontext);
            var dialog = new AlertDialog.Builder(mcontext).Create();
            var view = inflater.Inflate(Resource.Layout.AlertInterface,null);

            var _title = view.FindViewById<TextView>(Resource.Id._alertTitle);
            var _message = view.FindViewById<TextView>(Resource.Id._alertMessage);
            var _okBtn = view.FindViewById<Button>(Resource.Id._okBtn);
            var _cancleBtn = view.FindViewById<Button>(Resource.Id._cancelBtn);

            _cancleBtn.Visibility = viewStates;

            _okBtn.Click += delegate { dialog.Dismiss(); };

            _cancleBtn.Click+= delegate { dialog.Dismiss(); };

            _title.Text = title;
            _message.Text = message;

            dialog.SetView(view);

            dialog.SetIcon(Resource.Drawable.warning_icon);
            Activity activity = new Activity();
            activity.RunOnUiThread(() =>
            {
                dialog.Show();
            });
           
 
        }

        
    }

    
}