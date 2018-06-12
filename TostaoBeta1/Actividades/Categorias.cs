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
using System.Xml;
using TostaoApp.Clases;
using Android.Util;
using Redsis.TostaoApp.CrossPlatformLogic;
using Newtonsoft.Json;
using TostaoApp.DataHelper;
using TostaoApp.ClasesTablas;
using TostaoBeta1;

namespace TostaoApp.Actividades
{
    [Activity(Label = "Categorias")]
    public class Categorias : Activity
    {
        DataBase db;
        String tag = Constants.LOG;
        List<Categoria> listaCategorias;
        TextView cantidadProductos, cantidadCompra;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Categorias);

            db = new DataBase();
            listaCategorias = db.selectTableCategoria();
            
            CreateActionBar();
            CrearLayoutCategorias();
            cantidadProductos = FindViewById<TextView>(Resource.Id.cantidadProductos);
            cantidadCompra = FindViewById<TextView>(Resource.Id.totalCompra);
        }

        protected override void OnResume()
        {
            base.OnResume();
            List<Producto> CarroDeCompra = db.selectQueryTableProductoParaCarro();

            cantidadProductos.Text = TotalProductos(CarroDeCompra)[0] + "";

            cantidadCompra.Text = "$" + TotalProductos(CarroDeCompra)[1];
        }

        // CREACION DEL TOOLBAR ----- *************************************
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_pedidos, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        
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

        public void CreateActionBar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Categorias";
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

        private void CrearLayoutCategorias()
        {
            ScrollView categorias;

            // Se crean botones apartir de categorias
            List<Boton> botonesDeCategoria = new List<Boton>();
            foreach (Categoria categoria in listaCategorias)
            {
                botonesDeCategoria.Add(new Boton(0, categoria.Nombre, "", categoria.Imagen, categoria.Id, "", "", ""));
            }
            
            // Se deben asignar las funciones en este orden, se crean los layouts de los botones de categorias y finalmente se asignan las funciones
            LinearLayout categoriasLayout = CrearBotones(botonesDeCategoria.Count, botonesDeCategoria);
            AsignarFuncionesABotones(botonesDeCategoria);

            categorias = FindViewById<ScrollView>(Resource.Id.categorias);
            categorias.SetBackgroundColor(new Android.Graphics.Color(Resource.Color.botoneraButtonColor));
            categorias.AddView(categoriasLayout);
        }

        private LinearLayout CrearBotones(double numeroBotones, List<Boton> botonesDeCategoria)
        {
            LayoutBotonCreation layoutCreation = new LayoutBotonCreation();

            //Se crea el layout de botones, con el número de filas y columnas indicado, los botones se crean vacíos
            LinearLayout categoriasLayout = layoutCreation.rootView((int)numeroBotones, 1);

            //Se agrega la información a los botones del layout
            layoutCreation.CrearBotonera(layoutCreation.layouts, botonesDeCategoria, 0, 10, Resources);

            return categoriasLayout;
        }

        //Asigna las acciones a los botones del teclado principal
        private void AsignarFuncionesABotones(List<Boton> botones)
        {
            foreach (Boton boton in botones)
            {
                String botonAction = (boton.getLabel().Length > 0) ? boton.getLabel() : boton.getFuncion();
                if (boton.GetButtonLayout() != null)
                {
                    boton.GetButtonLayout().Touch += (s, e) =>
                    {
                        var handled = false;

                        if (e.Event.Action == MotionEventActions.Down)
                        {
                            handled = true;
                        }

                        if (e.Event.Action == MotionEventActions.Up)
                        {
                            var intent = new Intent(this, typeof(Productos));
                            intent.PutExtra("Categoria", boton.getDato());
                            StartActivity(intent);
                            //Toast.MakeText(Application.Context, boton.getDato(), ToastLength.Short).Show();
                            boton.GetButtonLayout().SetBackgroundColor(new Android.Graphics.Color(Resource.Color.botoneraOnPressed));
                            //FuncionesBotones(botonAction, boton);handled = true;
                        }

                        e.Handled = handled;
                    };
                }
            }
        }
    }
}