public abstract class EnemyBaseState
{
    protected EnemyController controller;
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyController controller, EnemyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public abstract void UpdateState();
}

