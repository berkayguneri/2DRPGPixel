using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenSkill : Skill
{
    [SerializeField] private float shurikenDuration;
    [SerializeField] private GameObject shurikenPrefab;
    private GameObject currentShuriken;

    [Header("Shuriken Mirage")]
    [SerializeField] private UISkillTreeSlots unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfShuriken;

    [Header("Shuriken simple")]
    [SerializeField] private UISkillTreeSlots unlockShurikenButton;
    public bool shurikenUnlocked { get; private set; }

    [Header("Explosive Shuriken")]
    [SerializeField] private UISkillTreeSlots unlockExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving shuriken")]
    [SerializeField] private UISkillTreeSlots unlockMovingShurikenButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking shuriken")]
    [SerializeField] private UISkillTreeSlots unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWondow;
    [SerializeField] private List<GameObject> shurikenLeft=new List<GameObject>();


    protected override void Start()
    {
        base.Start();
        unlockShurikenButton.GetComponent<Button>().onClick.AddListener(UnlockShuriken);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockShurikenMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveShuriken);
        unlockMovingShurikenButton.GetComponent<Button>().onClick.AddListener(UnlockMovingShuriken);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);

    }
    #region Unlock skill region

    protected override void CheckUnlock()
    {
        UnlockShuriken();
        UnlockShurikenMirage();
        UnlockExplosiveShuriken();
        UnlockMovingShuriken();
        UnlockMultiStack();
    }
    private void UnlockShuriken()
    {
        if (unlockShurikenButton.unLocked)
            shurikenUnlocked = true;
    }
    private void UnlockShurikenMirage()
    {
        if (unlockCloneInsteadButton.unLocked)
            cloneInsteadOfShuriken = true;
    }

    private void UnlockExplosiveShuriken()
    {
        if(unlockExplosiveButton.unLocked)
            canExplode = true;
    }
    
    private void UnlockMovingShuriken()
    {
        if (unlockMovingShurikenButton.unLocked)
            canMoveToEnemy = true;
    }

    private void UnlockMultiStack()
    {
        if(unlockMultiStackButton.unLocked)
            canUseMultiStacks = true;
    }
    #endregion

    

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiShuriken())
            return;

        if (currentShuriken == null)
        {
            CreateShuriken();
        }

        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentShuriken.transform.position;
            currentShuriken.transform.position=playerPos;

            if (cloneInsteadOfShuriken)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentShuriken.transform,new Vector3(2 * player.facingDir,0.78f));
                Destroy(currentShuriken);
            }
            else
            {
                currentShuriken.GetComponent<ShurikenSkillController>()?.FinishShuriken();
            }

        }
    }

    public void CreateShuriken()
    {
        currentShuriken = Instantiate(shurikenPrefab, player.transform.position, Quaternion.identity);
        ShurikenSkillController currentShruikenScript = currentShuriken.GetComponent<ShurikenSkillController>();

        currentShruikenScript.SetUpShuriken(shurikenDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentShuriken.transform),player);
    }

    private bool CanUseMultiShuriken()
    {
        if (canUseMultiStacks)
        {

            if (shurikenLeft.Count > 0)
            {

                if (shurikenLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWondow);

                coolDown = 0;
                GameObject shruikenToSpawn = shurikenLeft[shurikenLeft.Count - 1];
                GameObject newShuriken = Instantiate(shruikenToSpawn, player.transform.position, Quaternion.identity);

                shurikenLeft.Remove(shruikenToSpawn);

                newShuriken.GetComponent<ShurikenSkillController>().
                    SetUpShuriken(shurikenDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newShuriken.transform), player);

                if (shurikenLeft.Count <= 0)
                {
                    coolDown = multiStackCooldown;
                    RefilShuriken();
                }

                return true;
            }

            
        }
        return false;
    }


    private void RefilShuriken()
    {
        int amountToAdd = amountOfStacks - shurikenLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            shurikenLeft.Add(shurikenPrefab);
        }
    }

    private void ResetAbility()
    {

        if (coolDownTimer > 0)
            return;

        coolDownTimer = multiStackCooldown;
        RefilShuriken();
    }
}
