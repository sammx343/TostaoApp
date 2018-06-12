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
using TostaoApp.Clases;
using System.Xml;
using Android.Util;
using Redsis.TostaoApp.CrossPlatformLogic;
using TostaoApp.DataHelper;
using TostaoBeta1;

namespace TostaoApp.Actividades
{
    [Activity(Label = "Producto")]
    public class ProductoActividad : Activity
    {
        String tag = Constants.LOG;
        ImageView imagenProducto;
        TextView tituloProducto, cantidadProducto, precioProducto, precioTotalProducto;
        Button botonMas, botonMenos;
        DataBase db;
        string idProduct;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Producto);
            
            // Se recibe el intent desde el RecyclerViewHolder, que es quién cambia de actividad
            idProduct = Intent.GetStringExtra("Producto");
            db = new DataBase();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Producto producto = db.selectQueryTableProducto(idProduct);

            if (producto == null)
            {
                Toast.MakeText(this, "Producto NO encontrado", ToastLength.Long).Show();
                Finish();
            }

            CreateActionBar(producto.Nombre);
            CrearLayout(producto);
        }

// CREACION DEL TOOLBAR ----- *************************************
       /* public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_pedidos, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        */
        //Acciones del Toolbar de Menu
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                /*case Resource.Id.toolbar_carro_de_compra:
                    //Log.Info(tag, "Se accede al carro de compra");
                    StartActivity(typeof(CarroDeCompra));
                    break;*/
                case Android.Resource.Id.Home:
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CreateActionBar(string name)
        {
            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = name;
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }
// CREACION DEL TOOLBAR ----- *************************************

        private void CrearLayout(Producto producto)
        {
            imagenProducto = FindViewById<ImageView>(Resource.Id.imagen_producto);
            tituloProducto = FindViewById<TextView>(Resource.Id.titulo_producto);
            cantidadProducto = FindViewById<TextView>(Resource.Id.cantidadProducto);
            botonMas = FindViewById<Button>(Resource.Id.botonMas);
            botonMenos = FindViewById<Button>(Resource.Id.botonMenos);
            precioProducto = FindViewById<TextView>(Resource.Id.precio_producto);
            precioTotalProducto = FindViewById<TextView>(Resource.Id.precio_total_producto);

            int resID = Resources.GetIdentifier('p' + producto.Imagen, "drawable", Application.Context.PackageName);
            if (resID == 0)
            {
                Console.WriteLine("Entra en la imagen");
                resID = Resources.GetIdentifier("categorias_bebida_preparada", "drawable", Application.Context.PackageName);
            }

            imagenProducto.SetImageResource(resID);
            tituloProducto.Text = producto.Nombre;
            cantidadProducto.Text = producto.Cantidad + "";
            precioProducto.Text = "$" + producto.Precio;
            precioTotalProducto.Text = "$" + (producto.Precio * producto.Cantidad);

            botonMenos.Click += delegate
            {
                if (producto.Cantidad > 0)
                {
                    producto.Cantidad--;
                    db.updateTableProducto(producto.Cantidad, producto.Id);
                    cantidadProducto.Text = producto.Cantidad + "";
                    precioTotalProducto.Text = "$" + (producto.Precio * producto.Cantidad);
                }
            };

            botonMas.Click += delegate 
            {
                producto.Cantidad++;
                db.updateTableProducto(producto.Cantidad, producto.Id);
                cantidadProducto.Text = producto.Cantidad + "";
                precioTotalProducto.Text = "$" + (producto.Precio * producto.Cantidad);
            };
        }
    }
}