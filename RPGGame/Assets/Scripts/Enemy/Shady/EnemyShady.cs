using System.Collections;
using UnityEngine;
public class EnemyShady : Enemy
{
    [Header("Shady spesifics")]
    public float battleStateMoveSpeed;

    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region States
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "move", this);
        deadState = new ShadyDeadState(this, stateMachine, "dead", this);
        stunnedState = new ShadyStunnedState(this, stateMachine, "stunned", this);
        battleState = new ShadyBattleState(this, stateMachine, "moveFast", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initiliaze(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosive = Instantiate(explosivePrefab, attakCheck.position, Quaternion.identity);

        newExplosive.GetComponent<ExplosiveController>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);
}
