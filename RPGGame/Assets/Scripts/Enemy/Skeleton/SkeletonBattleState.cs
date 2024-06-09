using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDirection;

    private bool flippedOnce;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

        stateTimer = enemy.battleTime;
        flippedOnce= false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("xVelocity", enemy.rb.velocity.x);
        
        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime; 
            if (enemy.IsPlayerDetected().distance<enemy.attackDistance) 
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }

            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        if (distanceToPlayerX < 1f)
            return;

        if (player.position.x > enemy.transform.position.x)
            moveDirection = 1;

        else if (player.position.x < enemy.transform.position.x)
            moveDirection = -1;


        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.velocity.y);       
    }
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastAttackTime + enemy.attackCooldown)
        {
            enemy.attackCooldown=Random.Range(enemy.minAttackCooldown,enemy.maxAttackCooldown);
            enemy.lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
}
