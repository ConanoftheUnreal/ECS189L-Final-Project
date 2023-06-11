using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopLogic : MonoBehaviour
{
    // Get player wallet
    private int walletQuantity;
    private int attackStat;
    private int healthStat;
    private int speedStat;
    [SerializeField] private TMP_Text walletText;
    [SerializeField] private TMP_Text attackStatText;
    [SerializeField] private TMP_Text healthStatText;
    [SerializeField] private TMP_Text speedStatText;
    [SerializeField] private Button shopButton1;
    [SerializeField] private Button shopButton2;
    [SerializeField] private Button shopButton3;
    [SerializeField] private TMP_Text validBuyText;
    [SerializeField] private TMP_Text invalidBuyText;
    [SerializeField] private TMP_Text maxHealthText;
    private float buyTextDuration = 1.5f;
    private float currentTime;
    private bool madePurchase;

    private int attackUpgradeCost = 300;
    private int healingCost = 50;
    private int speedUpgradeCost = 300;


    void Start()
    {
        this.walletQuantity = GameObject.Find("Player").GetComponent<PlayerController>().GetWallet();
        this.attackStat = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
        this.healthStat = GameObject.Find("Player").GetComponent<PlayerController>().GetHealth();
        this.speedStat = GameObject.Find("Player").GetComponent<PlayerController>().GetSpeed();
        this.walletText.text = this.walletQuantity.ToString();
        this.shopButton1.onClick.AddListener(delegate {Buy(1); });
        this.shopButton2.onClick.AddListener(delegate {Buy(2); });
        this.shopButton3.onClick.AddListener(delegate {Buy(3); });
    }

    public void Buy(int option)
    {
        switch (option)
        {
            case 1:
                if (walletQuantity < attackUpgradeCost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    this.maxHealthText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    Debug.Log("Bought 1");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    this.maxHealthText.gameObject.SetActive(false);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(attackUpgradeCost);
                    GameObject.Find("Player").GetComponent<PlayerController>().IncreaseAttack(1);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
            case 2:
                if (walletQuantity < healingCost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    this.maxHealthText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else if (GameObject.Find("Player").GetComponent<PlayerController>().AtMaxHealth())
                {
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(false);
                    this.maxHealthText.gameObject.SetActive(true);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    Debug.Log("Bought 2");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    this.maxHealthText.gameObject.SetActive(false);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(healingCost);
                    GameObject.Find("Player").GetComponent<PlayerController>().IncreaseHealth(1);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
            case 3:
                if (walletQuantity < speedUpgradeCost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    this.maxHealthText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    Debug.Log("Bought 3");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    this.maxHealthText.gameObject.SetActive(false);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(speedUpgradeCost);
                    GameObject.Find("Player").GetComponent<PlayerController>().IncreaseSpeed(1);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
        }
        this.currentTime = 0.0f;
        this.madePurchase = true;
    }

    public void ExitShop()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        this.invalidBuyText.gameObject.SetActive(false);
        this.validBuyText.gameObject.SetActive(false);
        this.maxHealthText.gameObject.SetActive(false);
        FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
    }

    void Update()
    {
        this.walletQuantity = GameObject.Find("Player").GetComponent<PlayerController>().GetWallet();
        this.attackStat = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
        this.healthStat = GameObject.Find("Player").GetComponent<PlayerController>().GetHealth();
        this.speedStat = GameObject.Find("Player").GetComponent<PlayerController>().GetSpeed();
        this.walletText.text = this.walletQuantity.ToString();
        this.attackStatText.text = this.attackStat.ToString();
        this.healthStatText.text = this.healthStat.ToString();
        this.speedStatText.text = this.speedStat.ToString();

        if (currentTime >= buyTextDuration && this.madePurchase)
        {
            this.invalidBuyText.gameObject.SetActive(false);
            this.validBuyText.gameObject.SetActive(false);
            this.maxHealthText.gameObject.SetActive(false);
            this.currentTime = 0.0f;
            this.madePurchase = false;
        }
        else if (this.madePurchase)
        {
            // Unscaled because we've paused the game to open the shop
            this.currentTime += Time.unscaledDeltaTime;
        }
    }
}
