using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attakCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<ArrowController>() != null)
            {
                hit.GetComponent<ArrowController>().FlipArrow();
                SuccessfulCounterAttack();
            }


            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                    {
                        SuccessfulCounterAttack();

                        player.skill.parrySkill.UseSkill();

                        if (canCreateClone)
                        {
                            canCreateClone = false;
                            player.skill.parrySkill.MakeMirageOnParry(hit.transform);
                        }
                    }
                }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }

    private void SuccessfulCounterAttack()
    {
        stateTimer = 10;// 1 den yuksek tum rakamlar olabilir. 
        player.anim.SetBool("SuccessfulCounterAttack", true);
    }
}
