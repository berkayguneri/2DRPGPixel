using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UISkillTreeSlots dashUnlockButton;
    public bool dashUnlocked {  get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UISkillTreeSlots cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UISkillTreeSlots cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; } 

    public override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Created clone behind");
    }

    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }
    private void UnlockDash()
    {
        Debug.Log("attempt to unlock dash");
        if (dashUnlockButton.unLocked)
        {
            dashUnlocked = true;
            Debug.Log("dash unlocked");
        }

    }
    private void UnlockCloneOnDash()
    {

        if(cloneOnDashUnlockButton.unLocked)
            cloneOnDashUnlocked = true;

    }
    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unLocked)
            cloneOnArrivalUnlocked = true;

    }
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(0, 0.7f));

    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(0 , 0.7f));
    }

}
