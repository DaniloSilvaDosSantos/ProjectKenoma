using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyStateMachine : MonoBehaviour
{
    private EnemyBaseState currentState;
    public EnemyBaseState CurrentState => currentState;
    public EnemyController controller;

    public void Initialize(EnemyController controller)
    {
        this.controller = controller;
        ChangeState(new EnemyChaseState(controller, this));
    }

    private void Update()
    {
        if (!controller.isAlive) return;
        if (controller.isFreezed) return;

        currentState?.UpdateState();
    }

    public void ChangeState(EnemyBaseState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
