using UnityEngine;

public class VehicleBumper : MonoBehaviour
{
    [SerializeField]
    private float damage = 100f;

    [SerializeField]
    private AudioSource hitAudioSource;

    private void OnTriggerEnter(Collider collider)
    {
        EnemyBase enemy = collider.GetComponentInParent<EnemyBase>();

        if (enemy == null)
            return;

        enemy.TakeDamage(damage);

        if (hitAudioSource != null)
            hitAudioSource.PlayOneShot(hitAudioSource.clip);
    }
}
