using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour {

    public AudioSource source;

    public void PlayHyenaLaugh()
    {
        if(!source.isPlaying)
        {
            source.Play();
        }
    }
}
