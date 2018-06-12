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
using TostaoApp.Clases;

namespace TostaoApp.ClasesTablas
{
    public class Categoria
    {
        [PrimaryKey, Column("_id")]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
    }
}