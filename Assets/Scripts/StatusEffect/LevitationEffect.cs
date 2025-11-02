using UnityEngine;

public class LevitationEffect : IStatusEffect
{
    public float Duration { get; private set; }
    public bool UpdateEveryFrame => true;

    private float levitateDuration;
    private float startOffset;
    private float targetOffset;
    private float elapsed;
    private bool isFalling = false;
    private float gravity;
    private float currentOffset;

    public LevitationEffect(float duration, float levitateDuration, float height, float gravityValue)
    {
        Duration = duration;
        this.levitateDuration = levitateDuration;
        targetOffset = height;
        gravity = gravityValue;
    }

    public void OnApply(EnemyController enemy)
    {
        Debug.Log(enemy.gameObject.name + " is levitating!");

        startOffset = enemy.agent.baseOffset;

        enemy.agent.isStopped = true;
        //enemy.agent.updatePosition = false;

        currentOffset = startOffset;
        elapsed = 0;
        isFalling = false;
    }

    public void OnUpdate(EnemyController enemy, float deltaTime)
    {
        elapsed += deltaTime;

        if (!isFalling && elapsed < levitateDuration)
        {
            currentOffset = Mathf.Lerp(startOffset, startOffset + targetOffset, elapsed / levitateDuration);
            enemy.agent.baseOffset = currentOffset;
            return;
        }

        isFalling = true;

        currentOffset += gravity * deltaTime;
        enemy.agent.baseOffset = currentOffset;

        if (enemy.agent.baseOffset <= startOffset)
        {
            enemy.agent.baseOffset = startOffset;
            enemy.agent.isStopped = false;
            //enemy.agent.updatePosition = true;

            Duration -= Duration;
        }
    }

    public void OnRemove(EnemyController enemy)
    {
        Debug.Log(enemy.gameObject.name + " recovered from levitation.");
    }
}
