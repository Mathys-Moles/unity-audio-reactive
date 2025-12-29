using UnityEngine;
namespace AudioReactive.Component
{
    [AddComponentMenu("Audio/Audio Reactive/Audio Reactive Controller")]
    public class AudioReactiveController : MonoBehaviour
    {
        [SerializeField] private AudioSource _AudioSource;
        [SerializeField] private TrackReader[] _LinkTrack;

        [SerializeField] private bool _PlayOnAwake;

        public bool isPlaying { get; private set; }

        private float _Time;
        public float Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
        private void Awake()
        {
            if (_PlayOnAwake) Play();
        }

        [OnEditButton]
        public void Play()
        {
            isPlaying = true;
            _AudioSource.Play();
        }
        [OnEditButton]
        public void Pause()
        {
            isPlaying = false;
            _AudioSource.Pause();
        }
        [OnEditButton]
        public void Stop()
        {
            isPlaying = false;
            _AudioSource.Stop();
            foreach (TrackReader lTrack in _LinkTrack) lTrack.Reset();
        }

        private void Update()
        {
            if (!isPlaying) return;
            foreach(TrackReader lTrack in _LinkTrack) lTrack.Read(_AudioSource.time);
        }
    }
}


