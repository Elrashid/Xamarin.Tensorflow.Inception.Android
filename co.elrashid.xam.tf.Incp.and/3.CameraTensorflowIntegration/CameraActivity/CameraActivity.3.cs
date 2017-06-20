//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2

using System;
using Android.Graphics;
using System.Collections.Generic;
using co.elrashid.xam.tf.Incp.and.CameraTensorflowIntegration;

namespace co.elrashid.xam.tf.Incp.and.Camera
{
    public partial class CameraActivity : IDrawCallback
    {
        public void drawCallback(Canvas canvas)
        {
            if (!Debug)
            {
                return;
            }
            Bitmap copy = cropCopyBitmap;
            if (copy != null)
            {
                Matrix matrix = new Matrix();
                float scaleFactor = 2;
                matrix.PostScale(scaleFactor, scaleFactor);
                matrix.PostTranslate(
                    canvas.Width - copy.Width * scaleFactor,
                    canvas.Height - copy.Height * scaleFactor);
                canvas.DrawBitmap(copy, matrix, new Paint());

                List<string> lines = new List<string>();
                if (classifier != null)
                {
                    string statString = classifier.StatString;
                    string[] statLines = statString.Split("\n", true);
                    foreach (string line in statLines)
                    {
                        lines.Add(line);
                    }
                }

                lines.Add("Frame: " + previewWidth + "x" + previewHeight);
                lines.Add("Crop: " + copy.Width + "x" + copy.Height);
                lines.Add("View: " + canvas.Width + "x" + canvas.Height);
                lines.Add("Rotation: " + sensorOrientation);
                lines.Add("Inference time: " + lastProcessingTimeMs + "ms");

                borderedText.drawLines(canvas, 10, canvas.Height - 10, lines);
            }
        }
      
    }
}