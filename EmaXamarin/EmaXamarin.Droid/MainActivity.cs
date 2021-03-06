﻿using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace EmaXamarin.Droid
{
    [Activity(Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        private static readonly Logging Logger = Logging.For<MainActivity>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => Logger.Error("Unhandled exception", args.ExceptionObject as Exception);

            var androidFileRepository = new AndroidFileRepository();

            AlwaysShowMenuButton();

            var app = new App(androidFileRepository, new ExternalBrowserService());
            LoadApplication(app);
        }

        private void AlwaysShowMenuButton()
        {
            try
            {
                var config = ViewConfiguration.Get(this);
                var hardwareMenu = config.Class.GetDeclaredField("sHasPermanentMenuKey");
                if (hardwareMenu != null)
                {
                    hardwareMenu.Accessible = true;
                    hardwareMenu.SetBoolean(config, false);
                }
            }
            catch
            {
            }
        }
    }
}