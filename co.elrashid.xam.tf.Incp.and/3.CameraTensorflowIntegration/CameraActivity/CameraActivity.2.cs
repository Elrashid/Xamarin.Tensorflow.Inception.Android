//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2

using Android.OS;
using Android.Graphics;
using static Android.Media.ImageReader;
using Android.Media;
using static Android.Media.Image;
using co.elrashid.xam.tf.Incp.and.Env;

namespace co.elrashid.xam.tf.Incp.and.Camera
{

    public partial class CameraActivity : ImageReader.IOnImageAvailableListener
    { 
        void IOnImageAvailableListener.OnImageAvailable(ImageReader reader)
        {
            Image image = null;

            try
            {
                image = reader.AcquireLatestImage();

                if (image == null)
                {
                    return;
                }

                if (computing)
                {
                    image.Close();
                    return;
                }
                computing = true;

                Trace.BeginSection("imageAvailable");

                Plane[] planes = image.GetPlanes();
                fillBytes(planes);

                int yRowStride = planes[0].RowStride;
                int uvRowStride = planes[1].RowStride;
                int uvPixelStride = planes[1].PixelStride;
                ImageUtils.convertYUV420ToARGB8888(
                    yuvBytes[0],
                    yuvBytes[1],
                    yuvBytes[2],
                    previewWidth,
                    previewHeight,
                    yRowStride,
                    uvRowStride,
                    uvPixelStride,
                    rgbBytes);

                image.Close();
            }
            catch (Java.Lang.Exception e)
            {
                if (image != null)
                {
                    image.Close();
                }
                LOGGER.e(e, "Exception!");
                Trace.EndSection();
                return;
            }

            rgbFrameBitmap.SetPixels(rgbBytes, 0, previewWidth, 0, 0, previewWidth, previewHeight);
            Canvas canvas = new Canvas(croppedBitmap);
            canvas.DrawBitmap(rgbFrameBitmap, frameToCropTransform, null);

            // For examining the actual TF input.
            if (SAVE_PREVIEW_BITMAP)
            {
                ImageUtils.saveBitmap(croppedBitmap);
            }

            runInBackground(this);


            Trace.EndSection();
        }
    }
}