

using UnityEngine;
public class AudioReactiveManager : MonoBehaviour
{
    #region Singleton
    private AudioReactiveManager _Instance;
    public AudioReactiveManager GetInstance()
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
    private AudioSource _AudioSource;

    private void Awake()
    {
        SetInstance();
        _AudioSource = GetComponent<AudioSource>();
    }
    
}
