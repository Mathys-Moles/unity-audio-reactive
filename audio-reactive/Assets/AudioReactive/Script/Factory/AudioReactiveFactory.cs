
using AudioReactive.Factory.Tools;
using AudioReactive.nTrack;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace AudioReactive.Factory
{
    public class AudioReactiveFactory : MonoBehaviour
    {
        #region Singleton
        private static AudioReactiveFactory _Instance;
        public static AudioReactiveFactory GetInstance()
        {
            if (_Instance == null)
            {
                GameObject lG = new GameObject();
                _Instance = lG.AddComponent<AudioReactiveFactory>();
            }
            return _Instance;
        }
        private void SetInstance()
        {
            if (_Instance != null)
            {
                Debug.Log($"an instance of {nameof(AudioReactiveFactory)} already exists");
                Destroy(gameObject);
                return;
            }
            _Instance = this;
        }
        #endregion
        [Header("Canvas")]
        [SerializeField] private Canvas _Titel;
        [SerializeField] private Text _TitelPath;
        [SerializeField] private Text _TitelName;
        [SerializeField] private Canvas _Count;
        [SerializeField] private Text _CountText;
        [SerializeField] private Canvas _Pist;
        [SerializeField] private Slider _PistSlider;
        [SerializeField] private Transform _PistStopper;
        [SerializeField] private AudioSource _PistKeyFeedBack;
        [SerializeField] private Transform _PistKeyContainer;
        [Header("Path")]
        [SerializeField] private Slider _PistKeyPrefab;
        [Header("BPM")]
        [SerializeField] private Toggle _BPMCheck;
        [SerializeField] private Text _BPM;
        [SerializeField] private Toggle _BPMCustomCheck;
        [SerializeField] private Text _BPMStart;
        [SerializeField] private Text _BPMEnd;

        [Header("Energy")]
        [SerializeField] private Toggle _EnergyCheck;
        [SerializeField] private InputField _EnergyMin;
        [SerializeField] private InputField _EnergyMax;

        [Header("Setting")]
        [SerializeField] private int _Delay = 3;
        [SerializeField] private float _KeyFixValue = 0f;
        [SerializeField] private float _KeyMargin = 0.4f;
        [SerializeField] private bool _Debug = false;
        [Header("KeyMap")]
        [SerializeField] private KeyCode _Pause = KeyCode.Space;
        [SerializeField] private KeyCode _PlaceKey = KeyCode.Return;
        [SerializeField] private KeyCode _Save = KeyCode.Escape;

        private AudioSource _AudioSource;

        private int _DelayCount;
        bool Rec = false;
        bool _Switch = false;

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
            InsertAutoKey();
        }

        private void InsertAutoKey()
        {
            InsertBPMKey();
            RMSInsertKey();
        }

        private void InsertBPMKey()
        {
            if (!_BPMCheck.isOn) return;

            float lInterval = 60f / float.Parse(_BPM.text);
            float lStartTime = _BPMCustomCheck.isOn ? float.Parse(_BPMStart.text) : 0f;
            float lEndTime = _BPMCustomCheck.isOn ? float.Parse(_BPMEnd.text) : _AudioSource.clip.length;
            float lTime = lStartTime;
            if(_Debug) Debug.Log("BPM Key :");
            while (lTime <= lEndTime)
            {
                CreatKey(lTime);
                if (_Debug) Debug.Log(lTime);
                lTime += lInterval;
            }
            if (_Debug) Debug.Log("BPM Finish");
        }

        private void RMSInsertKey()
        {
            if (!_EnergyCheck.isOn) return;

            if(!float.TryParse(_EnergyMin.text, out float lMin)) Debug.Log("Min Sample is not float");
            else if (!float.TryParse(_EnergyMin.text, out float lMax)) Debug.Log("Max Sample is not float");
            if (_Debug) Debug.Log("Sample Key :");
            foreach (float lKeys in AudioRhythmAnalyzer.Analyze(_AudioSource.clip, lMin))
            {
                CreatKey(lKeys);
                if (_Debug) Debug.Log(lKeys);
            }
        }

        private void Update()
        {
            if (!Rec) return;

            SetLectorBar();
            PauseButton();
            PlaceKey();
            SaveTrack();
        }
        private void SetLectorBar()
        {
            if (!_Switch)
            {
                _PistSlider.value = _AudioSource.time;
                ReadTick();
            }
            else _AudioSource.time = _PistSlider.value;
        }
        private void ReadTick()
        {
            foreach (Transform lT in _PistKeyContainer)
                if (lT.TryGetComponent(out Slider lSlide) && Mathf.Abs(lSlide.value - _AudioSource.time) <= _KeyMargin && !_PistKeyFeedBack.isPlaying)
                    _PistKeyFeedBack.Play();
        }
        private void PauseButton()
        {
            if (Input.GetKeyDown(_Pause))
            {
                if (!_Switch) _AudioSource.Pause();
                else _AudioSource.Play();
                _PistStopper.gameObject.SetActive(_Switch);
                _Switch = !_Switch;
            }
        }
        private void PlaceKey()
        {
            if (Input.GetKeyDown(_PlaceKey))
            {
                CreatKey(_AudioSource.time - _KeyFixValue);
            }
        }
        private void SaveTrack()
        {
            if (Input.GetKeyDown(_Save))
            {
                Rec = false;
                _AudioSource.Stop();

                Track lTrack = ScriptableObject.CreateInstance<Track>();
                lTrack.name = _TitelName.text;
                lTrack.ticksTime = new float[_PistKeyContainer.childCount + 2];
                lTrack.ticksTime[0] = 0f;
                for (int i = 1; i < lTrack.ticksTime.Length - 1; i++)
                {
                    lTrack.ticksTime[i] = _PistKeyContainer.GetChild(i - 1).GetComponent<Slider>().value;
                }
                lTrack.ticksTime[lTrack.ticksTime.Length - 1] = _AudioSource.clip.length;
                lTrack.Organize();
                AssetDatabase.CreateAsset(lTrack, _TitelPath.text + "/" + _TitelName.text + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("Save is a success");

                _Titel.gameObject.SetActive(true);
                _Pist.gameObject.SetActive(false);
                _TitelName.text = string.Empty;
            }
        }
        private void CreatKey(float pValue)
        {
            Slider lSlide = Instantiate(_PistKeyPrefab, _PistKeyContainer);
            lSlide.maxValue = _AudioSource.clip.length;
            lSlide.value = pValue;
        }
    }
}

