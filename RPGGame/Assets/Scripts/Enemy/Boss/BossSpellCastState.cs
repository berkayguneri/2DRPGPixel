using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class BossSpellCastState : EnemyState
{
    private EnemyBoss enemy;

    private int amountOfSpells;
    private float spellTimer;
    public BossSpellCastState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string _animBoolName, EnemyBoss _enemy) : base(_enemyBase, enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpells=enemy.amountOfSpells;
        spellTimer = .5f;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;
        if (CanCast())
            enemy.CastSpell();

        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }
    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }

}
