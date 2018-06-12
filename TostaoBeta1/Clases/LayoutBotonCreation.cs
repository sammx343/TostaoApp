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
using System.Text.RegularExpressions;
using Android.Content.Res;
using TostaoBeta1;

namespace TostaoApp.Clases
{
    class LayoutBotonCreation
    {
        public List<LinearLayout> layouts = new List<LinearLayout>();

        //Se crean las filas de LinearLayouts
        public LinearLayout rootView(int rows, int columns)
        {
            LinearLayout lrl = new LinearLayout(Application.Context);
            LinearLayout.LayoutParams rlp = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.MatchParent);
            rlp.SetMargins(0, 5, 0, 0);
            lrl.LayoutParameters = rlp;
            lrl.Orientation = Android.Widget.Orientation.Vertical;
            lrl.SetBackgroundColor(new Android.Graphics.Color(Resource.Color.botoneraButtonColor));
            lrl.WeightSum = rows;

            for (int index = 0; index < rows; index++)
            {
                lrl.AddView(createLinearLayout(columns));
            }
            return lrl;
        }

        //Se crean las columnas de LinearLayouts
        public LinearLayout createLinearLayout(int columns)
        {
            LinearLayout layout2 = new LinearLayout(Application.Context);
            LinearLayout.LayoutParams rlp = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MatchParent,
                    0, 1);
            rlp.SetMargins(0, 0, 3, 0);
            layout2.LayoutParameters = rlp;
            layout2.Orientation = Android.Widget.Orientation.Horizontal;
            layout2.WeightSum = columns;
            for (int index = 0; index < columns; index++)
            {
                layout2.AddView(createButton(Resource.Color.colorPrimaryDark));
            }
            return layout2;
        }

        //Se crea Botón por Botón dentro de cada 
        public LinearLayout createButton(int color)
        {
            LinearLayout.LayoutParams myParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
            myParams.SetMargins(0, 3, 3, 0);

            LinearLayout layout = new LinearLayout(Application.Context);
            layout.WeightSum = 2;
            layout.Orientation = Android.Widget.Orientation.Vertical;

            layout.LayoutParameters = myParams;

            layout.SetBackgroundColor(new Android.Graphics.Color(color));
            layouts.Add(layout);
            return layout;
        }

        public static int KEY_1 = 1;

        //Crea Botones de la botonera principal
        public List<LinearLayout> CrearBotonera(List<LinearLayout> layouts, List<Boton> botones, int startLayoutIndex, int endLayoutIndex, Resources resources)
        {
            int layoutIndex = 0;
            int buttonsIndex = 0;
            List<LinearLayout> principalButtonsToAssignAction = new List<LinearLayout>();

            foreach (LinearLayout layout in layouts)
            {
                if (layoutIndex >= startLayoutIndex && layoutIndex < endLayoutIndex && buttonsIndex < botones.Count)
                {
                    Boton button = botones[buttonsIndex];
                    String botonTitle = (button.getLabel().Length > 0) ? button.getLabel() : button.getFuncion();
                    if (botonTitle.Length > 0)
                    {
                        LinearLayout.LayoutParams myParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);

                        layout.RemoveAllViews();
                        myParams.SetMargins(0, 3, 3, 0);
                        layout.WeightSum = 2;
                        layout.Orientation = Android.Widget.Orientation.Vertical;
                        layout.LayoutParameters = myParams;

                        ImageView buttonImage = new ImageView(Application.Context);
                        if (button.getImagen().Length > 0)
                        {
                            LinearLayout.LayoutParams imageViewParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
                            imageViewParams.SetMargins(0, 5, 0, 0);
                            buttonImage.LayoutParameters = imageViewParams;

                            String mDrawableName = button.getImagen();

                            //Se necesita saber si el nombre de la imagen es un codigo, para así agregar 'p' al String de la imagen y poder obtenerla
                            //por ejemplo, un código de la imagen puede ser 0911900, y su nombre será p0911900.png, si la imagen posee solo numeros
                            //cumplirá con la ER y se le agregará una p al principio para poder leerla
                            String reg = "\\d.*\\d";
                            Regex r = new Regex(reg, RegexOptions.IgnoreCase);
                            Match m = r.Match(mDrawableName);
                            if (m.Success)
                                mDrawableName = "p" + button.getImagen();

                            //LOS BOTONES DE LAS CATEGORIAS TIENEN UN NOMBRE EN EL XML QUE NO SE PUEDE LEER EN ANDROID, SE LLAMA LA FUNCIÓN PARA CAMBIARLO
                            //if (button.getCategorias().Equals("Categorias") && button.getLabel() != null)
                                mDrawableName = capitalCaseTo_snake_case(button.getImagen());

                            int resID = resources.GetIdentifier(mDrawableName, "drawable", Application.Context.PackageName);

                            buttonImage.SetImageResource(resID);
                            layout.AddView(buttonImage);
                        }

                        TextView buttonTitle = new TextView(Application.Context);
                        LinearLayout.LayoutParams textViewParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1);
                        buttonTitle.LayoutParameters = textViewParams;
                        buttonTitle.Gravity = GravityFlags.Center;

                        layout.AddView(buttonTitle);
                        button.SetButtonLayout(layout);

                        layout.SetTag(Resource.Id.TAG_ONLINE_ID, button.getLabel() + "," + button.getFuncion() + "," + button.getCategorias());
                        principalButtonsToAssignAction.Add(layout);
                        buttonTitle.Text = botonTitle;
                        //SetOnPressedStyleToButton(layout, button.getLabel());
                    }
                    buttonsIndex++;
                }
                layoutIndex++;
            }
            return principalButtonsToAssignAction;
        }

        public void SetOnPressedStyleToButton(LinearLayout layout, String label)
        {
            layout.Touch += (s, e) =>
            {
                var handled = false;
                if (e.Event.Action == MotionEventActions.Down)
                {
                    
                    handled = true;
                }

                if (e.Event.Action == MotionEventActions.Up)
                {
                    Toast.MakeText(Application.Context, label, ToastLength.Short).Show();
                    layout.SetBackgroundColor(new Android.Graphics.Color(Resource.Color.botoneraOnPressed));
                    //layout.SetBackgroundColor(new Android.Graphics.Color(Application.Context.GetColor(Resource.Color.colorPrimaryDark)));
                    handled = true;
                }

                e.Handled = handled;
            };
        }


        //Muchos nombres de imagenes en el xml se encuentran en CapitalCase
        //Es necesario este método para cambiarlos a snake_case para poder cargar las imágenes
        public String capitalCaseTo_snake_case(String text)
        {
            //Split por mayúsculas
            String[] textArray = Regex.Split(text, @"(?<!^)(?=[A-Z])");

            String capitalcaseToSnakeCase = "";
            foreach (string word in textArray)
            {
                if (!word.Equals(""))
                {
                    capitalcaseToSnakeCase += "_" + word.ToLower();
                }
            }
            capitalcaseToSnakeCase = "categorias" + capitalcaseToSnakeCase;
            return capitalcaseToSnakeCase;
        }
    }
}