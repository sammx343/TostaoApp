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
using Redsis.TostaoApp.CrossPlatformLogic;
using Android.Util;
using System.IO;
using SQLite;
using TostaoApp.DataHelper;
using TostaoApp.Conexion;
using Newtonsoft.Json;
using Java.Util;
using TostaoBeta1;

//using System.Data.SqlClient;

namespace TostaoApp.Actividades
{
    [Activity(Label = "Menu")]
    public class Menu : Activity
    {
        Button pedido, mapa;
        private SessionManager session;
        String tag = Constants.LOG;
        DataBase db;
        ServiceBus serviceBus;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);

            session = new SessionManager(Application.Context);
            pedido = FindViewById<Button>(Resource.Id.botonPedido);
            mapa = FindViewById<Button>(Resource.Id.botonMapa);

            pedido.Click += delegate {
                Log.Info(tag, "Se accede a pedido");
                StartActivity(typeof(Categorias));
            };

            db = new DataBase();
            serviceBus = new ServiceBus();

            var categorias = db.selectTableCategoria();

            /*UUID uuidd = new UUID(4, 10);
            Console.WriteLine(uuidd.ToString());
            Android.Telephony.TelephonyManager manager = (Android.Telephony.TelephonyManager)this.ApplicationContext.GetSystemService(Context.TelephonyService);

            string[] PermissionsLocation =
            {
              Manifest.Permission.ReadPhoneState
            };

            ActivityCompat.RequestPermissions(this, PermissionsLocation, 0);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) == Android.Content.PM.Permission.Granted)
            {
                string deviceIde = manager.DeviceId;
                Console.WriteLine(deviceIde);
            }
            else
            {
                Console.WriteLine("No hay permisos");
            }

            mapa.Click += delegate
            {
                Clases.Message message = new Clases.Message();
                string userDataString = JsonConvert.SerializeObject(new ClasesTablas.User(), Newtonsoft.Json.Formatting.Indented);
                message.Content = userDataString;
                string mensaje = JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented);
                //serviceBus.MainAsync(mensaje);
            };
            */
            //Connection();
            CreateActionBar();
        }

        // CREACION DEL TOOLBAR ----- *************************************
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //Acciones del Toolbar de Menu
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.toolbar_carro_de_compra:
                    Log.Info(tag, "Se accede al carro de compra");
                    StartActivity(typeof(CarroDeCompra));
                    break;
                case Resource.Id.toolbar_salir:
                    Log.Info(tag, "El usuario cierra la sesión");
                    CerrarSesion();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void CreateActionBar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Menu";
            ActionBar.SetHomeButtonEnabled(true);
        }
 // CREACION DEL TOOLBAR ----- *************************************

        public override void OnBackPressed()
        {
            CerrarSesion();
        }

        private void CerrarSesion()
        {
            if (session.isLoggedIn())
            {
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                alertDialog.SetTitle("Cerrar Sesión");
                alertDialog.SetMessage("Desea cerrar Sesión?");
                alertDialog.SetPositiveButton("Aceptar", delegate
                {
                    Log.Info(tag, "El usuario cierra la sesión");
                    session.setLogin(false);
                    Finish();
                    StartActivity(typeof(MainActivity));
                });
                alertDialog.SetNegativeButton("Cancelar", delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }
        }

        /*
        public void Connection()
        {
            //SqlCommand com = new SqlCommand("INSERT INTO tblVALUES(@AssetID,@AssetName,@ConfigID)");
            string connectionString = @"Server=<192.168.36.50>;Database=<eva>;User Id=admindb;Password=<R3ds1s2016>;Trusted_Connection=true";
            string databaseTable = "<articulo>";
            string referenceAccountNumber = "0001134919";
            string selectQuery = String.Format("SELECT * FROM articulo ", databaseTable, referenceAccountNumber);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //open connection
                    connection.Open();

                    SqlCommand command = new SqlCommand(selectQuery, connection);

                    Toast.MakeText(this, "ENTRA EN SQL", ToastLength.Short).Show();

                    command.Connection = connection;
                    command.CommandText = selectQuery;
                    var result = command.ExecuteReader();
                    //check if account exists
                    var exists = result.HasRows;
                }
            }
            catch (Exception exception)
            {
                #region connection error
                AlertDialog.Builder connectionException = new AlertDialog.Builder(this);
                connectionException.SetTitle("Connection Error");
                connectionException.SetMessage(exception.ToString());
                connectionException.SetNegativeButton("Return", delegate { });
                connectionException.Create();
                connectionException.Show();
                #endregion
            }
        }
        */
    }
}