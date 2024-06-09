using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity =>GetComponentInParent<Entity>();
    private CharacterStats myStats =>GetComponentInParent<CharacterStats>();   
    private RectTransform rectTransform;
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        rectTransform = GetComponent<RectTransform>();

        UpdateHealthUI();
    }


    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if(entity != null)
            entity.onFlipped -= FlipUI;

        if(myStats != null)
            myStats.onHealthChanged -= UpdateHealthUI;

    }
    private void FlipUI() => rectTransform.Rotate(0, 180, 0);
   

}
