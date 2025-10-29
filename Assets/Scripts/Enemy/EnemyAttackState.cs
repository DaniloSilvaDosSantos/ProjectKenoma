using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer = 0f;

    public EnemyAttackState(EnemyController controller, EnemyStateMachine sm) : base(controller, sm) { }

    public override void EnterState()
    {
        Debug.Log("Enemy is attacking");

        attackTimer = 0f;
    }

    public override void UpdateState()
    {
        if (controller.playerTarget == null) return;

        float distance = Vector3.Distance(controller.transform.position, controller.playerTarget.position);

        if (distance > controller.enemyData.attackRange)
        {
            stateMachine.ChangeState(new EnemyChaseState(controller, stateMachine));
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= controller.enemyData.attackCooldown)
        {
            Debug.Log("The enemy attacked the Player!");
            attackTimer = 0f;
        }
    }
}

