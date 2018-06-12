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
using System.Security.Cryptography;

namespace TostaoApp.Clases
{
    class Encriptacion
    {
        //Metodo de Encriptación para las contraseñas de usuarios SHA512
        public string SHA512(string input)
        {
            string hash;
            var data = Encoding.UTF8.GetBytes(input);
            SHA512 shaM = new SHA512Managed();
            byte[] cryptedPass = shaM.ComputeHash(data);
            hash = Encoding.UTF8.GetString(cryptedPass, 0, cryptedPass.Length);
            Console.WriteLine("Clave cifrada por sha512");
            Console.WriteLine(hash);
            return hash;
        }
    }
}