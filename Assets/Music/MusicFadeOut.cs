using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeOut : MonoBehaviour {
    private AudioSource audioSource;
    private float fadeOutRate = 0f;
    void Update() {
        if (fadeOutRate > 0f) {
            audioSource.volume -= fadeOutRate;
            if (audioSource.volume <= 0f) {
                audioSource.clip = null;
                Destroy(this);
            }
        }
    }

    public void SetFadeOut(AudioSource aSource, float foRate = 0.001f) {
        audioSource = aSource;
        fadeOutRate = foRate;
    }

}
