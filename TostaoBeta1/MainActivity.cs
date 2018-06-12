using Android.App;
using Android.Widget;
using Android.OS;
using TostaoApp.Actividades;
using Redsis.TostaoApp.CrossPlatformLogic;
using System;
using System.Text;
using System.Security.Cryptography;
using TostaoApp.Clases;
using Android.Util;
using System.IO;
using SQLite;
using TostaoApp.DataHelper;
using System.Collections.Generic;
using System.Xml;
using TostaoApp.ClasesTablas;
using NLipsum.Core;

using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TostaoApp
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        private SessionManager session;
        String tag = Constants.LOG;
        DataBase db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);

            // Session Manager
            session = new SessionManager(Application.Context);

            //Lectura de XML
            List<Boton> botones;

            //Creacion de base de datos
            db = new DataBase();
            db.createDataBase();
            //db.delete();
            
            botones = CreateBotonList("principal");
            InsertarTablaCategorias(botones);
            
            botones = CreateBotonList("tecladoCat");
            InsertarTablaProductos(botones);

            //Si el usuario se encuentra logeado no accede al Login y pasa directamente al Menu
            UserSessionManager();
        }

        private void InsertarTablaCategorias(List<Boton> botones)
        {
            //Solo se necesitan obtener los botones de categoría, por lo que se hace un filtro al array leido del XML
            //Hay algunos que tienen la opcion de Categorias pero no tienen Label, por eso pueden ser vacios o nulos
            foreach (Boton boton in botones)
            {
                if (boton.getCategorias().Equals("Categorias") && !string.IsNullOrEmpty(boton.getLabel()))
                {
                    Categoria categoria = new Categoria() {
                        Id = boton.getDato(),
                        Nombre = boton.getLabel(),
                        Imagen = boton.getImagen()
                    };
                    db.InsertIntoTableCategoria(categoria);
                }
            }
        }

        private void InsertarTablaProductos(List<Boton> botones)
        {
            foreach (Boton boton in botones)
            {
                if (!string.IsNullOrEmpty(boton.getLabel()))
                {
                    Producto producto = new Producto() {
                        Id = boton.getDato(),
                        IdCategoria = boton.getCategorias(),
                        Nombre = boton.getLabel(),
                        Imagen = boton.getImagen(),
                        Precio = (new Random()).Next(1, 20) * 1000,
                        Cantidad = 0,
                        Descripcion = LipsumGenerator.Generate(1),
                        Disponibilidad = true
                    };
                    db.InsertIntoTableProducto(producto);
                }
            }
        }

        // Verifica si el usuario se encuentra loggeado o no
        private void UserSessionManager()
        {
            try
            {
                if (session.isLoggedIn())
                {
                    Log.Info(tag, "Credenciales de Usuario ya guardadas, el usuario inicia sesión automáticamente");
                    StartActivity(typeof(Menu));
                }
                else
                {
                    Log.Info(tag, "El usuario accede a la Aplicación, debe colocar nombre y contraseña");
                    StartActivity(typeof(Login));
                }
                Finish();
            }
            catch (Exception e)
            {
                Log.Error(tag, "Error de inicio de sesión, usuario redirigido a la actividad de Login");
                Toast.MakeText(this, "Ocurrió un error iniciando sesión, accede nuevamente", ToastLength.Long).Show();
                Finish();
                StartActivity(typeof(Login));
            };
        }

        // Se crean las listas de botones del XML y se devuelve una lista
        private List<Boton> CreateBotonList(string Id)
        {
            Log.Info(tag, "Lectura de Datos para Categorías y Productos");
            
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Application.Context.Assets.Open("teclado.xml"));

                //Se crean botones a partir del teclado con el Id indicado
                XMLButtonParser buttonParser = new XMLButtonParser();
                return buttonParser.getXMLfromResource(doc, Id);
            }
            catch (Exception e)
            {
                //Si entra a la excepción ocurre que la información de categorías no se pudo leer
                Toast.MakeText(this, "No se pueden realizar pedidos en este momento", ToastLength.Long).Show();
                Finish();
                return null;
            }
        }

        
    }
}

