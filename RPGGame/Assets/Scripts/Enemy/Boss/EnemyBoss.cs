using UnityEngine;

public class EnemyBoss : Enemy
{
    #region States
    public BossBattleState battleState { get; private set; }
    public BossAttackState attackState { get; private set; }
    public BossIdleState idleState { get; private set; }
    public BossDeadState deadState { get; private set; }
    public BossSpellCastState spellCastState { get; private set; }
    public BossTeleportState teleportState { get; private set; }
    #endregion

    public bool bossFightBegun;

    [Header("SpellCast Details")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellStateCooldown;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastTimeCast;


    [Header("Teleport Details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);
        battleState = new BossBattleState(this, stateMachine, "move", this);
        attackState = new BossAttackState(this, stateMachine, "attack", this);
        idleState = new BossIdleState(this, stateMachine, "idle", this);
        deadState = new BossDeadState(this, stateMachine, "idle", this);
        spellCastState = new BossSpellCastState(this, stateMachine, "spellCast", this);
        teleportState = new BossTeleportState(this, stateMachine, "teleport", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initiliaze(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;

        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * 3;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 1.5f);


        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<BossSpellController>().SetupSpell(stats);
    }
    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }
    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }
        return false;
    }
}

