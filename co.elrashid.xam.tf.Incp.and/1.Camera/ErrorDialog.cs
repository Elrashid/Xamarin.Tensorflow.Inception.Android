//https://github.com/xamarin/monodroid-samples/tree/8d91a9a7fa0b958e4f649a644de4ea98ca1cf914/android5.0/Camera2Basic

using Android.App;
using Android.Content;
using Android.OS;

namespace co.elrashid.xam.tf.Incp.and.Camera
{
    public class ErrorDialog : DialogFragment
    {
        private static readonly string ARG_MESSAGE = "message";
        private static Activity mActivity;

        private class PositiveListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public void OnClick(IDialogInterface dialog, int which)
            {
                mActivity.Finish();
            }
        }

        public static ErrorDialog NewInstance(string message)
        {
            var args = new Bundle();
            args.PutString(ARG_MESSAGE, message);
            return new ErrorDialog { Arguments = args };
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mActivity = Activity;
            return new AlertDialog.Builder(mActivity)
                .SetMessage(Arguments.GetString(ARG_MESSAGE))
                .SetPositiveButton(Android.Resource.String.Ok, new PositiveListener())
                .Create();
        }
    }
}