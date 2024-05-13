using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone Can Duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Shuriken instead of clone")]
    [SerializeField] private bool shurikenInsteadOfClone;


    public void CreateClone(Transform _clonePosition)
    {
        if (shurikenInsteadOfClone)
        {
            SkillManager.instance.shurikenSkill.CreateShuriken();
            return;
        }


        GameObject newClone=Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().
            SetUpClone(_clonePosition,cloneDuration,canAttack,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player);
    }
    public void CreateCloneDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform);
    }

    public void CreateCloneDashOver()
    {
        if(createCloneOnDashOver)
            CreateClone(player.transform);
    }
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform);
    }
}
