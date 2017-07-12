using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    private bool musicLocked;
    public bool MusicLocked {
        get { return musicLocked; }
        set { musicLocked = value; }
    }
    public AudioClip startingMusic;
    static MusicPlayer instance = null;
    AudioSource audioSource;
	// Use this for initialization

    void Awake () {
        
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Music player Awake" + GetInstanceID());
        if (instance != null)
        {
            print("Duplicate music player destroyed while playing clip: " + audioSource.clip);
            instance.TransitionMusic(startingMusic);
            Destroy(gameObject);
            Destroy(this);  
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

    }
    void Start () {
        Debug.Log("Music player Start" + GetInstanceID());
	}

    //void DestroyMeIfMusicPlayerExists() {
    //    MusicPlayer[] MPs = FindObjectsOfType<MusicPlayer>();
    //    foreach (MusicPlayer MP in MPs) {
    //        if (MPs.Length > 1) {
    //            if (MP.GetComponent<AudioSource>().clip == null) {
    //                Destroy(MP.gameObject);
    //            }
    //            else if (!MP.GetComponent<AudioSource>().isPlaying) {
    //                Destroy(MP.gameObject);
    //            }
    //        }
    //    }
        
    //}
    
    public void FadeOut(float fadeOutRate) {
        gameObject.AddComponent<MusicFadeOut>();
        MusicFadeOut musicFadeOut = FindObjectOfType<MusicFadeOut>();
        musicFadeOut.SetFadeOut(audioSource, fadeOutRate);
    }

    public void FadeIn(float fadeInRate, AudioClip newClip) {
        gameObject.AddComponent<MusicFadeIn>();
        MusicFadeIn musicFadeIn = FindObjectOfType<MusicFadeIn>();
        musicFadeIn.SetFadeIn(audioSource, newClip, fadeInRate);
    }

    public void TransitionMusic(AudioClip newClip, float fadeOutRate = 0.001f, float fadeInRate = 0.001f) {
        if (musicLocked == false) {
            print("new clip: " + newClip);

            if (GetComponent<MusicFadeOut>()) {
                print(GetComponent<MusicFadeOut>());
                Destroy(GetComponent<MusicFadeOut>());
                Destroy(GetComponent<MusicFadeIn>());
                FadeIn(fadeInRate, newClip);
            } else if (newClip != audioSource.clip) {
                FadeOut(fadeOutRate);
                FadeIn(fadeInRate, newClip);
                print("new clip 2: " + newClip);
            }
            else if (!audioSource.isPlaying) {
                FadeIn(fadeInRate, newClip);
            }
        }
    }
}
