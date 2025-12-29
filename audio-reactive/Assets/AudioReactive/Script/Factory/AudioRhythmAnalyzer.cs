using System.Collections.Generic;
using UnityEngine;
namespace AudioReactive.Factory.Tools
{
    /// <summary>
    /// BETA
    /// </summary>
    public static class AudioRhythmAnalyzer
    {
        public static float[] Analyze(
            AudioClip clip,
            float threshold = 0.3f,
            float minSpacing = 0.1f,
            int windowSize = 1024
        )
        {
            if (clip == null)
            {
                return new float[0];
            }

            int channels = clip.channels;
            int frequency = clip.frequency;
            int totalSamples = clip.samples;

            float[] allSamples = new float[totalSamples * channels];
            clip.GetData(allSamples, 0);

            List<float> energies = new List<float>();
            List<float> times = new List<float>();

            for (int i = 0; i < totalSamples; i += windowSize / 2)
            {
                float energy = CalculateEnergy(allSamples, i, windowSize, channels, totalSamples);
                energies.Add(energy);
                times.Add((float)i / frequency);
            }

            if (energies.Count == 0)
            {
                return new float[0];
            }

            float avgEnergy = 0f;
            float maxEnergy = 0f;
            foreach (float e in energies)
            {
                avgEnergy += e;
                if (e > maxEnergy) maxEnergy = e;
            }
            avgEnergy /= energies.Count;

            List<float> detectedBeats = new List<float>();
            float lastBeatTime = -999f;
            float dynamicThreshold = avgEnergy + (maxEnergy - avgEnergy) * threshold;

            for (int i = 1; i < energies.Count - 1; i++)
            {
                float currentEnergy = energies[i];
                float prevEnergy = energies[i - 1];
                float nextEnergy = energies[i + 1];

                if (currentEnergy > dynamicThreshold &&
                    currentEnergy > prevEnergy &&
                    currentEnergy >= nextEnergy)
                {
                    float beatTime = times[i];

                    if (beatTime - lastBeatTime >= minSpacing)
                    {
                        detectedBeats.Add(beatTime);
                        lastBeatTime = beatTime;
                    }
                }
            }

            return detectedBeats.ToArray();
        }

        private static float CalculateEnergy(float[] samples, int startSample, int windowSize, int channels, int totalSamples)
        {
            float sum = 0f;
            int count = 0;

            for (int i = 0; i < windowSize; i++)
            {
                int sampleIndex = (startSample + i) * channels;

                if (startSample + i >= totalSamples || sampleIndex >= samples.Length)
                    break;

                float maxVal = 0f;
                for (int c = 0; c < channels; c++)
                {
                    if (sampleIndex + c < samples.Length)
                    {
                        float val = Mathf.Abs(samples[sampleIndex + c]);
                        if (val > maxVal) maxVal = val;
                    }
                }

                sum += maxVal * maxVal;
                count++;
            }

            return count > 0 ? Mathf.Sqrt(sum / count) : 0f;
        }
    }
}
