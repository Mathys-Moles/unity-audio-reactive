using UnityEngine;
using UnityEngine.UI;

namespace AudioReactive.Factory.Ui
{
    [RequireComponent(typeof(Button))]
    public class RecButton : MonoBehaviour
    {
        [Header("Validation")]
        [SerializeField] private ValidAudioClip _VAudio;
        [SerializeField] private EmptyText _VName;
        [SerializeField] private EmptyText _VPath;
      
     
        private Button _Button;


        private void Awake()
        {
            _Button = GetComponent<Button>();
        }
        private void OnEnable()
        {
            _Button.onClick.AddListener(Check);
        }
        private void OnDisable()
        {
            _Button.onClick.RemoveListener(Check);
        }
        private void Check()
        {
            if (!_VAudio.IsValid || _VPath.IsEmpty || _VName.IsEmpty) return;
            Debug.Log("check are okay");
         

            AudioReactiveFactory.GetInstance().StartDelay();
        }

    }
}

