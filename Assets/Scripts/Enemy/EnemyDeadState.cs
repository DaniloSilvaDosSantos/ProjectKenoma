using System.Collections;
using System.Dynamic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyController controller, EnemyStateMachine sm) : base(controller, sm) { }

    public override void EnterState()
    {
        //Debug.Log("Enemy is dying!");
        controller.StartCoroutine(DelayedDeathSFX(0.3f));

        if (controller.agent != null) controller.agent.isStopped = true;

        controller.animator.SetBool("isDead", true);

        controller.isAlive = false;
        Object.Destroy(controller.gameObject, 2f);

        EnemyXPDropSystem dropSystem = Object.FindAnyObjectByType<EnemyXPDropSystem>();
        PlayerController player = Object.FindAnyObjectByType<PlayerController>();

        if (dropSystem != null && player != null)
        {
            dropSystem.DropXP(controller.enemyData, controller.transform, player.transform);
        }
    }

    public override void UpdateState(){ }

    private IEnumerator DelayedDeathSFX(float delay = 0.3f)
    {
        yield return new WaitForSeconds(delay);
        Radio.Instance.PlaySFX("SFX/EnemyDeath", controller.audioSource);
    }
}