using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    private EnemyShady enemy;
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName, EnemyShady _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //AudioManager.instance.PlaySFX(20, null);

        //PlayerManager.instance.currency = PlayerManager.instance.currency + 10;
        //enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        //enemy.anim.speed = 0;
        //enemy.cd.enabled = false;

        //stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            enemy.SelfDestroy();
        //if (stateTimer > 0)
        //{
        //    rb.velocity = new Vector2(0, 10);
        //}

    }
}
