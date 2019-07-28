using System;
using UnityEngine;
using UnityOpus;

namespace MagicOnionExamples.Client.Voice
{
    [RequireComponent(typeof(AudioSource))]
    public class Speaker : MonoBehaviour
    {
        const NumChannels channels = NumChannels.Mono;
        const SamplingFrequency frequency = SamplingFrequency.Frequency_48000;
        
        int audioClipLength = (int)frequency; // 1.0[sec]
        AudioSource source;
        int samplePos = 0;
        float[] audioClipData;

        Decoder decoder;
        readonly float[] pcmBuffer = new float[Decoder.maximumPacketDuration * (int)channels];

        void OnEnable()
        {
            decoder = new Decoder(SamplingFrequency.Frequency_48000, NumChannels.Mono);

            source = GetComponent<AudioSource>();
            source.clip = AudioClip.Create("AudioStreamPlayer", audioClipLength, (int)channels, (int)frequency, false);
            source.loop = true;
        }

        void OnDisable()
        {
            source.Stop();
            decoder.Dispose();
            decoder = null;
        }

        public void ReceiveBytes(byte[] encodedData, int length)
        {
            if (decoder != null)
            {
                var pcmLength = decoder.Decode(encodedData, length, pcmBuffer);

                if (audioClipData == null || audioClipData.Length != pcmLength)
                {
                    // assume that pcmLength will not change.
                    audioClipData = new float[pcmLength];
                }
                Array.Copy(pcmBuffer, audioClipData, pcmLength);
                source.clip.SetData(audioClipData, samplePos);
                samplePos += pcmLength;
                if (!source.isPlaying && samplePos > audioClipLength / 2)
                {
                    source.Play();
                }
                samplePos %= audioClipLength;
            }
        }

        public float GetAveragedVolume()
        { 
            int size = 1024;
            float[] data = new float[size];
            float a = 0;
            source.GetOutputData(data,0);
            foreach(float s in data)
            {
                a += Mathf.Abs(s);
            }
            return a / size;
        }
    }
}
