using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour {

    public AudioClip myAudioClip;
    MusicPlayer musicPlayer;

    void Start() {
        
    }

    void OnTriggerEnter2D(Collider2D inCollider) {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        if (inCollider.transform.name == "Perimeter") {
            
            PlayerCharacter playerCharacter = inCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                //print("ENTERING MUSIC ZONE");
                TransitionToZoneMusic();
                musicPlayer = FindObjectOfType<MusicPlayer>();
                playerCharacter.CurrentMusicZone = this;
            }
        }
    }

    public void TransitionToZoneMusic() {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.TransitionMusic(myAudioClip);
        musicPlayer.StartLoop();
    }


    void OnTriggerExit2D(Collider2D outCollider) {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        if (outCollider.transform.name == "Perimeter") {
            print("EXITING MUSIC ZONE");
            PlayerCharacter playerCharacter = outCollider.transform.parent.GetComponent<PlayerCharacter>();
            if (playerCharacter != null) {
                musicPlayer.EndLoop();
                playerCharacter.CurrentMusicZone = null;
            }
        }
    }


}
