using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone")]
    [SerializeField] private float attackMultipler;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    [SerializeField] private UISkillTreeSlots cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultipler;
    [SerializeField] private bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private UISkillTreeSlots aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultipler;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] private UISkillTreeSlots multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultipler;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Shuriken instead of clone")]
    [SerializeField] private UISkillTreeSlots shurikenInsteadUnlockButton;
    [SerializeField] private bool shurikenInsteadOfClone;


    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        shurikenInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockShurikenInstead);
        
      
    }


    #region Unlock Region

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unLocked)
        {
            canAttack = true;
            attackMultipler = cloneAttackMultipler;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unLocked)
        {
            canApplyOnHitEffect = true;
            attackMultipler = aggresiveCloneAttackMultipler;
        }
    }

    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unLocked)
        {
            canDuplicateClone= true;
            attackMultipler = multiCloneAttackMultipler;
        }
    }

    private void UnlockShurikenInstead()
    {
        if (shurikenInsteadUnlockButton.unLocked)
        {
            shurikenInsteadOfClone= true;
        }
    }

    #endregion

    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if (shurikenInsteadOfClone)
        {
            SkillManager.instance.shurikenSkill.CreateShuriken();
            return;
        }

        GameObject newClone=Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().
            SetUpClone(_clonePosition,cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player,attackMultipler);
    }
  
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform,_offset);    
    }
}
