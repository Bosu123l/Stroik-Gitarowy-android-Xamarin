using System;

using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;
using System.Numerics;
namespace Projekt_SM_stroik
{
	[Activity (Label = "Projekt_SM_stroik", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int licznik=0;
		private Complex[] FFTprobe=new Complex[65536];
		private Dzwiek dzwiek;
		private List<byte[]> listaProbek=new List<byte[]>();
		private bool wykonuj=false;

		private byte[] tablicaProbek = new byte[65536];
		private double[] TablicaFFT = new double[65536];


			


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			dzwiek = new Dzwiek("/sdcard/test.3gpp");
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval =1;

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button buttonStart = FindViewById<Button> (Resource.Id.StartButton);
			Button OdtworzButton = FindViewById<Button> (Resource.Id.OdtworzButton);
			SeekBar sekbar =  FindViewById<SeekBar> (Resource.Id.Dlugosc);
			Button button= FindViewById<Button> (Resource.Id.button);

			OdtworzButton.Click += delegate {
				timer.Enabled = false;

				wykonuj=false;

				var documents=Android.OS.Environment.ExternalStorageDirectory.Path;
				var filename=System.IO.Path.Combine(documents,"probki.csv");
				var filename2=System.IO.Path.Combine(documents,"probki2.csv");



				File.WriteAllLines(filename2,Array.ConvertAll(TablicaFFT,element=>element.ToString()));
				File.WriteAllLines(filename,Array.ConvertAll(tablicaProbek,element=>element.ToString()));


				dzwiek.AudioPlayRelease();
				listaProbek.Clear();
			};
		
			buttonStart.Click += delegate {

				timer.Enabled = true;
				wykonuj=true;





				tablicaProbek=dzwiek.StartRecording();


				Complex[] buff=new Complex[65536];

				for(int i=0;i<tablicaProbek.Length;i++)
				{
					buff[i]=new Complex(tablicaProbek[i],0);
				}
				FFTprobe=buff;
				//TablicaFFT=FFT.FFTNormal(tablicaProbek,true);
			

				dzwiek.PlayRecording(tablicaProbek);
				
			};
			sekbar.KeyPress  += delegate {
				licznik=sekbar.Progress;
				button.Text=String.Format("{0}",licznik);
		
				
			};
		}
	}
}


