using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLosingSpeed;

    private float cloneTimer;
    private float attakcMultiplier;
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius = .8f;
    //private Transform closestEnemy;
    private int facingDirection = 1;


    private bool canDuplicateClone;
    private float chanceToDuplicate;

    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float closestEnemyCheckRadius = 25;
    [SerializeField] private Transform closestEnemy;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(FaceClosestTarget());
        
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0 )
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset/*, Transform _closestEnemy*/,bool _canDuplicate,float _chanceToDuplicate,Player _player,float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        
        attakcMultiplier= _attackMultiplier;
        cloneTimer = _cloneDuration;
        transform.position = _newTransform.position + _offset;

        player = _player;
       // closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        //FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position,attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {

                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats=hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attakcMultiplier);

                if (player.skill.cloneSkill.canApplyOnHitEffect)
                {
                    ItemData_Equippment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(hit.transform,new Vector3(0.5f * facingDirection,0));//ekleme yapilabilir
                    }
                }
            }
        }
    }

    //private void FaceClosestTarget()
    //{
    //    if (closestEnemy != null)
    //    {
    //        if (transform.position.x > closestEnemy.position.x)
    //        {
    //            facingDirection = -1;
    //            transform.Rotate(0, 180, 0);
    //        }
    //    }
    //}

    private IEnumerator FaceClosestTarget()
    {
        yield return null;

        FindClosestEnemy();
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private void FindClosestEnemy()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,closestEnemyCheckRadius,whatIsEnemy);

        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
             float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

             if (distanceToEnemy < closestDistance)
             {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
             }   
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closestEnemyCheckRadius);
    }
}
