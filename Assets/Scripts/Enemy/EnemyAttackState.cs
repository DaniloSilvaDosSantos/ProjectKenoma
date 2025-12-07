using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer = 0f;
    private NavMeshAgent agent;
    private HealthSystem playerHealth;

    private bool startedAttackCycle = false;

    public EnemyAttackState(EnemyController controller, EnemyStateMachine sm) : base(controller, sm)
    {
        agent = controller.agent;
    }

    public override void EnterState()
    {
        attackTimer = 0f;
        startedAttackCycle = false;

        if (agent != null) agent.isStopped = true;

        controller.animator.SetBool("isAttacking", true);
        controller.animator.SetBool("canAttack", false);
                        
        if (controller.playerTarget != null)
        {
            playerHealth = controller.playerTarget.GetComponent<HealthSystem>();

            if (playerHealth == null)
            {
                Debug.LogWarning("Player target doesn't have HealthSystem!");
            }
        }
    }

    public override void UpdateState()
    {
        if (!controller.isAlive) return;
        if (controller.playerTarget == null) return;

        if (attackTimer < controller.enemyData.attackCooldown) attackTimer += Time.deltaTime;

        if (!startedAttackCycle && attackTimer >= controller.enemyData.attackCooldown)
        {
            Debug.Log("peles");

            startedAttackCycle = true;
            controller.animator.SetBool("canAttack", true);
        }
    }

    public void OnAttackAnimationEnd()
    {
        float distance = Vector3.Distance(controller.transform.position, controller.playerTarget.position);

        PerformAttack(distance);

        if (distance <= controller.enemyData.attackRange)
        {
            attackTimer = 0f;
            startedAttackCycle = false;

            controller.animator.SetBool("canAttack", false);
            controller.animator.SetBool("isAttacking", true);
        }
        else
        {
            controller.animator.SetBool("isAttacking", false);
            controller.animator.SetBool("canAttack", false);

            if (agent != null) agent.isStopped = false;
            stateMachine.ChangeState(new EnemyChaseState(controller, stateMachine));
        }
    }

    private void PerformAttack(float distance)
    {
        if (playerHealth == null) return;

        if (distance <= controller.enemyData.attackRange)
        {
            playerHealth.TakeDamage(controller.enemyData.damage);
            // Debug.Log($"{controller.name} deal {controller.enemyData.damage} damage to the player!");
        }
        else
        {
            // Debug.Log($"{controller.name} attack has missed the player!");
        }
    }

    public override void ExitState()
    {
        controller.animator.SetBool("isAttacking", false);
        controller.animator.SetBool("canAttack", false);

        if (agent != null) agent.isStopped = false;
    }
}