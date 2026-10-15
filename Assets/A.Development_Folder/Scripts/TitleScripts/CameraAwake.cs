using UnityEngine;

public class CameraAwake : MonoBehaviour
{
    void Awake()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.time = 15f;

    }

}
