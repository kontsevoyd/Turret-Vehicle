using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [Header("Muzzle Settings")]
    [SerializeField]
    private GameObject aimLine;

    [SerializeField]
    private Transform muzzlePoint;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [Header("Fire Settings")]
    [SerializeField]
    private float fireInterval = 0.3f;

    [Header("Components")]
    [SerializeField]
    private ProjectilePool projectilePool;

    [SerializeField]
    private AudioSource shotAudioSource;

    private bool isShooting;
    private float fireTimer;

    private void Update()
    {
        if (!isShooting)
            return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        Projectile projectile = projectilePool.GetProjectile();

        projectile.transform.SetPositionAndRotation(muzzlePoint.position, muzzlePoint.rotation);
        projectile.gameObject.SetActive(true);

        projectile.StartTrail();
        muzzleFlash.Play();
        shotAudioSource.pitch = Random.Range(0.95f, 1.05f);
        shotAudioSource.PlayOneShot(shotAudioSource.clip);
    }

    public void StartShooting()
    {
        isShooting = true;
        fireTimer = 0f;
        aimLine.SetActive(true);
    }

    public void StopShooting()
    {
        isShooting = false;
        aimLine.SetActive(false);
    }
}
