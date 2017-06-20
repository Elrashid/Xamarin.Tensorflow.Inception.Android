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

using System.Collections.Generic;
namespace co.elrashid.xam.tf.Incp.and.CameraTensorflowIntegration
{

    using Context = Android.Content.Context;
    using Canvas = Android.Graphics.Canvas;
    using AttributeSet = Android.Util.IAttributeSet;
    using View = Android.Views.View;
    using Android.Graphics;
    using co.elrashid.xam.tf.Incp.and.CameraTensorflowIntegration;

    /// <summary>
    /// A simple View providing a render callback to other classes.
    /// </summary>
    public class OverlayView : View
    {
        private readonly  List<IDrawCallback> callbacks = new List<IDrawCallback>();

        public OverlayView(Context context, AttributeSet attrs) : base(context, attrs)
        {
        }

     

        public virtual void addCallback(IDrawCallback callback)
        {
            callbacks.Add(callback);
        }

        protected override void OnDraw(Canvas canvas)
        {
            lock (this)
            {
                foreach (IDrawCallback callback in callbacks)
                {
                    callback.drawCallback(canvas);
                }
            }
        }
    }

}