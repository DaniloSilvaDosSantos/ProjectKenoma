using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyController controller, EnemyStateMachine sm) : base(controller, sm) { }

    public override void EnterState()
    {
        //Debug.Log("Enemy is dying!");

        if (controller.agent != null) controller.agent.isStopped = true;

        controller.isAlive = false;
        Object.Destroy(controller.gameObject, 2f);

        //PLACE HOLDER!
        Object.FindAnyObjectByType<PlayerLevelSystem>()?.AddXP(controller.enemyData.xpValue);

    }

    public override void UpdateState(){ }
}