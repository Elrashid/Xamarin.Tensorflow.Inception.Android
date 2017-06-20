//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2


/* Copyright 2015 The TensorFlow Authors. All Rights Reserved.

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
    using Paint = Android.Graphics.Paint;
    using AttributeSet = Android.Util.IAttributeSet;
    using TypedValue = Android.Util.TypedValue;
    using View = Android.Views.View;
    using Android.Graphics;
    using Android.Util;
    using Tensorflow;

    public class RecognitionScoreView : View, ResultsView
	{
	  private const float TEXT_SIZE_DIP = 24;
	  private IList<Classifier_Recognition> results;
	  private readonly float textSizePx;
	  private readonly Paint fgPaint;
	  private readonly Paint bgPaint;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public RecognitionScoreView(final android.content.Context context, final android.util.AttributeSet set)
	  public RecognitionScoreView(Context context, AttributeSet set) : base(context, set)
	  {
            
		textSizePx = TypedValue.ApplyDimension(ComplexUnitType.Dip, TEXT_SIZE_DIP, Resources.DisplayMetrics);
		fgPaint = new Paint();
		fgPaint.TextSize = textSizePx;

		bgPaint = new Paint();
            bgPaint.Color = new Android.Graphics.Color(Color.Red);
            //TODO:5
          //  bgPaint.Color =  Color.ParseColor("0xcc4285f4");
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public void setResults(final java.util.List<org.tensorflow.demo.Classifier_Recognition> results)
	  public virtual IList<Classifier_Recognition> Results
	  {
		  set
		  {
			this.results = value;
			PostInvalidate();
		  }
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public void onDraw(final android.graphics.Canvas canvas)
	  protected override void OnDraw(Canvas canvas)
	  {
		const int x = 10;
		int y = (int)(fgPaint.TextSize * 1.5f);

		canvas.DrawPaint(bgPaint);

		if (results != null)
		{
		  foreach (Classifier_Recognition recog in results)
		  {
			canvas.DrawText(recog.Title + ": " + recog.Confidence, x, y, fgPaint);
			y += (int)(fgPaint.TextSize * 1.5f);
		  }
		}
	  }
	}

}