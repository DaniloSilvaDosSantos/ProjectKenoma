using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyController enemy;
    private List<ActiveEffect> activeEffects = new();

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var eff = activeEffects[i];
            eff.time += dt;

            if (eff.effect.UpdateEveryFrame) eff.effect.OnUpdate(enemy, dt);

            if (eff.time >= eff.effect.Duration)
            {
                eff.effect.OnRemove(enemy);
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void ApplyStatus(IStatusEffect effect)
    {
        effect.OnApply(enemy);
        activeEffects.Add(new ActiveEffect(effect));
    }

    private class ActiveEffect
    {
        public IStatusEffect effect;
        public float time;

        public ActiveEffect(IStatusEffect eff)
        {
            effect = eff;
            time = 0;
        }
    }
}
