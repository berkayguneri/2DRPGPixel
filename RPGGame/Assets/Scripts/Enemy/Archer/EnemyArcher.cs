using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Enemy
{
    [Header("Archer spesific info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowDamage;

    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    #region States
    public ArcherIdleState idleState {  get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(this, stateMachine, "idle",this);
        moveState = new ArcherMoveState(this, stateMachine, "move",this);
        battleState = new ArcherBattleState(this, stateMachine, "idle",this);
        attackState = new ArcherAttackState(this, stateMachine, "attack",this);
        deadState = new ArcherDeadState(this, stateMachine, "move",this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "stunned",this);
        jumpState = new ArcherJumpState(this, stateMachine, "jump", this);
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
        GameObject newArrow = Instantiate(arrowPrefab, attakCheck.position, Quaternion.identity);

        newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDir, stats);
    }

    public bool GroundBehindCheck() =>Physics2D.BoxCast(groundBehindCheck.position,groundBehindCheckSize,0,Vector2.zero,0,whatIsGround);
    public bool WallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position,groundBehindCheckSize);
    }
}
