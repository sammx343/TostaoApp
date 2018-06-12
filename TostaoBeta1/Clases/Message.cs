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

namespace TostaoApp.Clases
{
    public class Message
    {
        public string Type { get; set; }
        public int Version { get; set; }
        public char Format { get; set; }
        public string Content { get; set; }

        public Message()
        {
            Type = "666";
            Version = 666;
            Format = 'S';
            Content = "";
        }

        public Message(string type, int version, char format, string content)
        {
            Type = type;
            Version = version;
            Format = format;
            Content = content;
        }
    }




}