using AudioReactive.Component;
using UnityEngine;

namespace AudioReactive.Demo
{
    public class AudioReactiveDemo : MonoBehaviour
    {
        [SerializeField] private TrackReader _Track;
        [SerializeField] private Vector3 _TargetScale;

        private Vector3 _InitScale;

        private void Awake() => _InitScale = transform.localScale;

        private void OnEnable()
        {
            _Track.OnTickRatio += DoRatio;
            _Track.OnTick += DoTick;
        }
        private void OnDisable()
        {
            _Track.OnTickRatio -= DoRatio;
            _Track.OnTick -= DoTick;
        }

        private void DoRatio(float pTime)
        {
            transform.localScale = Vector3.Lerp(_InitScale, _TargetScale, pTime);
        }
        private void DoTick()
        {
            //Behavior on tick
        }
    }
}
