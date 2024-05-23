using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healtPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healthAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healtPercent);

        playerStats.IncreaseHealthBy(healthAmount);


    }
}
