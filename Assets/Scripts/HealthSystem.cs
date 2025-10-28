using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IEntityController controller;

    [Header("Variables")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("Events")]
    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;

    private void Awake()
    {
        controller = GetComponent<IEntityController>();

        if (controller != null)
        {
            EntityData data = controller.GetEntityData();
            if (data != null)
            {
                maxHealth = data.maxHealth;
                currentHealth = maxHealth;
            }
            else
            {
                Debug.LogWarning($"[HealthSystem] {gameObject.name} have a controller but the data is empty!");
                maxHealth = 100f;
                currentHealth = 100f;
            }
        }
        else
        {
            Debug.LogWarning($"[HealthSystem] None IEntityController has been found {gameObject.name}!");
            maxHealth = 100f;
            currentHealth = 100f;
        }
    }

    public void Initialize(float value)
    {
        maxHealth = value;
        currentHealth = value;
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (controller != null)
        {
            controller.OnEntityDeath();
        }
        else
        {
            Debug.Log($"{gameObject.name} died without a controller.");
        }
    }
}
