using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IEntityController controller;
    [SerializeField] private CameraShakeSystem cameraShakeSystem;
    [SerializeField] private AudioSource audioSource;

    [Header("Damage FX")]
    [SerializeField] private bool spawnParticles = true;
    [SerializeField] private GameObject damageParticlesPrefab;
    private ParticleSystem damageParticles;
    [Space]
    [SerializeField] private GameObject deathParticlesPrefab;
    private ParticleSystem  deathParticles;

    [Header("Vignette FX")]
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private float maxVinheta = 10f;

    [Header("Variables")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private bool isDamageDoubled = false;
    private float damageDoubleTimer = 0f;
    [SerializeField] private bool isInvulnerable = false;
    public bool IsInvulnerable => isInvulnerable;

    [Header("Events")]
    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnDamaged;

    private void Awake()
    {
        controller = GetComponent<IEntityController>();
        cameraShakeSystem = FindAnyObjectByType<CameraShakeSystem>();

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

        SpawnDamageParticles();

        if(isPlayer && GameController.Instance.damageMaterial != null) UpdateVignette();
    }

    public void Initialize(float value)
    {
        maxHealth = value;
        currentHealth = value;
    }

    private void Update()
    {
        if (isDamageDoubled)
        {
            damageDoubleTimer -= Time.deltaTime;
            if (damageDoubleTimer <= 0f)
            {
                isDamageDoubled = false;
            }
        }
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;

    public void UpdateMaxHealth(float newMaxHealth, bool healToFull = false)
    {
        maxHealth = newMaxHealth;

        if (healToFull)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        OnHealthChanged?.Invoke(currentHealth);
    }


    public void TakeDamage(float amount, bool registerKillForMagic = true, float screenShakeDuration = 0.25f, float screenShakeStrenght = 0.6f)
    {
        if(isInvulnerable) return;

        if (isDamageDoubled)
        {
            Debug.Log(gameObject.name + " received double damage!");

            amount *= 2f;
            isDamageDoubled = false;
        }

        Radio.Instance.PlaySFX("SFX/Hit");
        
        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if(cameraShakeSystem != null)
        {
            cameraShakeSystem.Shake(screenShakeDuration, screenShakeStrenght);
        }

        OnDamaged?.Invoke(amount);

        OnHealthChanged?.Invoke(currentHealth);

        if(isPlayer && GameController.Instance.damageMaterial != null) UpdateVignette();

        if (currentHealth <= 0)
        {
            if(registerKillForMagic)
            {
                FindAnyObjectByType<PlayerMagicSystem>().RegisterKill();
            } 

            Die();

            return;
        }

        if (damageParticles != null)
        {
            damageParticles.Play();
        }
    }

    public void ActivateDoubleDamage(float duration)
    {
        isDamageDoubled = true;

        if(damageDoubleTimer < duration) damageDoubleTimer = duration;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if(isPlayer && GameController.Instance.damageMaterial != null) UpdateVignette();

        OnHealthChanged?.Invoke(currentHealth);
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    private void Die()
    {
        SpawnDeathParticles();

        if (deathParticles != null)
        {
            deathParticles.Play();
        }

        OnDeath?.Invoke();

        if (controller != null)
        {
            controller.OnEntityDeath();
        }
        else
        {
            Debug.Log(gameObject.name + "died without a controller.");
        }
    }

    private void SpawnDamageParticles()
    {
        if (spawnParticles && damageParticlesPrefab != null)
        {
            GameObject instance = Instantiate(damageParticlesPrefab, transform.position, transform.rotation);
            instance.transform.SetParent(transform);
            damageParticles = instance.GetComponent<ParticleSystem>();

            if (damageParticles == null) Debug.LogWarning("damageParticlesPrefab don't have a ParticleSystem!");
        }
    }

    private void SpawnDeathParticles()
    {
        if (spawnParticles && deathParticlesPrefab != null)
        {
            GameObject instance = Instantiate(deathParticlesPrefab, transform.position, transform.rotation);
            instance.transform.SetParent(transform);
            deathParticles = instance.GetComponent<ParticleSystem>();

            if (deathParticles == null) Debug.LogWarning("deathParticlesPrefab não possui um ParticleSystem!");
        }
    }

    private void UpdateVignette()
    {
        if (GameController.Instance.damageMaterial == null) return;

        float normalizedHealth = currentHealth / maxHealth;

        float vinheta = Mathf.Lerp(maxVinheta, 0f, normalizedHealth);

        GameController.Instance.UpdateVignette(vinheta);
    }
}
