using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerDamageTester : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] private KeyCode damageInput = KeyCode.K;
    [SerializeField] private KeyCode healInput = KeyCode.H;
    [SerializeField] private float amountDamage = 20f;
    [SerializeField] private float amountHeal = 10f;
    private HealthSystem health;

    void Start()
    {
        health = GetComponent<HealthSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(damageInput))
        {
            health.TakeDamage(amountDamage);
            Debug.Log($"Player took " + amountDamage + " damage. Current HP: " + health.GetCurrentHealth());
        }
        if (Input.GetKeyDown(healInput))
        {
            health.Heal(amountHeal);
            Debug.Log($"Player healed " + amountHeal + ". Current HP: " + health.GetCurrentHealth());
        }
    }
}
