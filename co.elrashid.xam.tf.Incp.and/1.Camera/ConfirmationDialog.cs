//https://github.com/xamarin/monodroid-samples/tree/8d91a9a7fa0b958e4f649a644de4ea98ca1cf914/android5.0/Camera2Basic

using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V13.App;

namespace co.elrashid.xam.tf.Incp.and.Camera
{
    public class ConfirmationDialog : DialogFragment
    {
        private static Fragment mParent;
        private class PositiveListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public void OnClick(IDialogInterface dialog, int which)
            {
                FragmentCompat.RequestPermissions(mParent,
                                new string[] { Manifest.Permission.Camera }, Camera2BasicFragment.REQUEST_CAMERA_PERMISSION);
            }
        }

        private class NegativeListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public void OnClick(IDialogInterface dialog, int which)
            {
                Activity activity = mParent.Activity;
                if (activity != null)
                {
                    activity.Finish();
                }
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mParent = ParentFragment;
            return new AlertDialog.Builder(Activity)
                .SetMessage(Resource.String.request_permission)
                .SetPositiveButton(Android.Resource.String.Ok, new PositiveListener())
                .SetNegativeButton(Android.Resource.String.Cancel, new NegativeListener())
                .Create();
        }
    }
}