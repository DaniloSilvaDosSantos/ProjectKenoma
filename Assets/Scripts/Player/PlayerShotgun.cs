using UnityEngine;

public class PlayerShotgun : MonoBehaviour
{
    [Header("References")]
    private PlayerController controller;
    [SerializeField] private Transform firePoint;

    [Header("Variables")]
    [SerializeField] private float cooldownTimer = 0f;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        if (controller == null)
        {
            Debug.LogError("PlayerController don't have been found!");
        }

        if (firePoint == null && controller.playerCamera != null)
        {
            firePoint = controller.playerCamera.transform;
        }
    }

    private void Update()
    {
        if (!controller.isAlive) return;

        if(cooldownTimer <= controller.playerData.shotgunCooldown)cooldownTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer >= controller.playerData.shotgunCooldown)
        {
            Shoot();
            cooldownTimer = 0f;
        }
    }

    private void Shoot()
    {
        Debug.Log("Shooting");

        var data = controller.playerData;

        float maxRange = data.shotgunMaxRangeD;
        float halfAngle = data.shotgunHalfAngle;

        Collider[] hits = Physics.OverlapSphere(firePoint.position, maxRange);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Vector3 dirToTarget = (hit.ClosestPoint(firePoint.position) - firePoint.position).normalized;
            float angleToTarget = Vector3.Angle(firePoint.forward, dirToTarget);

            if (angleToTarget > halfAngle) continue;

            float distance = Vector3.Distance(firePoint.position, hit.ClosestPoint(firePoint.position));

            float damageToApply = DetermineShotgunDamage(distance, data);

            if (damageToApply > 0f)
            {
                if (hit.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(damageToApply);
                }
                else
                {
                    var parentHealth = hit.GetComponentInParent<HealthSystem>();
                    if (parentHealth != null) parentHealth.TakeDamage(damageToApply);
                }
            }
        }
    }

    private float DetermineShotgunDamage(float distance, PlayerData data)
    {
        if (distance <= data.shotgunRangeA)
        {
            return data.shotgunMaxDamage;
        }
        else if (distance <= data.shotgunRangeB)
        {
            return data.shotgunCloseDamage;
        }
        else if (distance <= data.shotgunRangeC)
        {
            return data.shotgunFarDamage;
        }
        else if (distance <= data.shotgunMaxRangeD)
        {
            return data.shotgunVeryFarDamage;
        }
        return 0f;
    }
}

