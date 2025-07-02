using UnityEngine;

public class AttackSystem : Fighter
{
    protected virtual void Update()
    {
    }

    public override void BasicAttack_1()
    {
        if (!CanAttack()) return;

        anim.SetTrigger("Atk_1");
        
        // Apply base damage to hitbox
        if (TryGetComponent<HitDetection>(out var hitDetection) && hitDetection.attackHitboxes.Length > 0)
        {
            hitDetection.attackHitboxes[0].damage = baseDamage;
        }
    }

    public override void BasicAttack_2()
    {
        if (!CanAttack()) return;

        anim.SetTrigger("Atk_2");
        
        // Apply base damage to hitbox
        if (TryGetComponent<HitDetection>(out var hitDetection) && hitDetection.attackHitboxes.Length > 1)
        {
            hitDetection.attackHitboxes[1].damage = baseDamage;
        }
    }

    protected override bool CanAttack()
    {
        return base.CanAttack();
    }
}