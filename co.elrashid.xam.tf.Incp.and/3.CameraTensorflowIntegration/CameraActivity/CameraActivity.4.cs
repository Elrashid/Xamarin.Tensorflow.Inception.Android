//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2


using Android.Graphics;
using Android.OS;
using co.elrashid.xam.tf.Incp.and.Tensorflow;
using Java.Lang;
using System.Collections.Generic;

namespace co.elrashid.xam.tf.Incp.and.Camera
{
    public partial class CameraActivity : IRunnable
    {
        public void Run()
        {
            long startTime = SystemClock.UptimeMillis();
            IList<Classifier_Recognition> results = classifier.recognizeImage(croppedBitmap);
            lastProcessingTimeMs = SystemClock.UptimeMillis() - startTime;

            cropCopyBitmap = Bitmap.CreateBitmap(croppedBitmap);
            resultsView.Results = results;
            requestRender();
            computing = false;
        }
    }
}