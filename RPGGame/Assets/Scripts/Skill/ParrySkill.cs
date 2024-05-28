using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UISkillTreeSlots parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UISkillTreeSlots restoreUnlockButton;
    [Range(0,1f)]
    [SerializeField] private float restoreHealthPercantage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UISkillTreeSlots parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount =Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercantage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockedParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockedParryWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unLocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if(restoreUnlockButton.unLocked)
            restoreUnlocked = true;
    }

    private void UnlockedParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unLocked)
            parryWithMirageUnlocked = true;
    }


    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.cloneSkill.CreateCloneWithDelay(_respawnTransform);
    }
}
