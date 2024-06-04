using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dashSkill.CloneOnDash();
        stateTimer = player.dashDuration;

        //player.stats.MakeInvincible(true);

    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dashSkill.CloneOnArrival();
        player.SetVelocity(0, rb.velocity.y);
        //player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        player.SetVelocity(player.dashSpeed * player.dashDirection,0);
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);

        player.playerFX.CreateAfteerImage();

        //AudioManager.instance.PlaySFX(37, null);

    }
}
