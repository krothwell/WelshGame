using UnityEngine;

public class MusicFadeIn : MonoBehaviour {
    private AudioSource audioSource;
    private AudioClip newClip;
    private float fadeInRate = 0f;
    void Update() {
        if (GetComponent<MusicFadeOut>() == null) {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
            audioSource.clip = newClip;
            if (fadeInRate > 0f) {
                audioSource.volume += fadeInRate;
                if (audioSource.volume >= 0.5f) {
                    //if (!audioSource.isPlaying) {
                    //    audioSource.Play();
                    //}
                    Destroy(this);
                }
            }
        }
    }

    public void SetFadeIn(AudioSource aSource, AudioClip nClip, float fiRate = 0.001f) {
        audioSource = aSource;
        fadeInRate = fiRate;
        newClip = nClip;
    }

}
