using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UISkillTreeSlots unlockDodgeButton;
    [SerializeField] private int evasionAmount = 10;
    public bool dodgeUnlocked;

    [Header("Mirage Dodge")]
    [SerializeField] private UISkillTreeSlots unlockMirageDodge;
    public bool dodgeMirageUnlocked;


    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }
    private void UnlockDodge()
    {
        if (unlockDodgeButton.unLocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockMirageDodge()
    {
        if(unlockMirageDodge.unLocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform,new Vector3(2.7f * player.facingDir, 0.7f));
    }
}
