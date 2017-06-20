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
using Android.Graphics;

namespace co.elrashid.xam.tf.Incp.and.CameraTensorflowIntegration
{
    public interface IDrawCallback
    {
        void drawCallback(Canvas canvas);
    }
}