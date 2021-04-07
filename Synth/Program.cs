using System;
using System.IO;
using System.Media;

namespace Synth
{
    public class Program
    {
        private const int SAMPLE_RATE = 44100;
        private const short BITS_PER_SAMPLE = 16;

        public static void Main(string[] args)
        {
            short[] wave = new short[SAMPLE_RATE];
            float frequency = 440f;
            int samplesPerWaveLength = (int)(SAMPLE_RATE / frequency);
            short ampStep = (short)((short.MaxValue * 2) / samplesPerWaveLength);
            short tempSample;

            // Saw
            //tempSample = -short.MaxValue;

            // Triangle
            //tempSample = -short.MaxValue;

            // Noise
            //Random random = new Random();

            for (int i = 0; i < SAMPLE_RATE; i++)
            {
                // Sine
                wave[i] = Convert.ToInt16(short.MaxValue * Math.Sin(Math.PI * 2 * frequency / SAMPLE_RATE * i));

                // Square
                //wave[i] = Convert.ToInt16(short.MaxValue * Math.Sign(Math.Sin(Math.PI * 2 * frequency / SAMPLE_RATE * i)));

                // Saw
                //for (int j = 0; j < samplesPerWaveLength && i < SAMPLE_RATE; j++)
                //{
                //    tempSample += ampStep;
                //    wave[i++] = Convert.ToInt16(tempSample);
                //}
                //i--;

                // Triangle
                //if (Math.Abs(tempSample + ampStep) > short.MaxValue)
                //{
                //    ampStep = (short)-ampStep;
                //}
                //tempSample += ampStep;
                //wave[i] = Convert.ToInt16(tempSample);

                // Noise
                //wave[i] = (short)random.Next(-short.MaxValue, short.MaxValue);
            }
            byte[] binaryWave = new byte[SAMPLE_RATE * sizeof(short)];
            Buffer.BlockCopy(wave, 0, binaryWave, 0, wave.Length * sizeof(short));
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                short blockAlign = BITS_PER_SAMPLE / 8;
                int subChunkTwoSize = SAMPLE_RATE * blockAlign;
                binaryWriter.Write(new[] { 'R', 'I', 'F', 'F' });
                binaryWriter.Write(36 + subChunkTwoSize);
                binaryWriter.Write(new[] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                binaryWriter.Write(16);
                binaryWriter.Write((short)1);
                binaryWriter.Write((short)1);
                binaryWriter.Write(SAMPLE_RATE);
                binaryWriter.Write(SAMPLE_RATE * blockAlign);
                binaryWriter.Write(blockAlign);
                binaryWriter.Write(BITS_PER_SAMPLE);
                binaryWriter.Write(new[] { 'd', 'a', 't', 'a' });
                binaryWriter.Write(subChunkTwoSize);
                binaryWriter.Write(binaryWave);
                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();
                Console.ReadLine();
            }
        }
    }
}
