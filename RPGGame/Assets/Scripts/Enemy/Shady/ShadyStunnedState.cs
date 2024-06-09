using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    private EnemyShady enemy;
    public ShadyStunnedState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fX.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDir.x, enemy.stunDir.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fX.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
