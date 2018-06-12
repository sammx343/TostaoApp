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
    class SessionManager
    {
        ISharedPreferences sharedPreferences;
        ISharedPreferencesEditor sharedPreferencesEditor;
        int PRIVATE_MODE = 0;

        private static String PREF_NAME = "UserLogin";
        private static String KEY_IS_LOGGEDIN = "isLoggedIn";

        public SessionManager(Context context)
        {
            FileCreationMode fileCreationMode = new FileCreationMode();
            sharedPreferences = context.GetSharedPreferences(PREF_NAME, fileCreationMode);
            sharedPreferencesEditor = sharedPreferences.Edit();
        }

        public void setLogin(bool isLoggedIn)
        {
            sharedPreferencesEditor.PutBoolean(KEY_IS_LOGGEDIN, isLoggedIn);
            sharedPreferencesEditor.Commit();
        }

        public bool isLoggedIn()
        {
            return sharedPreferences.GetBoolean(KEY_IS_LOGGEDIN, false);
        }
    }
}