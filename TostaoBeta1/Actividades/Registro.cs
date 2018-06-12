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
using Android.Util;
using System.ComponentModel.DataAnnotations;
using Redsis.TostaoApp.CrossPlatformLogic;
using System.Text.RegularExpressions;
using TostaoApp.Clases;
using TostaoApp.Conexion;
using Newtonsoft.Json;
using TostaoBeta1;

namespace TostaoApp.Actividades
{
    [Activity(Label = "Activity1")]
    public class Registro : Activity
    {
        EditText correoUsuario, nombreUsuario, apellidoUsuario, passUsuario, repetirPass;
        Button aceptarRegistro;
        String tag = Constants.LOG;
        Encriptacion encript;
        SessionManager session;
        ServiceBus serviceBus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Registro);

            FindViews();

            session = new SessionManager(Application.Context);
            encript = new Encriptacion();
            serviceBus = new ServiceBus();
            aceptarRegistro.Click += ButtonRegistro;
        }
        
        public override void OnBackPressed()
        {
            CancelarRegistro();
        }

        private void FindViews()
        {
            correoUsuario = FindViewById<EditText>(Resource.Id.correo_usuario);
            nombreUsuario = FindViewById<EditText>(Resource.Id.nombre_usuario);
            apellidoUsuario = FindViewById<EditText>(Resource.Id.apellido_usuario);
            passUsuario = FindViewById<EditText>(Resource.Id.contraseña_usuario);
            repetirPass = FindViewById<EditText>(Resource.Id.repetir_contraseña_usuario);
            aceptarRegistro = FindViewById<Button>(Resource.Id.boton_aceptar_registro);
        }

        private void ButtonRegistro(object sender, EventArgs e)
        {
            
            
            if (string.IsNullOrEmpty(correoUsuario.Text))
            {
                ShowAlertField(GetString(Resource.String.error_email), GetString(Resource.String.error));
                correoUsuario.RequestFocus();
                return;
            }
            else if (!IsValidEmail(correoUsuario.Text))
            {
                ShowAlertField(GetString(Resource.String.error_email_no_valido), GetString(Resource.String.error));
                correoUsuario.RequestFocus();
                return;
            }
            else if (string.IsNullOrEmpty(nombreUsuario.Text))
            {
                ShowAlertField(GetString(Resource.String.error_nombre), GetString(Resource.String.error));
                nombreUsuario.RequestFocus();
                return;
            }
            else if (string.IsNullOrEmpty(apellidoUsuario.Text))
            {
                ShowAlertField(GetString(Resource.String.error_apellido), GetString(Resource.String.error));
                apellidoUsuario.RequestFocus();
                return;
            }
            else if (string.IsNullOrEmpty(passUsuario.Text))
            {
                ShowAlertField(GetString(Resource.String.error_contraseña), GetString(Resource.String.error));
                passUsuario.RequestFocus();
                return;
            }
            else if (passUsuario.Text.Length <= 4) {
                ShowAlertField(GetString(Resource.String.error_contraseña_minimos_caracteres), GetString(Resource.String.error));
                passUsuario.RequestFocus();
                return;
            }
            else if (!passUsuario.Text.Equals(repetirPass.Text))
            {
                ShowAlertField(GetString(Resource.String.error_contraseña_distinta), GetString(Resource.String.error));
                repetirPass.RequestFocus();
                return;
            }

            Log.Info(tag, "No hubo errores. El usuario logró crear su cuenta");



            encript.SHA512(passUsuario.Text);
            session.setLogin(true);
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Creado");
            alertDialog.SetMessage("El usuario fue creado con éxito");
            alertDialog.SetPositiveButton("Aceptar", delegate
            {
                Finish();
                StartActivity(typeof(Menu));
            });
            alertDialog.SetCancelable(false);
            alertDialog.Show();
        }

        private bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            return match.Success;
        }

        private void ShowAlertField(string message, string title)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
            alertDialog.SetNegativeButton("Aceptar", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        private void CancelarRegistro() {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle("Salir");
            alertDialog.SetMessage("Desea cancelar el registro de usuario?");
            alertDialog.SetPositiveButton("Aceptar", delegate
            {
                Log.Info(tag, "El usuario cancela el registro, se devuelve a Login");
                Finish();
                StartActivity(typeof(Login));
            });
            alertDialog.SetNegativeButton("Cancelar", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }
    }
}