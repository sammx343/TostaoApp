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
using System.Security.Cryptography;
using Redsis.TostaoApp.CrossPlatformLogic;
using Android.Util;
using System.IO;
using SQLite;
using TostaoBeta1;

namespace TostaoApp.Actividades
{
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        Button loginButton;
        EditText correoUsuario, passUsuario;
        TextView registroButton;
        int contadorErrores = 4;
        string usuario = "user";
        string contraseña = "password";
        private SessionManager session;
        Encriptacion encript;
        String tag = Constants.LOG;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            // Create your application here
            loginButton = FindViewById<Button>(Resource.Id.login);
            correoUsuario = FindViewById<EditText>(Resource.Id.correo_usuario);
            passUsuario = FindViewById<EditText>(Resource.Id.contraseña_usuario);
            registroButton = FindViewById<TextView>(Resource.Id.boton_registrar_usuario);

            // Session Manager
            session = new SessionManager(Application.Context);
            encript = new Encriptacion();

            loginButton.Click += ButtonLogin;
            registroButton.Click += delegate
            {
                Log.Info(tag, "Se accede a la pantalla de Registro");
                Finish();
                StartActivity(typeof(Registro));
            };
        }

        private void ButtonLogin(object sender, EventArgs e)
        {
            Log.Info(tag, "Se presiona el botón de Inicio de Sesión");
            if (string.IsNullOrEmpty(correoUsuario.Text))
            {
                ShowAlertMissingField(GetString(Resource.String.error_email));
                correoUsuario.RequestFocus();
                return;
            }
            else if (string.IsNullOrEmpty(passUsuario.Text))
            {
                ShowAlertMissingField(GetString(Resource.String.error_contraseña));
                passUsuario.RequestFocus();
                return;
            }
            else if (passUsuario.Length() <= 4)
            {
                ShowAlertMissingField(GetString(Resource.String.error_contraseña_distinta));
                passUsuario.RequestFocus();
                return;
            }

            if (!usuario.Equals(correoUsuario.Text) || !contraseña.Equals(passUsuario.Text))
            {
                ShowAlertMissingField("Usuario o Contraseña incorrectos, intentelo de nuevo");
                return;
            }
            Log.Info(tag, "No hubo errores. El usuario logró acceder a su sesión");
            session.setLogin(true);
            encript.SHA512(passUsuario.Text);

            Finish();
            StartActivity(typeof(Menu));
        }

        private void ShowAlertMissingField(string message)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(GetString(Resource.String.error));
            alertDialog.SetMessage(message);
            alertDialog.SetNegativeButton(GetString(Resource.String.boton_aceptar), delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

    }
}