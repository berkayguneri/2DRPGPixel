using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossTrigger : EnemySkelettonAnimationTriggers
{
    private EnemyBoss enemyBoss => GetComponentInParent<EnemyBoss>();

    private void Relocate() => enemyBoss.FindPosition();

    private void MakeInvisible() =>enemyBoss.fX.MakeTransparent(true);
    private void MakeVisible() =>enemyBoss.fX.MakeTransparent(false);
}
