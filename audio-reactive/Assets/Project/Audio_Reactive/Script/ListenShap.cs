using UnityEngine;

public class ListenShap : MonoBehaviour
{
    bool state;
    public void OnTick()
    {
        GetComponent<Renderer>().material.color = state? Color.blue : Color.red;
        state = !state;
    }

}
