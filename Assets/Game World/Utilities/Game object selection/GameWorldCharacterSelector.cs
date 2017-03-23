
using UnityEngine;


public class GameWorldCharacterSelector : GameWorldSelector {
    bool abilityInRange;
    public override void DisplayCircle() {
        if (abilitySelected != null) {
            if(abilitySelected.IsInRangeOfCharacter(GetComponent<Character>())) {
                abilityInRange = true;
                BuildCircle();
            } else {
                DisplayOutOfRange();
            }
        } else {
            BuildCircle();
        }

    }

    public override void SetSelected() {
        if (abilitySelected) {
            if (abilityInRange) {
                Select();
            }
        } else {
            Select();
        }
        
    }

    public void DisplayOutOfRange() {
        BuildCircle();
        ChangeColourToOutOfRange();
    }

    void ChangeColourToOutOfRange() {
        myAnimator.Stop();
        selectionCircle.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.1f); ;
    }
}
