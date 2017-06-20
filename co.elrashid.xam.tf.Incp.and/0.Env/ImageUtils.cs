//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2
using System;

/*
 * Copyright 2017 The Android Things Samples Authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace co.elrashid.xam.tf.Incp.and.Env
{

    using Bitmap = Android.Graphics.Bitmap;
    using Canvas = Android.Graphics.Canvas;
    using Matrix = Android.Graphics.Matrix;
    using Image = Android.Media.Image;
    using Environment = Android.OS.Environment;
    //   using Assert = junit.framework.Assert;
    using Assert = NUnit.Framework.Assert;
    using Java.IO;
    using Java.Nio;


    
    public class ImageUtils
    {
        
        private static readonly Logger LOGGER = new Logger();


        internal const int kMaxChannelValue = 262143;



        public static int getYUVByteSize(int width, int height)
        {
 
            int ySize = width * height;

            int uvSize = ((width + 1) / 2) * ((height + 1) / 2) * 2;

            return ySize + uvSize;
        }

        public static void saveBitmap(Bitmap bitmap)
        {
 
            string root = Environment.ExternalStorageDirectory.AbsolutePath + File.Separator + "tensorflow";
            LOGGER.i("Saving %dx%d bitmap to %s.", bitmap.Width, bitmap.Height, root);
;
            File myDir = new File(root);

            if (!myDir.Mkdirs())
            {
                LOGGER.i("Make dir failed");
            }

            const string fname = "preview.png";

            File file = new File(myDir, fname);
            if (file.Exists())
            {
                file.Delete();
            }
            try
            {

                System.IO.FileStream @out = new System.IO.FileStream(file.Path , System.IO.FileMode.Create, System.IO.FileAccess.Write);
                bitmap.Compress(Bitmap.CompressFormat.Png, 99, @out);
                @out.Flush();
                @out.Close();
            }

            catch (Exception e)
            {
                LOGGER.e(e.ToString(), "Exception!");
            }
        }


        public static int[] convertImageToBitmap(Image image, int[] output, byte[][] cachedYuvBytes)
        {
            if (cachedYuvBytes == null || cachedYuvBytes.Length != 3)
            {
                cachedYuvBytes = new byte[3][];
            }
            Image.Plane[] planes = image.GetPlanes();
            fillBytes(planes, cachedYuvBytes);

            int yRowStride = planes[0].RowStride;
 
            int uvRowStride = planes[1].RowStride;
 
            int uvPixelStride = planes[1].PixelStride;

            convertYUV420ToARGB8888(cachedYuvBytes[0], cachedYuvBytes[1], cachedYuvBytes[2], image.Width, image.Height, yRowStride, uvRowStride, uvPixelStride, output);
            return output;
        }

        public static void convertYUV420ToARGB8888(byte[] yData, byte[] uData, byte[] vData, int width, int height, int yRowStride, int uvRowStride, int uvPixelStride, int[] @out)
        {
            int i = 0;
            for (int y = 0; y < height; y++)
            {
                int pY = yRowStride * y;
                int uv_row_start = uvRowStride * (y >> 1);
                int pU = uv_row_start;
                int pV = uv_row_start;

                for (int x = 0; x < width; x++)
                {
                    int uv_offset = (x >> 1) * uvPixelStride;
                    @out[i++] = YUV2RGB(convertByteToInt(yData, pY + x), convertByteToInt(uData, pU + uv_offset), convertByteToInt(vData, pV + uv_offset));
                }
            }
        }

        private static int convertByteToInt(byte[] arr, int pos)
        {
            return arr[pos] & 0xFF;
        }

        private static int YUV2RGB(int nY, int nU, int nV)
        {
            nY -= 16;
            nU -= 128;
            nV -= 128;
            if (nY < 0)
            {
                nY = 0;
            }

            // This is the floating point equivalent. We do the conversion in integer
            // because some Android devices do not have floating point in hardware.
            // nR = (int)(1.164 * nY + 2.018 * nU);
            // nG = (int)(1.164 * nY - 0.813 * nV - 0.391 * nU);
            // nB = (int)(1.164 * nY + 1.596 * nV);

            int nR = (int)(1192 * nY + 1634 * nV);
            int nG = (int)(1192 * nY - 833 * nV - 400 * nU);
            int nB = (int)(1192 * nY + 2066 * nU);

            nR = Math.Min(kMaxChannelValue, Math.Max(0, nR));
            nG = Math.Min(kMaxChannelValue, Math.Max(0, nG));
            nB = Math.Min(kMaxChannelValue, Math.Max(0, nB));

            nR = (nR >> 10) & 0xff;
            nG = (nG >> 10) & 0xff;
            nB = (nB >> 10) & 0xff;

            return unchecked((int)0xff000000) | (nR << 16) | (nG << 8) | nB;
        }
 
        private static void fillBytes(Image.Plane[] planes, byte[][] yuvBytes)
        {
            // Because of the variable row stride it's not possible to know in
            // advance the actual necessary dimensions of the yuv planes.
            for (int i = 0; i < planes.Length; ++i)
            {
 
                ByteBuffer buffer = planes[i].Buffer;
                if (yuvBytes[i] == null || yuvBytes[i].Length != buffer.Capacity())
                {
                    yuvBytes[i] = new  byte[buffer.Capacity()];
                }
                buffer.Get(yuvBytes[i]);
            }
        }


 
        public static void cropAndRescaleBitmap(Bitmap src, Bitmap dst, int sensorOrientation)
        {
            Assert.AreEqual(dst.Width, dst.Height);
 
            float minDim = Math.Min(src.Width, src.Height);
 
            Matrix matrix = new Matrix();
 
            float translateX = -Math.Max(0, (src.Width - minDim) / 2);
 
            float translateY = -Math.Max(0, (src.Height - minDim) / 2);
            matrix.PreTranslate(translateX, translateY);


            float scaleFactor = dst.Height / minDim;
            matrix.PostScale(scaleFactor, scaleFactor);

            // Rotate around the center if necessary.
            if (sensorOrientation != 0)
            {
                matrix.PostTranslate(-dst.Width / 2.0f, -dst.Height / 2.0f);
                matrix.PostRotate(sensorOrientation);
                matrix.PostTranslate(dst.Width / 2.0f, dst.Height / 2.0f);
            }

            Canvas canvas = new Canvas(dst);
            canvas.DrawBitmap(src, matrix, null);
        }

 
        public static Matrix getTransformationMatrix(int srcWidth, int srcHeight, int dstWidth, int dstHeight, int applyRotation, bool maintainAspectRatio)
        {
           
            Matrix matrix = new Matrix();

            if (applyRotation != 0)
            {
               
                matrix.PostTranslate(-srcWidth / 2.0f, -srcHeight / 2.0f);

             
                matrix.PostRotate(applyRotation);
            }

           
            bool transpose = (Math.Abs(applyRotation) + 90) % 180 == 0;
  
            int inWidth = transpose ? srcHeight : srcWidth;
 
            int inHeight = transpose ? srcWidth : srcHeight;

            // Apply scaling if necessary.
            if (inWidth != dstWidth || inHeight != dstHeight)
            {
             
                float scaleFactorX = dstWidth / (float)inWidth;
                
                float scaleFactorY = dstHeight / (float)inHeight;

                if (maintainAspectRatio)
                {
                 
                    float scaleFactor = Math.Max(scaleFactorX, scaleFactorY);
                    matrix.PostScale(scaleFactor, scaleFactor);
                }
                else
                {
                   
                    matrix.PostScale(scaleFactorX, scaleFactorY);
                }
            }

            if (applyRotation != 0)
            {
                matrix.PostTranslate(dstWidth / 2.0f, dstHeight / 2.0f);
            }

            return matrix;
        }
    }

}