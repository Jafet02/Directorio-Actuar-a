using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DirecAct.Droid
{
    [Activity(Label = "Directorio Actuaría", Icon = "@drawable/icon_actuaria", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region Singleton
        static MainActivity instance;

        public static MainActivity GetInstance()
        {
            if (instance == null)
            {
                instance = new MainActivity();
            }
            return instance;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        #endregion
    }
}