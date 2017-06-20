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
using System;
using System.Collections.Generic;
namespace co.elrashid.xam.tf.Incp.and.Env
{
    using Java.Lang;
    using Log = Android.Util.Log;
    public sealed class Logger
	{
	  private const string DEFAULT_TAG = "tensorflow";
	  private static readonly int DEFAULT_MIN_LOG_LEVEL =(int) Android.Util.LogPriority.Debug ;

	  // Classes to be ignored when examining the stack trace
	  private static readonly ISet<string> IGNORED_CLASS_NAMES;

	  static Logger()
	  {
		IGNORED_CLASS_NAMES = new HashSet<string>();
		IGNORED_CLASS_NAMES.Add("dalvik.system.VMStack");
		IGNORED_CLASS_NAMES.Add("java.lang.Thread");

		IGNORED_CLASS_NAMES.Add(typeof(Logger).FullName);
	  }

	  private readonly string tag;
	  private readonly string messagePrefix;
	  private int minLogLevel = DEFAULT_MIN_LOG_LEVEL;

	  public Logger(Type clazz) : this(clazz.Name)
	  {
	  }


	  public Logger(string messagePrefix) : this(DEFAULT_TAG, messagePrefix)
	  {
	  }


	  public Logger(string tag, string messagePrefix)
	  {
		this.tag = tag;
		string prefix = string.ReferenceEquals(messagePrefix, null) ? CallerSimpleName : messagePrefix;
		this.messagePrefix = (prefix.Length > 0) ? prefix + ": " : prefix;
	  }

	  public Logger() : this(DEFAULT_TAG, null)
	  {
	  }


	  public Logger(int minLogLevel) : this(DEFAULT_TAG, null)
	  {
		this.minLogLevel = minLogLevel;
	  }

	  public int MinLogLevel
	  {
		  set
		  {
			this.minLogLevel = value;
		  }
	  }

	  public bool isLoggable(int logLevel)
	  {
		return logLevel >= minLogLevel || Log.IsLoggable(tag, (Android.Util.LogPriority)logLevel);
	  }


	  private static string CallerSimpleName
	  {
		  get
		  {
		
			StackTraceElement[] stackTrace = Thread.CurrentThread().GetStackTrace();
    
			foreach (StackTraceElement elem in stackTrace)
			{
	
			  string className = elem.ClassName;
			  if (!IGNORED_CLASS_NAMES.Contains(className))
			  {
				string[] classParts = className.Split("\\.", true);
				return classParts[classParts.Length - 1];
			  }
			}
    
			return typeof(Logger).Name;
		  }
	  }

	  private string toMessage(string format, params object[] args)
	  {
		return messagePrefix + (args.Length > 0 ? string.Format(format, args) : format);
	  }

	  public void v(string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Verbose))
		{
		  Log.Verbose(tag, toMessage(format, args));
		}
	  }

	  public void v(Exception t, string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Verbose))
		{
		  Log.Verbose(tag, toMessage(format, args), t);
		}
	  }

	  public void d(string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Debug))
		{
		  Log.Debug(tag, toMessage(format, args));
		}
	  }

	  public void d(Exception t, string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Debug))
		{
		  Log.Debug(tag, toMessage(format, args), t);
		}
	  }

	  public void i(string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Info))
		{
		  Log.Info(tag, toMessage(format, args));
		}
	  }

	  public void i(Exception t, string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Info))
		{
		  Log.Info(tag, toMessage(format, args), t);
		}
	  }

	  public void w(string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Warn))
		{
		  Log.Warn(tag, toMessage(format, args));
		}
	  }

	  public void w(Exception t, string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Warn))
		{
		  Log.Warn(tag, toMessage(format, args), t);
		}
	  }

	  public void e(string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Error))
		{
		  Log.Error(tag, toMessage(format, args));
		}
	  }

	  public void e(Exception t, string format, params object[] args)
	  {
		if (isLoggable((int)Android.Util.LogPriority.Error))
		{
		  Log.Error(tag, toMessage(format, args), t);
		}
	  }
	}

}