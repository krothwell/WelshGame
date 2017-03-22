using System;

public class StrikeAbility : CharacterEffectAbility {

    public override void FollowThroughAbility() {
        if (myCharacter.GetCombatController().IsCurrentTargetInWeaponRange()) {
            print(myCharacter + " hit connected");
            StopAbility();
        }
    }

    public override void SetMyRange() {
        myRange =  myCharacter.GetCombatController().GetWeaponReachXY();
    }

    public override void InitialiseMe(Character character) {
        SetMyCharacter(character);
        SetCharAction(new StrikeAbilityAction(character.GetMyAnimator()));
    }
}
