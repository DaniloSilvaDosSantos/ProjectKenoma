public interface IStatusEffect
{
    float Duration { get; }
    bool UpdateEveryFrame { get; }

    void OnApply(EnemyController enemy);
    void OnUpdate(EnemyController enemy, float deltaTime);
    void OnRemove(EnemyController enemy);
}

