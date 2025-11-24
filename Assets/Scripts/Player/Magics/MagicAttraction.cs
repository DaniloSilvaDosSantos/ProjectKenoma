using UnityEngine;
using System.Collections.Generic;

public class MagicAttraction : MagicBase
{
    public override void Cast()
    {
        Vector3 origin = controller.transform.position;
        Vector3 forward = playerCamera.transform.forward;

        float radius = magicData.range;
        float halfAngle = magicData.attractionConeAngle;
        float minDistance = magicData.pullMinDistance;

        Collider[] hits = Physics.OverlapSphere(origin, radius);
        List<EnemyController> targets = new();

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            if (!hit.TryGetComponent(out EnemyController enemy)) continue;

            Vector3 dirToEnemy = (enemy.transform.position - origin).normalized;
            float angle = Vector3.Angle(forward, dirToEnemy);

            if (angle <= halfAngle) targets.Add(enemy);
        }

        if (targets.Count == 0) return;

        foreach (var enemy in targets)
        {
            ApplyInstantPull(enemy, origin, minDistance);
        }
    }

    private void ApplyInstantPull(EnemyController enemy, Vector3 playerPos, float minDistance)
    {
        if (enemy == null) return;

        if (enemy.agent != null)
        {
            enemy.agent.enabled = false;
        }

        enemy.isFreezed = true;

        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.Log(enemy.gameObject.name + " doesn't have a rigidbody!");
        }

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Vector3 distanceToPlayer = playerPos - enemy.transform.position;
        float distance = distanceToPlayer.magnitude;
        Vector3 pullDirection = distanceToPlayer.normalized;

        if (distance <= minDistance)
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            float percent = Mathf.InverseLerp(minDistance, magicData.range, distance);

            float finalForce = percent * magicData.pullForceMultiplier * 10f;

            rb.AddForce(pullDirection * finalForce, ForceMode.Impulse);
        }

        if (enemy.TryGetComponent(out StatusEffectHandler handler))
        {
            Debug.Log(enemy.gameObject.name + " have been stunned!");
            handler.ApplyStatus(new StunnedEffect(magicData.stunDuration));
        }
    }
}
