using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public AudioClip _musicGamePrincipal;
    // Start is called before the first frame update
    void Start()
    {
        _musicGamePrincipal = GetComponent<AudioClip>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
