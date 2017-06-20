//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2


using Android.Graphics;
using Android.Util;
using Android.Views;
using co.elrashid.xam.tf.Incp.and.Tensorflow;
using co.elrashid.xam.tf.Incp.and.Env;
using co.elrashid.xam.tf.Incp.and.CameraTensorflowIntegration;

namespace co.elrashid.xam.tf.Incp.and.Camera
{
    public partial class CameraActivity: Camera2BasicFragment.ConnectionCallback
    {
        public void onPreviewSizeChosen(Android.Util.Size size, int rotation)
        {
            float textSizePx = TypedValue.ApplyDimension(ComplexUnitType.Dip, TEXT_SIZE_DIP, Resources.DisplayMetrics);
            borderedText = new BorderedText(textSizePx);
            borderedText.Typeface = Typeface.Monospace;


            classifier =
                TensorFlowImageClassifier.create(
                     Assets,
                    MODEL_FILE,
                    LABEL_FILE,
                    INPUT_SIZE,
                    IMAGE_MEAN,
                    IMAGE_STD,
                    INPUT_NAME,
                    OUTPUT_NAME);

            resultsView = (ResultsView)FindViewById(Resource.Id.results2);
            previewWidth = size.Width;
            previewHeight = size.Height;

            Display display = WindowManager.DefaultDisplay;
            int screenOrientation = (int)display.Rotation;

            LOGGER.i("Sensor orientation: %d, Screen orientation: %d", rotation, screenOrientation);

            sensorOrientation = rotation + screenOrientation;

            LOGGER.i("Initializing at size %dx%d", previewWidth, previewHeight);
            rgbBytes = new int[previewWidth * previewHeight];
            rgbFrameBitmap = Bitmap.CreateBitmap(previewWidth, previewHeight, Bitmap.Config.Argb8888);
            croppedBitmap = Bitmap.CreateBitmap(INPUT_SIZE, INPUT_SIZE, Bitmap.Config.Argb8888);

            frameToCropTransform =
                ImageUtils.getTransformationMatrix(
                    previewWidth, previewHeight,
                    INPUT_SIZE, INPUT_SIZE,
                    sensorOrientation.Value, MAINTAIN_ASPECT);

            cropToFrameTransform = new Matrix();
            frameToCropTransform.Invert(cropToFrameTransform);

            yuvBytes = new byte[3][];

            addCallback(this);
        }

    }
}