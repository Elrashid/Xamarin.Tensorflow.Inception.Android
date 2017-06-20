//https://github.com/xamarin/monodroid-samples/tree/8d91a9a7fa0b958e4f649a644de4ea98ca1cf914/android5.0/Camera2Basic

using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using static Android.Media.ImageReader;
using Android.Media;

namespace co.elrashid.xam.tf.Incp.and.Camera
{

    [Activity(Label = "XamarinTF", MainLauncher = true, Icon = "@drawable/icon")]

    public partial class CameraActivity : Activity 
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.Hide();
            SetContentView(Resource.Layout.activity_camera2);

            if (bundle == null)
            {
                setFragment();
            }
        } 


    }
}

