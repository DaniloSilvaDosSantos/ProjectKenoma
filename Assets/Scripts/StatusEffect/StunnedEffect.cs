using UnityEngine;

public class StunnedEffect : IStatusEffect
{
    public float Duration { get; private set; }
    public bool UpdateEveryFrame => false;

    public StunnedEffect(float duration)
    {
        Duration = duration;
    }

    public void OnApply(EnemyController enemy)
    {
        enemy.isFreezed = true;

        if (enemy.agent != null)
        {
            enemy.agent.updatePosition = false;
            enemy.agent.updateRotation = false;
            enemy.agent.isStopped = true;
        }
    }

    public void OnUpdate(EnemyController enemy, float deltaTime) { }

    public void OnRemove(EnemyController enemy)
    {
        enemy.isFreezed = false;

        if (enemy.agent != null)
        {
            enemy.agent.updatePosition = true;
            enemy.agent.updateRotation = true;
            enemy.agent.isStopped = false;
        }
    }
}

