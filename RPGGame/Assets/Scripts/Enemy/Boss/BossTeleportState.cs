using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportState : EnemyState
{

    private EnemyBoss enemy;
    public BossTeleportState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName, EnemyBoss _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("in teleport state");
        enemy.stats.MakeInvincible(true);
    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.CanDoSpellCast())
                stateMachine.ChangeState(enemy.spellCastState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
            
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }
}
