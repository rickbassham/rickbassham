using System;
using System.Collections.Generic;
using System.Text;

using SpeechLib;
using EricOulashin;
using Mike.Rules;

namespace Borgify
{
    class Program
    {
        static void Main(string[] args)
        {
            string tempPath = "temp.wav";

            SpVoice voice = new SpVoice();

            SpFileStream fileStream = new SpFileStream();

            fileStream.Open(tempPath, SpeechStreamFileMode.SSFMCreateForWrite, false);

            voice.AudioOutputStream = fileStream;

            voice.Speak("This is a test of the emergency broadcast system.", SpeechVoiceSpeakFlags.SVSFDefault);
            voice.WaitUntilDone(-1);

            fileStream.Close();

            voice.AudioOutputStream = null;


            WAVFile input = new WAVFile();

            input.Open(tempPath, WAVFile.WAVFileMode.READ);

            int start = 0;
            int max = 25;

            float[] pitchShifts = new float[max];
            string[] tempFiles = new string[max];
            WAVFile[] tempWavs = new WAVFile[max];

            for (int i = start; i < max; i++)
            {
                pitchShifts[i] = (0.70f * i / max) + 0.5f;
                tempFiles[i] = string.Format("temp{0}.wav", i);
                tempWavs[i] = new WAVFile();
                tempWavs[i].Create(tempFiles[i], input.IsStereo, input.SampleRateHz, input.BitsPerSample);
            }

            while (input.NumSamplesRemaining > 0)
            {
                short sample = input.GetNextSampleAs16Bit();

                float sampleVal = (float)sample / 32768f;

                for (int i = start; i < max; i++)
                {
                    float[] indata = new float[] { sampleVal };

                    PitchShifter.PitchShift(pitchShifts[i], 1, input.SampleRateHz, indata);

                    tempWavs[i].AddSample_16bit((short)(indata[0] * 32768));
                }
            }

            for (int i = start; i < max; i++)
            {
                tempWavs[i].Close();
            }

            WAVFile.MergeAudioFiles(tempFiles, "output.wav", "temp");
        }
    }
}
