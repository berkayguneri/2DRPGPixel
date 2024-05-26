using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image shurikenImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image flaskImage;



    [SerializeField] private TextMeshProUGUI currentSouls;
    private SkillManager skillManager;
    private void Start()
    {
        if(playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skillManager = SkillManager.instance;
    }
    private void Update()
    {
        currentSouls.text=PlayerManager.instance.GetCurrency().ToString("#,#");


        if (Input.GetKeyDown(KeyCode.LeftShift) && skillManager.dashSkill.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skillManager.parrySkill.parryUnlocked)
            SetCooldownOf(parryImage);
        
        if(Input.GetKeyDown(KeyCode.F) && skillManager.shurikenSkill.shurikenUnlocked)
            SetCooldownOf(shurikenImage);

        if(Input.GetKeyDown(KeyCode.Mouse1) && skillManager.swordSkill.swordUnlocked)
            SetCooldownOf(swordImage);

        if(Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) !=null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage,skillManager.dashSkill.coolDown);
        CheckCooldownOf(parryImage,skillManager.parrySkill.coolDown);
        CheckCooldownOf(shurikenImage,skillManager.shurikenSkill.coolDown);
        CheckCooldownOf(swordImage,skillManager.swordSkill.coolDown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image,float _coolDown)
    {
        if(_image.fillAmount >0)
            _image.fillAmount -= 1 /_coolDown * Time.deltaTime;
    }
}
