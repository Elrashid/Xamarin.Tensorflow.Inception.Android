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
namespace co.elrashid.xam.tf.Incp.and.Env
{

    using Bitmap = Android.Graphics.Bitmap;
	using TextUtils = Android.Text.TextUtils;

	[Serializable]
	public class Size : IComparable<Size>
	{

	  // 1.4 went out with this UID so we'll need to maintain it to preserve pending queries when
	  // upgrading.
	  public const long serialVersionUID = 7689808733290872361L;

	  public readonly int width;
	  public readonly int height;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public Size(final int width, final int height)
	  public Size(int width, int height)
	  {
		this.width = width;
		this.height = height;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public Size(final android.graphics.Bitmap bmp)
	  public Size(Bitmap bmp)
	  {
		this.width = bmp.Width;
		this.height = bmp.Height;
	  }

	  /// <summary>
	  /// Rotate a size by the given number of degrees. </summary>
	  /// <param name="size"> Size to rotate. </param>
	  /// <param name="rotation"> Degrees {0, 90, 180, 270} to rotate the size. </param>
	  /// <returns> Rotated size. </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static Size getRotatedSize(final Size size, final int rotation)
	  public static Size getRotatedSize(Size size, int rotation)
	  {
		if (rotation % 180 != 0)
		{
		  // The phone is portrait, therefore the camera is sideways and frame should be rotated.
		  return new Size(size.height, size.width);
		}
		return size;
	  }

	  public static Size parseFromString(string sizeString)
	  {
		if (TextUtils.IsEmpty(sizeString))
		{
		  return null;
		}

		sizeString = sizeString.Trim();

		// The expected format is "<width>x<height>".
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String[] components = sizeString.split("x");
		string[] components = sizeString.Split("x", true);
		if (components.Length == 2)
		{
		  try
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int width = int.Parse(components[0]);
			int width = int.Parse(components[0]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int height = int.Parse(components[1]);
			int height = int.Parse(components[1]);
			return new Size(width, height);
		  }
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final NumberFormatException e)
		  catch (Java.Lang.NumberFormatException e)
		  {
			return null;
		  }
		}
		else
		{
		  return null;
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static java.util.List<Size> sizeStringToList(final String sizes)
	  public static IList<Size> sizeStringToList(string sizes)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<Size> sizeList = new java.util.ArrayList<Size>();
		IList<Size> sizeList = new List<Size>();
		if (!string.ReferenceEquals(sizes, null))
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String[] pairs = sizes.split(",");
		  string[] pairs = sizes.Split(",", true);
		  foreach (String pair in pairs)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Size size = Size.parseFromString(pair);
			Size size = Size.parseFromString(pair);
			if (size != null)
			{
			  sizeList.Add(size);
			}
		  }
		}
		return sizeList;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static String sizeListToString(final java.util.List<Size> sizes)
	  public static string sizeListToString(IList<Size> sizes)
	  {
		string sizesString = "";
		if (sizes != null && sizes.Count > 0)
		{
		  sizesString = sizes[0].ToString();
		  for (int i = 1; i < sizes.Count; i++)
		  {
			sizesString += "," + sizes[i].ToString();
		  }
		}
		return sizesString;
	  }

	  public float aspectRatio()
	  {
		return (float) width / (float) height;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public int compareTo(final Size other)
	  public virtual int CompareTo(Size other)
	  {
		return width * height - other.width * other.height;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public boolean equals(final Object other)
	  public override bool Equals(object other)
	  {
		if (other == null)
		{
		  return false;
		}

		if (!(other is Size))
		{
		  return false;
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Size otherSize = (Size) other;
		Size otherSize = (Size) other;
		return (width == otherSize.width && height == otherSize.height);
	  }

	  public override int GetHashCode()
	  {
		return width * 32713 + height;
	  }

	  public override string ToString()
	  {
		return dimensionsAsString(width, height);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static final String dimensionsAsString(final int width, final int height)
	  public static string dimensionsAsString(int width, int height)
	  {
		return width + "x" + height;
	  }
	}

}