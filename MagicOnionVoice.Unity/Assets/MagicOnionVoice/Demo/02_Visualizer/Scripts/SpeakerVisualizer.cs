using MagicOnionExamples.Client.Voice;
using UnityEngine;

namespace MagicOnionExamples.Demo.Voice
{
    public class SpeakerVisualizer : MonoBehaviour
    {
        [SerializeField] Speaker speaker;
        [SerializeField] float VolumeCoefficient = 2.0f;

        void Update()
        {
            float scale = speaker.GetAveragedVolume() * VolumeCoefficient;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
