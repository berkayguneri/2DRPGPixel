using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Transform player;
    private int moveDirection;
    private EnemyShady enemy;
    private float defaultSpeed;
    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName,EnemyShady _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
       this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        defaultSpeed = enemy.moveSpeed;

        enemy.moveSpeed = enemy.battleStateMoveSpeed;

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

        //stateTimer = enemy.battleTime;
       // flippedOnce = false;
    }

   
    public override void Update()
    {
        base.Update();


        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                //if (CanAttack())
                //    stateMachine.ChangeState(enemy.attackState
                enemy.stats.KillEntity();
            }
        }
        else
        {
            //if (flippedOnce == false)
            //{
            //    flippedOnce = true;
            //    enemy.Flip();
            //}

            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }

       // float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x); sonradan eklendi bug fix bolumunde aktif edilebilir tekrar

        //if (distanceToPlayerX < 1f)
        //    return;

        if (player.position.x > enemy.transform.position.x)
            moveDirection = 1;

        else if (player.position.x < enemy.transform.position.x)
            moveDirection = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultSpeed;
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastAttackTime + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
}
