using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AudioReactive.Factory.Ui
{
    [RequireComponent(typeof(InputField))]
    public class EmptyText : MonoBehaviour
    {
        private bool _IsEmpty;
        private InputField _InputField;
        public UnityEvent onEmpty;
        public UnityEvent onNoEmpty;

        public bool IsEmpty
        {
            get { return _IsEmpty; }
            private set
            {
                _IsEmpty = value;
                if (_IsEmpty) onEmpty?.Invoke();
                else onNoEmpty?.Invoke();
            }
        }
        private void Awake()
        {
            _InputField = GetComponent<InputField>();
            Check(_InputField.text);
        }
        private void OnEnable() => _InputField.onValueChanged.AddListener(Check);
        private void OnDisable() => _InputField.onValueChanged.RemoveListener(Check);
        private void Check(string pNewTexte) => IsEmpty = pNewTexte == String.Empty;
    }
}