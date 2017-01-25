using UnityEngine;
using System.Collections;

public class EndIngGameDialogueBtn : MonoBehaviour {
    LowerUI lowerUI;
    MainCharacter mainCharacter;
    void Start() {
        lowerUI = FindObjectOfType<LowerUI>();
        mainCharacter = FindObjectOfType<MainCharacter>();
    }

    public void CloseDialogue() {
        lowerUI.SetNotInUse();
        mainCharacter.playerStatus = MainCharacter.PlayerStatus.passive;
    }
    
}
