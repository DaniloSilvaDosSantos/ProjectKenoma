using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private NavMeshAgent agent;

    public EnemyChaseState(EnemyController controller, EnemyStateMachine sm) : base(controller, sm)
    {
        agent = controller.agent;
    }

    public override void EnterState()
    {
        //Debug.Log("Enemy is Chasing");

        if (agent == null) agent = controller.agent;

        agent.isStopped = false;

        if (controller.playerTarget) agent.SetDestination(controller.playerTarget.position);
    }

    public override void UpdateState()
    {
        if (controller.playerTarget == null) return;

        agent.SetDestination(controller.playerTarget.position);

        float distance = Vector3.Distance(controller.transform.position, controller.playerTarget.position);

        if (distance <= controller.enemyData.attackRange)
        {
            stateMachine.ChangeState(new EnemyAttackState(controller, stateMachine));
        }
    }

    public override void ExitState()
    {
        agent.isStopped = true;
    }
}

