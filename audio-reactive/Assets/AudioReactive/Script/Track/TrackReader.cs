using AudioReactive.nTrack;
using System;
using UnityEngine;
namespace AudioReactive.Component
{
    [AddComponentMenu("Audio/Audio Reactive/Track Reader")]
    public class TrackReader : MonoBehaviour
    {
        [SerializeField] private Track _Track;

        private int _NextIndex = 1;

        public Action OnTick;
        public Action<float> OnTickRatio;

        public void Read(float pTime)
        {
            if (pTime < 0.5f) Reset();
            if (_Track.ticksTime[_NextIndex] < pTime)
            {
                OnTick?.Invoke();
                _NextIndex++;
            }
            float lRatio = (pTime - _Track.ticksTime[_NextIndex - 1]) / (_Track.ticksTime[_NextIndex] - _Track.ticksTime[_NextIndex - 1]);
            OnTickRatio?.Invoke(lRatio);
        }
        public void Reset()
        {
            _NextIndex = 1;
        }

    }
}

