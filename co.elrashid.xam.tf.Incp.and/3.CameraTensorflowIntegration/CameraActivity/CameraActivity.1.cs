//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Graphics;
using static Android.Media.Image;
using Java.Nio;
using Java.Lang;
using co.elrashid.xam.tf.Incp.and.Env;
using co.elrashid.xam.tf.Incp.and.CameraTensorflowIntegration;
using co.elrashid.xam.tf.Incp.and.Tensorflow;

namespace co.elrashid.xam.tf.Incp.and.Camera
{



    public partial class CameraActivity
    {
        #region 1
        private static readonly Logger LOGGER = new Logger();

        private const int INPUT_SIZE = 299;
        private const int IMAGE_MEAN = 128;
        private const float IMAGE_STD = 128.0f;
        private const string INPUT_NAME = "Mul:0";
        private const string OUTPUT_NAME = "final_result";

        private const string MODEL_FILE = "file:///android_asset/rounded_graph.pb";
        private const string LABEL_FILE = "file:///android_asset/retrained_labels.txt";

        private const bool SAVE_PREVIEW_BITMAP = false;

        private const bool MAINTAIN_ASPECT = true;

        private static readonly Android.Util.Size DESIRED_PREVIEW_SIZE = new Android.Util.Size(640, 480);

        private IClassifier classifier;

        private int? sensorOrientation;

        private int previewWidth = 0;
        private int previewHeight = 0;
        private byte[][] yuvBytes;
        private int[] rgbBytes = null;
        private Bitmap rgbFrameBitmap = null;
        private Bitmap croppedBitmap = null;

        private Bitmap cropCopyBitmap;

        private bool computing = false;

        private Matrix frameToCropTransform;
        private Matrix cropToFrameTransform;

        private ResultsView resultsView;

        private BorderedText borderedText;

        private long lastProcessingTimeMs;


        private const float TEXT_SIZE_DIP = 10;

        #endregion

        int LayoutId
        {
            get
            {
                return Resource.Layout.fragment_camera2_basic2;
            }
        }

        Android.Util.Size DesiredPreviewFrameSize
        {
            get
            {
                return DESIRED_PREVIEW_SIZE;
            }
        }




        void onSetDebug(System.Boolean debug)
        {
            classifier.enableStatLogging(debug);
        }





        void fillBytes(Plane[] planes)
        {
            // Because of the variable row stride it's not possible to know in
            // advance the actual necessary dimensions of the yuv planes.
            for (int i = 0; i < planes.Length; ++i)
            {
                ByteBuffer buffer = planes[i].Buffer;
                if (yuvBytes[i] == null)
                {
                    LOGGER.d("Initializing buffer %d at size %d", i, buffer.Capacity());
                    yuvBytes[i] = new byte[buffer.Capacity()];
                }
                buffer.Get(yuvBytes[i]);
            }
        }

        void runInBackground(IRunnable r)
        {
            if (handler != null)
            {
                handler.Post(r);
            }
        }
        private Handler handler;
        private HandlerThread handlerThread;

        protected override void OnPause()
        {
            LOGGER.d("onPause " + this);

            if (!IsFinishing)
            {
                LOGGER.d("Requesting finish");
                Finish();
            }

            handlerThread.QuitSafely();
            try
            {
                handlerThread.Join();
                handlerThread = null;
                handler = null;
            }
            catch (InterruptedException e)
            {
                LOGGER.e(e, "Exception!");
            }

            base.OnPause();

        }


        protected override void OnStop()
        {
            LOGGER.d("onStop " + this);
            base.OnStop();
        }


        protected override void OnResume()
        {
            LOGGER.d("onResume " + this);
            base.OnResume();

            handlerThread = new HandlerThread("inference");
            handlerThread.Start();
            handler = new Handler(handlerThread.Looper);
        }

        void addCallback(IDrawCallback callback)
        {

            OverlayView overlay = (OverlayView)FindViewById(Resource.Id.debug_overlay2);
            if (overlay != null)
            {
                overlay.addCallback(callback);
            }
        }

        public void requestRender()
        {
            OverlayView overlay = (OverlayView)FindViewById(Resource.Id.debug_overlay2);
            if (overlay != null)
            {
                overlay.PostInvalidate();
            }
        }

        private bool debug = false;

        virtual protected bool Debug
        {
            get
            {
                return debug;
            }
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.VolumeDown || keyCode == Keycode.VolumeUp)
            {
                debug = !debug;
                requestRender();
                onSetDebug(debug);
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        void setFragment()
        {

            Fragment fragment = Camera2BasicFragment.newInstance(this, this, LayoutId, DesiredPreviewFrameSize);

            FragmentManager.BeginTransaction().Replace(Resource.Id.container2, fragment).Commit();
        }


    }
}