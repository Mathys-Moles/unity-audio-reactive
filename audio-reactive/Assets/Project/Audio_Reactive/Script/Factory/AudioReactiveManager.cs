
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace AudioReactive.Factory
{
    public class AudioReactiveManager : MonoBehaviour
    {
        #region Singleton
        private static AudioReactiveManager _Instance;
        public static AudioReactiveManager GetInstance()
        {
            if (_Instance == null)
            {
                GameObject lG = new GameObject();
                _Instance = lG.AddComponent<AudioReactiveManager>();
            }
            return _Instance;
        }
        private void SetInstance()
        {
            if (_Instance != null)
            {
                Debug.Log($"an instance of {nameof(AudioReactiveManager)} already exists");
                Destroy(gameObject);
                return;
            }
            _Instance = this;
        }
        #endregion
        [Header("Canvas")]
        [SerializeField] private Canvas _Titel;
        [SerializeField] private Canvas _Count;
        [SerializeField] private Text _CountText;
        [SerializeField] private Canvas _Pist;
        [SerializeField] private Slider _PistSlider;
        [SerializeField] private Transform _PistStopper;
        [SerializeField] private AudioSource _PistKeyFeedBack;
        [SerializeField] private Transform _PistKeyContainer;
        [Header("Path")]
        [SerializeField] private Slider _PistKeyPrefab;
        [Header("Setting")]
        [SerializeField] private int _Delay = 3;
        [SerializeField] private float _KeyFixValue = 0f;
        [SerializeField] private float _KeyMargin = 0.4f;


        private AudioSource _AudioSource;

        private int _DelayCount;
        private void Awake()
        {
            SetInstance();
            _AudioSource = GetComponent<AudioSource>();
        }
        public void StartDelay()
        {
            _Titel.gameObject.SetActive(false);
            _Count.gameObject.SetActive(true);
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            _DelayCount = _Delay;
            _CountText.text = _DelayCount.ToString();
            while (_DelayCount >= 0)
            {
                yield return new WaitForSeconds(1f);
                _DelayCount--;
                _CountText.text = _DelayCount.ToString();
            }
            StartRec();
        }
        private void StartRec()
        {
            _AudioSource.Play();
            _Pist.gameObject.SetActive(true);
            _Count.gameObject.SetActive(false);

            Rec = true;
            _PistSlider.maxValue = _AudioSource.clip.length;
        }
        bool Rec = false;
        bool _Switch = false;

        private void Update()
        {
            if (!Rec) return;

            if (!_Switch)
            {
                _PistSlider.value = _AudioSource.time;
                Debug.Log(_PistSlider.value);
                foreach (Transform lT in _PistKeyContainer)
                    if (lT.TryGetComponent(out Slider lSlide) && Mathf.Abs(lSlide.value - _AudioSource.time) <= _KeyMargin && !_PistKeyFeedBack.isPlaying) 
                        _PistKeyFeedBack.Play();
            }
            else _AudioSource.time = _PistSlider.value;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!_Switch) _AudioSource.Pause();
                else _AudioSource.Play();
                _PistStopper.gameObject.SetActive(_Switch);
                _Switch = !_Switch;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                Slider lSlide = Instantiate(_PistKeyPrefab, _PistKeyContainer);
                lSlide.maxValue = _AudioSource.clip.length;
                lSlide.value = _AudioSource.time - _KeyFixValue;
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                Rec = false;
                _AudioSource.Stop();


            }
        }
    }
}

