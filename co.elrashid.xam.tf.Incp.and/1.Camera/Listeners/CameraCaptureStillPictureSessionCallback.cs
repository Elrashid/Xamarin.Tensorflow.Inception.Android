//https://github.com/xamarin/monodroid-samples/tree/8d91a9a7fa0b958e4f649a644de4ea98ca1cf914/android5.0/Camera2Basic

using Android.Hardware.Camera2;
using Android.Util;

namespace co.elrashid.xam.tf.Incp.and.Camera.Listeners
{
    public class CameraCaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
    {
        private static readonly string TAG = "CameraCaptureStillPictureSessionCallback";

        public Camera2BasicFragment Owner { get; set; }

        public CameraCaptureStillPictureSessionCallback(Camera2BasicFragment owner)
        {
            Owner = owner;
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            Owner.ShowToast("Saved: " + Owner.mFile);
            Log.Debug(TAG, Owner.mFile.ToString());
            Owner.UnlockFocus();
        }
    }
}