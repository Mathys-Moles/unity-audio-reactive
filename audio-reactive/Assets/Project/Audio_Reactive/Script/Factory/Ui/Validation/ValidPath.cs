using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace AudioReactive.Factory
{
    [RequireComponent(typeof(InputField))]
    public class ValidPath : MonoBehaviour
    {
        private bool _IsValid;
        public UnityEvent onValid;
        public UnityEvent onNoValid;
        private InputField _InputField;

        public bool IsValid
        {
            get { return _IsValid; }
            private set
            {
                _IsValid = value;
                if (_IsValid) onValid?.Invoke();
                else onNoValid?.Invoke();
            }
        }
        private void Awake()
        {
            _InputField = GetComponent<InputField>();
            Check(_InputField.text);
        }
        private void OnEnable() => _InputField.onValueChanged.AddListener(Check);
        private void OnDisable() => _InputField.onValueChanged.RemoveListener(Check);
        public void Check(string pNewText) => IsValid = System.IO.Directory.Exists(pNewText);
    }
}