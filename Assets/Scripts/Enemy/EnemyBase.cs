using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float maxHealth = 100f;
    protected Health health;
    protected VehicleController target;

    public UnityEvent<EnemyBase> Hit = new();
    public UnityEvent<EnemyBase> Died = new();

    protected enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }

    protected virtual void Awake()
    {
        health = GetComponent<Health>();
        health.Initialize(maxHealth);
        health.Hit.AddListener(HandleHit);
        health.Died.AddListener(HandleDeath);
    }

    public void SetTarget(VehicleController target)
    {
        this.target = target;
    }

    public virtual void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    protected virtual void HandleHit()
    {
        Hit.Invoke(this);
    }

    protected virtual void HandleDeath()
    {
        Died.Invoke(this);
        Destroy(gameObject);
    }

    public void Kill()
    {
        health.Kill();
    }

    protected virtual void OnDestroy()
    {
        if (health != null)
        {
            health.Hit.RemoveListener(HandleHit);
            health.Died.RemoveListener(HandleDeath);
        }
    }
}
