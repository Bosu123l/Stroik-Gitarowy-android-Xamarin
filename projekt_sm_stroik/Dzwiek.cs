using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using System.Threading.Tasks;

namespace Projekt_SM_stroik
{
	public class Dzwiek
	{

		private AudioRecord record;

		private AudioSource audioSource=AudioSource.Mic;
		private  int sampleRateInHz=11025;
		private ChannelIn chanelConfig=ChannelIn.Mono;
		private Encoding audioFormat=Encoding.Pcm16bit;
		private int buffSizeInBytes;
		private double[] audioBuffer;
		private byte[] audioData;
		private AudioTrack audioTrack;
		private int probki;


		public Dzwiek(string path)
		{

			buffSizeInBytes = 65536;//dlugosc tego zalezy od dlugosci nagrania





			record = new AudioRecord(audioSource,sampleRateInHz,chanelConfig,audioFormat,buffSizeInBytes);
			audioData = new byte[buffSizeInBytes];






		}
		public void PlayRecording(byte[] AudioData)
		{
			audioTrack = new AudioTrack (
				// Stream type
				Android.Media.Stream.Music,
				// Frequency
				11025,
				// Mono or stereo
				ChannelConfiguration.Mono,
				// Audio encoding
				Android.Media.Encoding.Pcm16bit,
				// Length of the audio clip.
				AudioData.Length,
				// Mode. Stream or static.
				AudioTrackMode.Stream);

			audioTrack.Play ();

			audioTrack.WriteAsync (AudioData, 0, AudioData.Length);

		}
		public void AudioPlayRelease()
		{
			record.Stop ();
			audioTrack.Stop ();
			audioTrack.Release ();
			record.Release ();
		}

		public byte[] StartRecording()
		{




			record.StartRecording();

			probki = record.Read(audioData, 0, buffSizeInBytes);
			return audioData;

		}

		public void StopRecording()
		{

			record.Stop();
		}

	}
}

