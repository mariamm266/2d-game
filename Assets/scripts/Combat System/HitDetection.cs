using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [System.Serializable]
    public class Hitbox
    {
        public Transform point;
        public float radius = 0.5f;
        public int damage = 10;
        public LayerMask targetLayers;
        [HideInInspector] public float lastHitTime;
    }

    public Hitbox[] attackHitboxes;
    public float hitCooldown = 0.5f;
    public bool drawDebugGizmos = true;

    private void OnDrawGizmos()
    {
        if (!drawDebugGizmos || attackHitboxes == null) return;

        Gizmos.color = Color.red;
        foreach (var box in attackHitboxes)
        {
            if (box.point != null)
                Gizmos.DrawWireSphere(box.point.position, box.radius);
        }
    }

    [System.Obsolete]
    public void ActivateHitbox(int index)
    {
        if (index < 0 || index >= attackHitboxes.Length) return;
        
        var hitbox = attackHitboxes[index];
        if (Time.time < hitbox.lastHitTime + hitCooldown) return;
        if (hitbox.point == null) return;

        var hits = Physics2D.OverlapCircleAll(
            hitbox.point.position, 
            hitbox.radius, 
            hitbox.targetLayers
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                var health = hit.GetComponent<Health>();
                if (health != null && !health.IsDead())
                {
                    health.TakeDamage(hitbox.damage);
                    hitbox.lastHitTime = Time.time;
                }
            }
        }
    }
}