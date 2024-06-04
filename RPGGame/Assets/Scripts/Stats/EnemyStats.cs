using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0, 1)]
    [SerializeField] private float percantageModifier = 0.4f;


    protected override void Start()
    {
        ApplyLevelModifers();

        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChange);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

    }

    private void Modify(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifer = _stats.GetValue() * percantageModifier;
            _stats.AddModifier(Mathf.RoundToInt(modifer));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        //enemy.DamageImpact();

    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        myDropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}
