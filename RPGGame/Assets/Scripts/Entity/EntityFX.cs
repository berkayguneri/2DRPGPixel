using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("FlashFX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material originalMat;


    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockedColor;


    private void Start()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        Color currentColor = spriteRenderer.color;

        spriteRenderer.color=Color.white;
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMat;
    }

    private void RedColorBlink()
    {
        if(spriteRenderer.color!=Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }


    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

   

    private void IgniteColorFX()
    {
        if (spriteRenderer.color != igniteColor[0])
            spriteRenderer.color = igniteColor[0];
        else
            spriteRenderer.color = igniteColor[1];
    }

    private void ShockColorFX()
    {
        if (spriteRenderer.color != shockedColor[0])
            spriteRenderer.color = shockedColor[0];
        else
            spriteRenderer.color = shockedColor[1];
    }

    private void ChillColorFX()
    {
        if (spriteRenderer.color != chillColor[0])
            spriteRenderer.color = chillColor[0];
        else
            spriteRenderer.color = chillColor[1];
    }

}
