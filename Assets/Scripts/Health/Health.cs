using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent Hit = new UnityEvent();
    public UnityEvent Died = new UnityEvent();

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0f;

    public void Initialize(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);

        Hit?.Invoke();

        if (CurrentHealth <= 0f)
            Died?.Invoke();
    }

    public void Kill()
    {
        if (CurrentHealth <= 0f)
            return;

        CurrentHealth = 0f;
        Died?.Invoke();
    }
}
