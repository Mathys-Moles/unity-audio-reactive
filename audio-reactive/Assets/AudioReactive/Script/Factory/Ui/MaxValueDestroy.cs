using UnityEngine;
using UnityEngine.UI;
namespace AudioReactive.Factory.Ui
{
    [RequireComponent(typeof(Slider))]
    public class MaxValueDestroy : MonoBehaviour
    {
        private Slider _Slider;
        private void Awake()
        {
            _Slider = GetComponent<Slider>();
        }
        private void Update()
        {
            if (_Slider.value == _Slider.maxValue || _Slider.value == _Slider.minValue) Destroy(gameObject);
        }
    }
}

