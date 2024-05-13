using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    protected Transform player;
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position,player.position)<2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
