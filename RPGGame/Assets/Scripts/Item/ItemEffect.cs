using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDesrcription;

   public virtual void ExecuteEffect(Transform _enemyPosition)
   {
        Debug.Log("Effect executed");
   }
}
