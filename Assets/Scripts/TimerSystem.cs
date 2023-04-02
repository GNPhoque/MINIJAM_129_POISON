using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimerSystem
{
	public static Stopwatch sw;

	public static void StartTimer()
	{
		sw = new Stopwatch();
		sw.Start();
	}

	public static void StopTimer()
	{
		sw?.Stop();
	}

	public static void ResetTimer()
	{
		sw = null;
	}

	public static string GetFullTime()
	{
		TimeSpan ts = sw.Elapsed;
		return $"\n{ts.Minutes}:{ts.Seconds}:{ts.Milliseconds}";
	}

	public static string GetTime()
	{
		TimeSpan ts = sw.Elapsed;
		return $"{ts.ToString("mm\\:ss\\:ff")}";
	}

	public static int GetTimeScore()
	{
		if (sw == null) return 0;
		TimeSpan ts = sw.Elapsed;
		return int.Parse(ts.ToString("mmssff"));
	}
}
