using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer spriteRenderer;

    [Header("PopUpText")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("FlashFX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material originalMat;


    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockedColor;

    [Header("Particle System ")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit fx")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;

    private GameObject myHealthBar;



    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        player = PlayerManager.instance.player;

        originalMat = spriteRenderer.material;

        myHealthBar = GetComponentInChildren<HealthBarUI>(true).gameObject;
    }


    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1.5f, 3f);


        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar.SetActive(false);
            spriteRenderer.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            spriteRenderer.color = Color.white;
        }
    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        Color currentColor = spriteRenderer.color;

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }


    public void IgniteFXFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFXFor(float _seconds)
    {
        chillFX.Play();
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

    public void CreateHitFx(Transform _target, bool _critical)
    {

        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitprefab = hitFX;

        if (_critical)
        {
            hitprefab = criticalHitFX;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFX = Instantiate(hitprefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);//_target);


        newHitFX.transform.Rotate(hitFxRotation);

        Destroy(newHitFX, .5f);

    }

}
