using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class VehicleController : MonoBehaviour, IDamageable
{
    [Header("Vehicle Settings")]
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float maxHealth = 200f;

    [SerializeField]
    private float swayAmount = 0.5f;

    [SerializeField]
    private float swaySpeed = 0.5f;

    [Header("Components")]
    [SerializeField]
    private ProjectileWeapon projectileWeapon;

    [SerializeField]
    private BoxCollider bodyCollider;

    [SerializeField]
    private AudioSource engineAudioSource;

    [SerializeField]
    private AudioSource hitAudioSource;

    [Header("Wheels and Smoke")]
    [SerializeField]
    private Transform[] wheels;

    [SerializeField]
    private float wheelRotationSpeed = 500f;

    [SerializeField]
    private Vector3 wheelRotationAxis = Vector3.right;

    [SerializeField]
    private ParticleSystem exhaustSmoke;

    public bool CanBeTargeted { get; private set; }
    protected Health health;
    private bool isMoving;
    private float startPositionX;
    private float swayTimer;
    public UnityEvent Hit = new();
    public UnityEvent Died = new();

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Initialize(maxHealth);

        health.Hit.AddListener(HandleHit);
        health.Died.AddListener(HandleDeath);

        CanBeTargeted = false;

        startPositionX = transform.position.x;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        Move();
        RotateWheels();
    }

    public void StartVehicle()
    {
        isMoving = true;
        CanBeTargeted = true;
        projectileWeapon.StartShooting();
        exhaustSmoke.Play();

        if (engineAudioSource != null && !engineAudioSource.isPlaying)
            engineAudioSource.Play();
    }

    public void StopVehicle()
    {
        isMoving = false;
        projectileWeapon.StopShooting();
        exhaustSmoke.Stop();

        if (engineAudioSource != null)
            engineAudioSource.Stop();
    }

    public void DisableCombat()
    {
        CanBeTargeted = false;
        projectileWeapon.StopShooting();
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    public Vector3 GetClosestPoint(Vector3 position)
    {
        return bodyCollider.ClosestPoint(position);
    }

    public Vector3 GetCenterPoint()
    {
        return bodyCollider.bounds.center;
    }

    private void HandleHit()
    {
        if (hitAudioSource != null)
            hitAudioSource.PlayOneShot(hitAudioSource.clip);

        Hit.Invoke();
    }

    private void HandleDeath()
    {
        CanBeTargeted = false;
        Died.Invoke();
    }

    private void Move()
    {
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        swayTimer += Time.deltaTime;

        Vector3 position = transform.position;
        position.x = startPositionX + Mathf.Sin(swayTimer * swaySpeed) * swayAmount;
        transform.position = position;
    }

    private void RotateWheels()
    {
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(wheelRotationAxis, wheelRotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.Hit.RemoveListener(HandleHit);
            health.Died.RemoveListener(HandleDeath);
        }
    }
}
