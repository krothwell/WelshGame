using System;

public class StrikeAbility : CharacterEffectAbility {


    public override void FollowThroughAbility() {
        if (myCharacter.GetCombatController().IsCurrentTargetInWeaponRange()) {
            print(myCharacter + " hit connected");
            targetCharacter.GetCombatController().GetHit();
            StopAbility();
        }
    }

    public override void SetMyRange() {
        myRange =  myCharacter.GetCombatController().GetWeaponReachXY();
    }

   
}
