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

namespace TostaoApp.Clases
{
    public class Producto
    {
        [PrimaryKey, Column("_id"), Unique]
        public string Id { get; set; }
        public string IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public bool Disponibilidad { get; set; }
    }
}