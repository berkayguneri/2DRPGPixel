using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class CharacterStats : MonoBehaviour
{
    private EntityFX fX;

    [Header("Major Stats")]
    public Stats strength;
    public Stats agility;
    public Stats intelligence;
    public Stats vitality;

    [Header("Offensive Stats")]
    public Stats damage;
    public Stats critChange;
    public Stats critPower;


    [Header("Defensive stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;


    public bool isIgnited; // zaman içinde hasar verir
    public bool isChilled; // zýrhý %20 azaltýr
    public bool isShocked; // reduce accuracy by 20%


    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    public int currentHealth;
    public System.Action onHealthChanged;
    protected bool isDead;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        fX=GetComponent<EntityFX>();

        Debug.Log("character stats called");
    }


    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;



        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteDamage();

    }

   

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage=CalculateCriticalDamage(totalDamage);
            Debug.Log("Total crit damage is" + totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
        //DoMagicalDamage(_targetStats);
    }

    #region Magical damage and ailments
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    private void AttemptyToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilmnets(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilmnets(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilmnets(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        _targetStats.ApplyAilmnets(canApplyIgnite, canApplyChill, canApplyShock);
    }


    public void ApplyAilmnets(bool _ignite, bool _chill, bool _shocked)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 2;
        }

        if (_chill)
        {
            isChilled=_chill;
            chilledTimer = 2;
        }

        if (_shocked)
        {
            isShocked = _shocked;
            shockedTimer = 2;
        }

    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            Debug.Log("Take burn damage" + igniteDamage);

            DecreaseHealthBy(igniteDamage);


            if (currentHealth < 0 && !isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int _damage) =>igniteDamage=_damage;

    #endregion
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fX.StartCoroutine("FlashFX");
        

        if (currentHealth < 0 && !isDead)
            Die();
        
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }
    #region Stat calculation
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage-=_targetStats.armor.GetValue();
        }
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChange.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower=(critPower.GetValue() +strength.GetValue()) * .01f;
        Debug.Log("total crit power %" + totalCritPower);

        float critDamage=_damage * totalCritPower;
        Debug.Log("crit damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion
}
