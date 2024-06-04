using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShurikenSkillController : MonoBehaviour
{
    private Animator anim=>GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float shurikenExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    [SerializeField] private float growSpeed = 5f;

    private Transform closestTarget;
   
    public void SetUpShuriken(float _shurikenDuration, bool _canExplode, bool _canMove, float _moveSpeed,Transform _closestTarget,Player _player)
    {
        shurikenExistTimer = _shurikenDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed=_moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }
    private void Update()
    {
        shurikenExistTimer -= Time.deltaTime;

        if (shurikenExistTimer < 0)
        {
            FinishShuriken();
        }

        if (canMove)
        {
            if (closestTarget == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishShuriken();
                canMove = false;
            }
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDirection(transform);

                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equippment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    public void FinishShuriken()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() =>Destroy(gameObject);
}
