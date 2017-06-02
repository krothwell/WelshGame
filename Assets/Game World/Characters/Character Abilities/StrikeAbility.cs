using System;

public class StrikeAbility : CharacterEffectAbility {

    public override void FollowThroughAbility() {
        if (myCharacter.GetCombatController().IsCurrentTargetInWeaponRange()) {
            print(myCharacter + " hit connected");
            //print(targetCharacter);
            targetCharacter.GetCombatController().GetHit();
        }
    }

    public override void SetMyRange() {
        myRange =  myCharacter.GetCombatController().GetWeaponReachXY();
    }

   
}
