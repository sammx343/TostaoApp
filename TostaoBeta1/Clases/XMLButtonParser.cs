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

namespace TostaoApp.Clases
{
    class XMLButtonParser
    {
        public List<Boton> getXMLfromResource(XmlDocument doc, String id)
        {
            List<Boton> buttonsParser = new List<Boton>();

            string xpath = "tecladoTactil/teclado";
            var keyboards = doc.SelectNodes(xpath);

            foreach (XmlNode keyboard in keyboards)
            {
                if (keyboard.Attributes["id"].Value.Equals(id))
                {
                    XmlNodeList buttons = keyboard.SelectNodes("boton");
                    foreach (XmlNode button in buttons)
                    {
                        XmlNode accion = button["accion"];
                        Console.WriteLine("algo");
                        int orden = int.Parse(button.Attributes["orden"].Value);
                        string label = button["label"].InnerText;
                        string funcion = accion.Attributes["funcion"].Value;
                        string dato = accion["dato"].InnerXml;
                        string imagen = button["imagen"].InnerText;
                        string over = button["over"].InnerText;
                        string Enabled = button["Enabled"].InnerText;
                        string Categoria = (button["Categoria"] == null) ? string.Empty : button["Categoria"].InnerText;

                        buttonsParser.Add(new Boton(orden, label, funcion, imagen, dato, over, Enabled, Categoria));
                    }
                }
            }
            return buttonsParser;
        }
    }
}