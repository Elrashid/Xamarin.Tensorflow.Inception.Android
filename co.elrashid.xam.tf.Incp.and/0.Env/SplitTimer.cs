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

namespace co.elrashid.xam.tf.Incp.and.Env
{

    using SystemClock = Android.OS.SystemClock;
    	public class SplitTimer
	{
	  private readonly Logger logger;

	  private long lastWallTime;
	  private long lastCpuTime;

	  public SplitTimer(string name)
	  {
		logger = new Logger(name);
		newSplit();
	  }

	  public virtual void newSplit()
	  {
		lastWallTime = SystemClock.UptimeMillis();
		lastCpuTime = SystemClock.CurrentThreadTimeMillis();
	  }

	  public virtual void endSplit(string splitName)
	  {

		long currWallTime = SystemClock.UptimeMillis();

		long currCpuTime = SystemClock.CurrentThreadTimeMillis();

		logger.i("%s: cpu=%dms wall=%dms", splitName, currCpuTime - lastCpuTime, currWallTime - lastWallTime);

		lastWallTime = currWallTime;
		lastCpuTime = currCpuTime;
	  }
	}

}