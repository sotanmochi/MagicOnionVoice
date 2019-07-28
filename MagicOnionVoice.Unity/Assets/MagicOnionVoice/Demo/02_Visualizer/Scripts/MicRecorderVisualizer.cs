using MagicOnionExamples.Client.Voice;
using UnityEngine;

namespace MagicOnionExamples.Demo.Voice
{
    public class MicRecorderVisualizer : MonoBehaviour
    {
        [SerializeField] MicRecorder recorder;
        [SerializeField] float VolumeCoefficient = 2.0f;

        void Update()
        {
            float scale = recorder.GetAveragedVolume() * VolumeCoefficient;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
