interface IAttackable {

    void GetHit(WorldDamage damage);

    void TriggerCombat(Character charIn);

    void EndCombat(Character charIn);
}
