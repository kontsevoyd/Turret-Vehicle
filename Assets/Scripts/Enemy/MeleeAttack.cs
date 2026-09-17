using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    private float preferredDistance = 2f;

    [SerializeField]
    private float attackRange = 3f;

    [SerializeField]
    private float attackExitRange = 4f;

    [SerializeField]
    private float attackInterval = 1f;

    [SerializeField]
    private float hitDelay = 0.35f;

    public float AttackRange => attackRange;
    public float AttackExitRange => attackExitRange;
    public float PreferredDistance => preferredDistance;

    private float attackTimer;

    public bool Attack(VehicleController target)
    {
        if (target == null || !target.CanBeTargeted)
            return false;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return false;
        }

        attackTimer = attackInterval;

        StartCoroutine(HitAfterDelay(target));

        return true;
    }

    private IEnumerator HitAfterDelay(VehicleController target)
    {
        yield return new WaitForSeconds(hitDelay);

        if (target == null || !target.CanBeTargeted)
            yield break;

        Vector3 targetPoint = target.GetClosestPoint(transform.position);
        float distance = Vector3.Distance(transform.position, targetPoint);

        if (distance <= attackRange)
        {
            target.TakeDamage(damage);
        }
    }
}
