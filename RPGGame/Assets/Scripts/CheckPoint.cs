using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate CheckPoint ID")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        if(activationStatus==false)
            AudioManager.instance.PlaySFX(5, null);
        activationStatus = true;
        anim.SetBool("active", true);
    }
}
