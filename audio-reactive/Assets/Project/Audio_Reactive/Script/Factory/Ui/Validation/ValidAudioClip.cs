using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AudioReactive.Factory
{
    public class ValidAudioClip : MonoBehaviour
    {
        [SerializeField] private AudioSource _Source;
        [SerializeField] private Text _Texte;


        private bool _IsValid;
        public UnityEvent onValid;
        public UnityEvent onNoValid;

        public bool IsValid
        {
            get { return _IsValid; }
            private set
            {
                _IsValid = value;
                if (_IsValid)
                {
                    _Texte.text = _Source.clip.name;
                    onValid?.Invoke();
                }
                else onNoValid?.Invoke();
            }
        }
        private void Update()
        {
            IsValid = _Source.clip != null;
        }

        
    }
}

