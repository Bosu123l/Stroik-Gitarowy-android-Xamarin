using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

//using BarChart;

namespace Projekt_SM_stroik
{
	public static class FFT
	{
		
		public static double[] FFTNormal(short[] data, bool forward)
		{
			return FFT.FFTNormal(Array.ConvertAll(data, x => (double)x), forward);
		}
		public static double[] FFTNormal(byte[] _data, bool _forward)
		{
			//return FFT.FFTNormal(Array.ConvertAll(data, x => (byte)x), forward);
			//Double[] doubleArray = Array.ConvertAll (dzwiek.StartRecording (), x => (double)x);

			bool forward = _forward;
			Double[] data = Array.ConvertAll (_data, x => (double)x);
			var n = data.Length;

			if ((n & (n - 1)) != 0)
				throw new ArgumentException(
					"data length " + n + " in FFT is not a power of 2");
			n /= 2;

			Reverse(data, n);

			double sign = forward ? B : -B;
			var mmax = 1;
			while (n > mmax)
			{
				var istep = 2 * mmax;
				var theta = sign * Math.PI / mmax;
				double wr = 1, wi = 0;
				var wpr = Math.Cos(theta);
				var wpi = Math.Sin(theta);
				for (var m = 0; m < istep; m += 2)
				{
					for (var k = m; k < 2 * n; k += 2 * istep)
					{
						var j = k + istep;
						var tempr = wr * data[j] - wi * data[j + 1];
						var tempi = wi * data[j] + wr * data[j + 1];
						data[j] = data[k] - tempr;
						data[j + 1] = data[k + 1] - tempi;
						data[k] = data[k] + tempr;
						data[k + 1] = data[k + 1] + tempi;
					}
					var t = wr;
					wr = wr * wpr - wi * wpi;
					wi = wi * wpr + t * wpi;
				}
				mmax = istep;
			}

			//Scale(data, n, forward);

			return data;

		}

		public static double[] FFTNormal(double[] data, bool forward)
		{
			var n = data.Length;

			if ((n & (n - 1)) != 0)
				throw new ArgumentException(
					"data length " + n + " in FFT is not a power of 2");
			n /= 2;

			Reverse(data, n);

			double sign = forward ? B : -B;
			var mmax = 1;
			while (n > mmax)
			{
				var istep = 2 * mmax;
				var theta = sign * Math.PI / mmax;
				double wr = 1, wi = 0;
				var wpr = Math.Cos(theta);
				var wpi = Math.Sin(theta);
				for (var m = 0; m < istep; m += 2)
				{
					for (var k = m; k < 2 * n; k += 2 * istep)
					{
						var j = k + istep;
						var tempr = wr * data[j] - wi * data[j + 1];
						var tempi = wi * data[j] + wr * data[j + 1];
						data[j] = data[k] - tempr;
						data[j + 1] = data[k + 1] - tempi;
						data[k] = data[k] + tempr;
						data[k + 1] = data[k + 1] + tempi;
					}
					var t = wr;
					wr = wr * wpr - wi * wpi;
					wi = wi * wpr + t * wpi;
				}
				mmax = istep;
			}

			//Scale(data, n, forward);

			return data;
		}

		public static int A { get; set; }
		public static int B { get; set; }

		static FFT()
		{
			A = 1;
			B = 1;
		}

		static void Scale(double[] data, int n, bool forward)
		{
			// forward scaling if needed                                                                         
			if ((forward) && (A != 1))
			{
				var scale = Math.Pow(n, (A - 1) / 2.0);
				for (var i = 0; i < data.Length; ++i)
					data[i] *= scale;
			}

			// inverse scaling if needed                                                                         
			if ((!forward) && (A != -1))
			{
				var scale = Math.Pow(n, -(A + 1) / 2.0);
				for (var i = 0; i < data.Length; ++i)
					data[i] *= scale;
			}
		}

		static void Reverse(double[] data, int n)
		{
			int j = 0, k = 0;
			var top = n / 2;
			while (true)
			{
				var t = data[j + 2];
				data[j + 2] = data[k + n];
				data[k + n] = t;
				t = data[j + 3];
				data[j + 3] = data[k + n + 1];
				data[k + n + 1] = t;
				if (j > k)
				{
					t = data[j];
					data[j] = data[k];
					data[k] = t;
					t = data[j + 1];
					data[j + 1] = data[k + 1];
					data[k + 1] = t;

					t = data[j + n + 2];
					data[j + n + 2] = data[k + n + 2];
					data[k + n + 2] = t;
					t = data[j + n + 3];
					data[j + n + 3] = data[k + n + 3];
					data[k + n + 3] = t;
				}

				k += 4;
				if (k >= n)
					break;

				var h = top;
				while (j >= h)
				{
					j -= h;
					h /= 2;
				}
				j += h;
			}
		}
	}
}