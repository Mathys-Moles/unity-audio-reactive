using System;
using UnityEngine;
namespace AudioReactive.nTrack
{
    [CreateAssetMenu(fileName = "AudioReactiveTrack", menuName = "Audio/ReactiveAudio/Track")]
    public class Track : ScriptableObject
    {
        [SerializeField] public float[] ticksTime;
        [Space(5)]
        [SerializeField] public float _Delay;

        [OnEditButton]
        private void SlideDelay()
        {
            for (int i = 1; i < ticksTime.Length - 1; i++)
            {
                ticksTime[i] += _Delay;
            }
        }
        [OnEditButton]
        public void Organize() => Array.Sort(ticksTime);


    }
}

