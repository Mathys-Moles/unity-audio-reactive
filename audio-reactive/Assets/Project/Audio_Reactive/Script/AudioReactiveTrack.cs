using UnityEngine;

[CreateAssetMenu(fileName = "AudioReactiveTrack", menuName = "Audio/ReactiveAudio/Track")]
public class AudioReactiveTrack : ScriptableObject
{
   [SerializeField] public float[] _TickTime;
}
