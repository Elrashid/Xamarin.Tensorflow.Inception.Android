//https://github.com/xamarin/monodroid-samples/tree/8d91a9a7fa0b958e4f649a644de4ea98ca1cf914/android5.0/Camera2Basic

using Android.App;
using Android.Hardware.Camera2;

namespace co.elrashid.xam.tf.Incp.and.Camera.Listeners
{
    public class CameraStateListener : CameraDevice.StateCallback
    {
        public Camera2BasicFragment owner;
        public override void OnOpened(CameraDevice cameraDevice)
        {
            // This method is called when the camera is opened.  We start camera preview here.
            owner.mCameraOpenCloseLock.Release();
            owner.mCameraDevice = cameraDevice;
            owner.CreateCameraPreviewSession();
        }

        public override void OnDisconnected(CameraDevice cameraDevice)
        {
            owner.mCameraOpenCloseLock.Release();
            cameraDevice.Close();
            owner.mCameraDevice = null;
        }

        public override void OnError(CameraDevice cameraDevice, CameraError error)
        {
            owner.mCameraOpenCloseLock.Release();
            cameraDevice.Close();
            owner.mCameraDevice = null;
            if (owner == null)
                return;
            Activity activity = owner.Activity;
            if (activity != null)
            {
                activity.Finish();
            }

        }
    }
}