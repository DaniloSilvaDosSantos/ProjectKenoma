using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private HealthSystem playerHealth;

    public EnemyAttackState(EnemyController controller, EnemyStateMachine sm) : base(controller, sm) { }

    public override void EnterState()
    {
        Debug.Log("Enemy is attacking");

        attackTimer = 0f;

        isAttacking = true;

        if (controller.playerTarget != null)
        {
            playerHealth = controller.playerTarget.GetComponent<HealthSystem>();

            if (playerHealth == null)
            {
                Debug.LogWarning("Player target" + controller.playerTarget.name + " don't have a HealthSystem!");
            }
        }
    }

    public override void UpdateState()
    {
        if (controller.playerTarget == null || !controller.isAlive) return;

        float distance = Vector3.Distance(controller.transform.position, controller.playerTarget.position);

        if (controller.agent != null) controller.agent.isStopped = true;

        attackTimer += Time.deltaTime;

        if (attackTimer >= controller.enemyData.attackCooldown)
        {
            PerformAttack(distance);
            attackTimer = 0f;

            if (distance > controller.enemyData.attackRange)
            {
                stateMachine.ChangeState(new EnemyChaseState(controller, stateMachine));
                return;
            }
        }
    }

    private void PerformAttack(float distance)
    {
        if (playerHealth == null) return;

        if (distance <= controller.enemyData.attackRange)
        {
            playerHealth.TakeDamage(controller.enemyData.damage);
            Debug.Log(controller.name + " deal " + controller.enemyData.damage + " damage to the player!");
        }
        else
        {
            Debug.Log(controller.name + " attack has missed the player!");
        }
    }
}

