using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : EnemyState
{
    private EnemyBoss enemy;
    private Transform player;
    public BossIdleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName, EnemyBoss _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.PlaySFX(24, null);
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            enemy.bossFightBegun = true;

        if(Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if (stateTimer < 0 && enemy.bossFightBegun)
            stateMachine.ChangeState(enemy.battleState);
    }
}
