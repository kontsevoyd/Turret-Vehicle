using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float damage = 25f;

    [SerializeField]
    private float lifetime = 5f;

    [Header("Projectile VFX")]
    [SerializeField]
    private TrailRenderer trail;

    private float lifeTimer;

    private ProjectilePool pool;

    private Rigidbody rigibody;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        lifeTimer = 0f;
        rigibody.linearVelocity = transform.forward * speed;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            pool.Release(this);
        }
    }

    public void SetPool(ProjectilePool projectilePool)
    {
        pool = projectilePool;
    }

    public void ResetProjectile()
    {
        trail.emitting = false;
        trail.Clear();

        rigibody.linearVelocity = Vector3.zero;
        rigibody.angularVelocity = Vector3.zero;
    }

    public void StartTrail()
    {
        trail.Clear();
        trail.emitting = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.collider.GetComponentInParent<IDamageable>();

        damageable?.TakeDamage(damage);

        pool.Release(this);
    }
}
