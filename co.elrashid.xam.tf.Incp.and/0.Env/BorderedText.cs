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

namespace co.elrashid.xam.tf.Incp.and.Env
{

    using Canvas = Android.Graphics.Canvas;
	using Color = Android.Graphics.Color;
	using Paint = Android.Graphics.Paint;
	using Align = Android.Graphics.Paint.Align;
	using Style = Android.Graphics.Paint.Style;
	using Rect = Android.Graphics.Rect;
	using Typeface = Android.Graphics.Typeface;

	public class BorderedText
	{
	  private readonly Paint interiorPaint;
	  private readonly Paint exteriorPaint;

	  private readonly float textSize;

	  public BorderedText(float textSize) : this(Color.White, Color.Black, textSize)
	  {
	  }
        
	  public BorderedText(int interiorColor, int exteriorColor, float textSize)
	  {
		interiorPaint = new Paint();
		interiorPaint.TextSize = textSize;
		interiorPaint.Color = new Color(interiorColor);
		interiorPaint.SetStyle(Paint.Style.Fill);
		interiorPaint.AntiAlias = false;
		interiorPaint.Alpha = 255;
            
        exteriorPaint = new Paint();
		exteriorPaint.TextSize = textSize;
		exteriorPaint.Color = new Color(exteriorColor);
		exteriorPaint.SetStyle(Paint.Style.FillAndStroke);  
		exteriorPaint.StrokeWidth = textSize / 8;
		exteriorPaint.AntiAlias = false;
		exteriorPaint.Alpha = 255;

		this.textSize = textSize;
	  }

	  public virtual Typeface Typeface
	  {
		  set
		  {
			interiorPaint.SetTypeface( value);
			exteriorPaint.SetTypeface( value);
		  }
	  }

	  public virtual void drawText(Canvas canvas, float posX, float posY, string text)
	  {
		canvas.DrawText(text, posX, posY, exteriorPaint);
		canvas.DrawText(text, posX, posY, interiorPaint);
	  }

	  public virtual void drawLines(Canvas canvas, float posX, float posY, List<string> lines)
	  {
		int lineNum = 0;
		foreach (string line in lines)
		{
		  drawText(canvas, posX, posY - TextSize * (lines.Count - lineNum - 1), line);
		  ++lineNum;
		}
	  }

	  public virtual int InteriorColor
	  {
		  set
		  {
                

            interiorPaint.Color = new Color(value); ;
		  }
	  }

	  public virtual int ExteriorColor
	  {
		  set
		  {
			exteriorPaint.Color = new Color(value); 
		  }
	  }

	  public virtual float TextSize
	  {
		  get
		  {
			return textSize;
		  }
	  }

	  public virtual int Alpha
	  {
		  set
		  {
			interiorPaint.Alpha = value;
			exteriorPaint.Alpha = value;
		  }
	  }

	  public virtual void getTextBounds(string line, int index, int count, Rect lineBounds)
	  {
		interiorPaint.GetTextBounds(line, index, count, lineBounds);
	  }

	  public virtual Paint.Align TextAlign
	  {
		  set
		  {
			interiorPaint.TextAlign = value;
			exteriorPaint.TextAlign = value;
		  }
	  }
	}

}