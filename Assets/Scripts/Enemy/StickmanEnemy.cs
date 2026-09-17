using UnityEngine;
using UnityEngine.UI;

public class StickmanEnemy : EnemyBase
{
    [Header("Stickman Settings")]
    [SerializeField]
    private float activationRadius = 10f;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float rotationSpeed = 180f;

    [Header("Components")]
    [SerializeField]
    private MeleeAttack meleeAttack;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private ParticleSystem deathEffectPrefab;

    [SerializeField]
    private Transform deathVfxPoint;

    [SerializeField]
    private AudioSource hitAudioSource;

    [Header("Stickman UI")]
    [SerializeField]
    private GameObject healthUI;

    [SerializeField]
    private Image healthFill;

    private EnemyState currentState = EnemyState.Idle;

    private static int SpeedHash = Animator.StringToHash("Speed");
    private static int AttackHash = Animator.StringToHash("Attack");

    private void Update()
    {
        if (target == null || !target.CanBeTargeted)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        Vector3 targetPoint = target.GetClosestPoint(transform.position);

        float distance = Vector3.Distance(transform.position, targetPoint);

        switch (currentState)
        {
            case EnemyState.Idle:

                if (distance <= activationRadius)
                    ChangeState(EnemyState.Chasing);

                break;

            case EnemyState.Chasing:

                if (distance <= meleeAttack.AttackRange)
                {
                    ChangeState(EnemyState.Attacking);
                    break;
                }

                ChaseTarget();
                break;

            case EnemyState.Attacking:

                if (distance > meleeAttack.AttackExitRange)
                {
                    ChangeState(EnemyState.Chasing);
                    break;
                }

                ChaseTarget();
                AttackTarget();
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        if (currentState == EnemyState.Idle)
        {
            animator.SetFloat(SpeedHash, 0f);
        }
    }

    private void ChaseTarget()
    {
        Vector3 targetClosestPoint = target.GetClosestPoint(transform.position);
        Vector3 moveDirection = targetClosestPoint - transform.position;
        moveDirection.y = 0f;

        Vector3 targetCenterPoint = target.GetCenterPoint();
        Vector3 lookDirection = targetCenterPoint - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float distance = moveDirection.magnitude;

        bool canMove = distance > meleeAttack.PreferredDistance && moveDirection.z <= 0f;

        float targetSpeed = canMove ? 1f : 0f;

        animator.SetFloat(SpeedHash, targetSpeed, 0.15f, Time.deltaTime);

        if (!canMove)
            return;

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

    private void AttackTarget()
    {
        bool attacked = meleeAttack.Attack(target);

        if (attacked)
        {
            animator.SetTrigger(AttackHash);
        }
    }

    protected override void HandleHit()
    {
        base.HandleHit();

        healthUI.SetActive(true);

        healthFill.fillAmount = health.CurrentHealth / health.MaxHealth;

        if (hitAudioSource != null)
            hitAudioSource.PlayOneShot(hitAudioSource.clip);
    }

    protected override void HandleDeath()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, deathVfxPoint.position, Quaternion.identity);
        }

        base.HandleDeath();
    }
}
