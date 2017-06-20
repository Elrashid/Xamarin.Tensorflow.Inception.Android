//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2

/* Copyright 2016 The TensorFlow Authors. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
==============================================================================*/
using System;
using System.Collections.Generic;
namespace co.elrashid.xam.tf.Incp.and.Tensorflow
{

    using AssetManager = Android.Content.Res.AssetManager;
    using Bitmap = Android.Graphics.Bitmap;
    using Trace = Android.OS.Trace;
    using Log = Android.Util.Log;
    using TensorFlowInferenceInterface = Org.Tensorflow.Contrib.Android.TensorFlowInferenceInterface;
    using Org.Tensorflow;
    using Java.Lang;

    /// <summary>
    /// A classifier specialized to label images using TensorFlow. </summary>
    public class TensorFlowImageClassifier : IClassifier
    {
        private const string TAG = "TensorFlowImageClassifier";

        // Only return this many results with at least this confidence.
        private const int MAX_RESULTS = 3;
        private const float THRESHOLD = 0.1f;

        // Config values.
        private string inputName;
        private string outputName;
        private int inputSize;
        private int imageMean;
        private float imageStd;

        // Pre-allocated buffers.
        private List<string> labels = new List<string>();
        private int[] intValues;
        private float[] floatValues;
        private float[] outputs;
        private string[] outputNames;

        private bool logStats = false;

        private TensorFlowInferenceInterface inferenceInterface;

        private TensorFlowImageClassifier()
        {
        }

        public static IClassifier create(AssetManager assetManager, string modelFilename, string labelFilename, int inputSize, int imageMean, float imageStd, string inputName, string outputName)
        {
            TensorFlowImageClassifier c = new TensorFlowImageClassifier();
            c.inputName = inputName;
            c.outputName = outputName;

            // Read the label names into memory.
            // TODO(andrewharp): make this handle non-assets.
            string actualFilename = labelFilename.Split("file:///android_asset/", true)[1];
            Log.Info(TAG, "Reading labels from: " + actualFilename);
            System.IO.StreamReader br = null;
            try
            {
                br = new System.IO.StreamReader(assetManager.Open(actualFilename));
                string line;
                while (!string.ReferenceEquals((line = br.ReadLine()), null))
                {
                    c.labels.Add(line);
                }
                br.Close();
            }
            catch (Java.IO.IOException e)
            {
                throw new Exception("Problem reading label file!", e);
            }

            c.inferenceInterface = new TensorFlowInferenceInterface(assetManager, modelFilename);

            Operation operation = c.inferenceInterface.GraphOperation(outputName);

            int numClasses = (int)operation.Output(0).Shape().Size(1);
            Log.Info(TAG, "Read " + c.labels.Count + " labels, output layer size is " + numClasses);

            // Ideally, inputSize could have been retrieved from the shape of the input operation.  Alas,
            // the placeholder node for input in the graphdef typically used does not specify a shape, so it
            // must be passed in as a parameter.
            c.inputSize = inputSize;
            c.imageMean = imageMean;
            c.imageStd = imageStd;

            // Pre-allocate buffers.
            c.outputNames = new string[] { outputName };
            c.intValues = new int[inputSize * inputSize];
            c.floatValues = new float[inputSize * inputSize * 3];
            c.outputs = new float[numClasses];

            return c;
        }

        public virtual IList<Classifier_Recognition> recognizeImage(Bitmap bitmap)
        {
            // Log this method so that it can be analyzed with systrace.
            Trace.BeginSection("recognizeImage");

            Trace.BeginSection("preprocessBitmap");
            // Preprocess the image data from 0-255 int to normalized float based
            // on the provided parameters.
            bitmap.GetPixels(intValues, 0, bitmap.Width, 0, 0, bitmap.Width, bitmap.Height);
            for (int i = 0; i < intValues.Length; ++i)
            {
 
                int val = intValues[i];
                floatValues[i * 3 + 0] = (((val >> 16) & 0xFF) - imageMean) / imageStd;
                floatValues[i * 3 + 1] = (((val >> 8) & 0xFF) - imageMean) / imageStd;
                floatValues[i * 3 + 2] = ((val & 0xFF) - imageMean) / imageStd;
            }
            Trace.EndSection();

            // Copy the input data into TensorFlow.
            Trace.BeginSection("feed");
            inferenceInterface.Feed(inputName, floatValues, 1, inputSize, inputSize, 3);
            Trace.EndSection();

            // Run the inference call.
            Trace.BeginSection("run");
            inferenceInterface.Run(outputNames, logStats);
            Trace.EndSection();

            // Copy the output Tensor back into the output array.
            Trace.BeginSection("fetch");
            inferenceInterface.Fetch(outputName, outputs);
            Trace.EndSection();
            // Find the best classifications.
            //     Java.Util.PriorityQueue.<Classifier_Recognition> pq = new PriorityQueue<Classifier_Recognition>(3, new ComparatorAnonymousInnerClass(this));

            //  var pq = new Java.Util.PriorityQueue(3, new cc() );
            var qqq = new Queue<Classifier_Recognition>(3);

            for (int i = 0; i < outputs.Length; ++i)
            {
                if (outputs[i] > THRESHOLD)
                {
 
                    qqq.Enqueue(new Classifier_Recognition("" + i, labels.Count > i ? labels[i] : "unknown", outputs[i], null));
                }
            }



            for (int i = 0; i < outputs.Length; ++i)
            {
                if (outputs[i] > THRESHOLD)
                {
                    qqq.Enqueue(new Classifier_Recognition("" + i, labels.Count > i ? labels[i] : "unknown", outputs[i], null));
                }
            }
             List<Classifier_Recognition> recognitions = new List<Classifier_Recognition>();


             int recognitionsSize = Math.Min(qqq.Count, MAX_RESULTS);


            for (int i = 0; i < recognitionsSize; ++i)
            {
                 recognitions.Add(qqq.Dequeue());

            }



            Trace.EndSection(); // "recognizeImage"
            return recognitions;
        }




 

        public virtual void enableStatLogging(bool logStats)
        {
            this.logStats = logStats;
        }

        public virtual string StatString
        {
            get
            {
                return inferenceInterface.StatString;
            }
        }

        public virtual void close()
        {
            inferenceInterface.Close();
        }
    }
 
}