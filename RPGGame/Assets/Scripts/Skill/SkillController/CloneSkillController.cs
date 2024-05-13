using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private Player player;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius = .8f;
    private Transform closestEnemy;
    private int facingDirection = 1;


    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
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
    public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Transform _closestEnemy,bool _canDuplicate,float _chanceToDuplicate,Player _player)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        
        transform.position = _newTransform.position + new Vector3(0, .7f, 0);
        cloneTimer = _cloneDuration;

        player= _player;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
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

                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(hit.transform);//ekleme yapilabilir
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }

}
