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

namespace TostaoApp.ClasesTablas
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string UUID { get; set; }
        public long Phone { get; set; }

        public User()
        {
            Id = 1;
            Email = "semamo@redsis.com";
            Name = "Sebastian";
            Lastname = "Mayor";
            Password = "123456";
            UUID = "001";
            Phone = 3104523698;
        }

        public User(int id, string email, string name, string lastname, string password, string uuid, long phone)
        {
            Id = id;
            Email = email;
            Name = name;
            Lastname = lastname;
            Password = password;
            UUID = uuid;
            Phone = phone;
        }
    }
    
}