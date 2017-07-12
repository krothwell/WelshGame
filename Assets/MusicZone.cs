using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour {

    public AudioClip myAudioClip;
    MusicPlayer musicPlayer;
    AudioSource audioSource;

    void Start() {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        audioSource = musicPlayer.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D incursionCollider) {
        
        if (incursionCollider.transform.name == "Perimeter") {
            //print("ENTERING MUSIC ZONE");
            PlayerCharacter playerCharacter = incursionCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                TransitionToZoneMusic();
                playerCharacter.CurrentMusicZone = this;
            }
        }
    }

    public void TransitionToZoneMusic() {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.TransitionMusic(myAudioClip);
        audioSource = musicPlayer.GetComponent<AudioSource>();
        audioSource.loop = true;
    }


    void OnTriggerExit2D(Collider2D incursionCollider) {
        if (incursionCollider.transform.name == "Perimeter") {
            print("EXITING MUSIC ZONE");
            PlayerCharacter playerCharacter = incursionCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                audioSource.loop = false;
            }
        }
    }


}
