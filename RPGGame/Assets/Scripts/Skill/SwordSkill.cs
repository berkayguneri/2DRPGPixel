using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce")]
    [SerializeField] private UISkillTreeSlots bounceUnlockButton;
    [SerializeField] private int amountBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce")]
    [SerializeField] private UISkillTreeSlots pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin")]
    [SerializeField] private UISkillTreeSlots spinUnlockButton;
    [SerializeField] private float hitCooldown = .8f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = 1;


    [Header("Skill Info")]
    [SerializeField] private UISkillTreeSlots swordUnlockButton;
    public bool swordUnlocked { get;private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive Skills")]
    [SerializeField] private UISkillTreeSlots timeStopUnlockButton;
    public bool timeStopUnlock {  get;private set; }
    [SerializeField] private UISkillTreeSlots vulnerableUnlockButton;
    public bool vulnerableUnlock { get;private set; }

    private Vector2 finalDirection;


    [Header("Aim Direction")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();

        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnurable);
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType==SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType==SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetUpBounce(true, amountBounce,bounceSpeed);

        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);

        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);

        newSwordScript.SetupSword(finalDirection, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Unlock Region

    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unLocked)
            timeStopUnlock = true;
    }

    private void UnlockVulnurable()
    {
        if (vulnerableUnlockButton.unLocked)
            vulnerableUnlock = true;
    }



    private void UnlockSword()
    {
        if (swordUnlockButton.unLocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }
    private void UnlockBounceSword()
    {
        if(bounceUnlockButton.unLocked)
            swordType= SwordType.Bounce;
    }
    private void UnlockPierceSword()
    {
        if(pierceUnlockButton.unLocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSpinSword()
    {
        if(spinUnlockButton.unLocked)
            swordType = SwordType.Spin;
    }


    #endregion
    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {

        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
