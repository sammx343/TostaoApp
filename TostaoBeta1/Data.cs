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

namespace TostaoApp
{
    class Data
    {
        public string productId { get; set; }
        public int imageId { get; set; }
        public string description { get; set; }
        public int cantidadProducto { get; set; }
        public int precioProducto { get; set; }
    }
}