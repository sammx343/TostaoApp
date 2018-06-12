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
using Android.Support.V7.Widget;
using TostaoApp.Clases;
using TostaoApp.DataHelper;
using TostaoBeta1;

namespace TostaoApp.Actividades
{
    [Activity(Label = "CarroDeCompra")]
    public class CarroDeCompra : Activity
    {
        private RecyclerView recycler;
        private RecyclerViewAdapter adapter;
        private RecyclerView.LayoutManager layoutManager;
        private List<Data> lstData;
        DataBase db;
        string idCategoria;
        public int recycleViewIndex = -1;
        TextView cantidadProductos, cantidadCompra;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CarroDeCompra);

            CreateActionBar();

            db = new DataBase();
            cantidadProductos = FindViewById<TextView>(Resource.Id.cantidadProductos);
            cantidadCompra = FindViewById<TextView>(Resource.Id.totalCompra);
        }

        protected override void OnResume()
        {
            base.OnResume();

            RecyclerViewCreation();
            AddDbToRecyclerView();

            List<Producto> CarroDeCompra = db.selectQueryTableProductoParaCarro();

            cantidadProductos.Text = TotalProductos(CarroDeCompra)[0] + "";

            cantidadCompra.Text = "$" + TotalProductos(CarroDeCompra)[1];

            if (recycleViewIndex != -1)
            {
                layoutManager.ScrollToPosition(recycleViewIndex);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            LinearLayoutManager t = (LinearLayoutManager)layoutManager;
            recycleViewIndex = t.FindFirstCompletelyVisibleItemPosition();
        }

// CREACION DEL TOOLBAR ----- *************************************
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.toolbar_carro_de_compra:
                    //Log.Info(tag, "Se accede al carro de compra");
                    StartActivity(typeof(CarroDeCompra));
                    break;
                case Android.Resource.Id.Home:
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CreateActionBar()
        {
            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Mi Pedido";
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }
        // CREACION DEL TOOLBAR ----- *************************************

        private int[] TotalProductos(List<Producto> CarroDeCompra)
        {
            int cantidadProductos = 0;
            int precioTotal = 0;
            foreach (Producto producto in CarroDeCompra)
            {
                cantidadProductos += producto.Cantidad;
                precioTotal += producto.Cantidad * producto.Precio;
            }
            return new int[] { cantidadProductos, precioTotal };
        }
        
        private void RecyclerViewCreation()
        {
            lstData = new List<Data>();
            recycler = FindViewById<RecyclerView>(Resource.Id.recycler);
            recycler.HasFixedSize = true;
            layoutManager = new LinearLayoutManager(this);
            recycler.SetLayoutManager(layoutManager);
            adapter = new RecyclerViewAdapter(lstData, this);
            recycler.SetAdapter(adapter);
            adapter.callit();
        }

        //Escoge los productos dependiendo de la categoria y los añade al recycler view
        private void AddDbToRecyclerView()
        {
            List<Producto> productos = db.selectQueryTableProductoParaCarro();
            foreach (Producto producto in productos)
            {
                Console.WriteLine(producto.Id + " " + producto.Nombre);
                AddData(producto.Id, producto.Nombre, 'p' + producto.Imagen, producto.Cantidad, producto.Precio);
            }
        }

        private void AddData(string idProducto, string producto, string imagen, int cantidad, int precio)
        {
            int img = Resources.GetIdentifier(imagen, "drawable", Application.Context.PackageName);
            Button n = FindViewById<Button>(Resources.GetIdentifier("plusButton", "layout", Application.Context.PackageName));

            if (img == 0)
            {
                img = Resources.GetIdentifier("categorias_bebida_preparada", "drawable", Application.Context.PackageName);
            }
            lstData.Add(new Data() { productId = idProducto, imageId = img, description = producto, cantidadProducto = cantidad, precioProducto = precio });
        }
    }
}