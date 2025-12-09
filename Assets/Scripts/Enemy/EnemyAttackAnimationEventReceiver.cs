using UnityEngine;

public class EnemyAttackAnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private EnemyController controller;

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
    }

    public void EnemyAttackAnimationEvent()
    {
        if (controller != null) controller.EnemyAttackAnimationEvent();
    }
}

