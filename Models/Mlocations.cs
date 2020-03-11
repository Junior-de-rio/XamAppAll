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
using SQLite;
using MyXamarinApp.Classes;

namespace MyXamarinApp.Models
{    [Table("MLocations")]
    public class Mlocation
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double accuracy { get; set; }
        public string personalName { get; set; }
        public DateTime time { get; set; }


        public override string ToString()
        {
            return $"_id: {_id}\nAddress:{address}\nAssociateName:{personalName}\nLatitude: {latitude}\nLongitude: {longitude}\n{MyTexts.accuracy}: {accuracy} m\nTime: {time}";
        }
    }
}