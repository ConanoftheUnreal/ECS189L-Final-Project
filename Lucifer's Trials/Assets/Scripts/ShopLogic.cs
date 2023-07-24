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
    [SerializeField] private TMP_Text attackCost;
    [SerializeField] private TMP_Text healthCost;
    [SerializeField] private TMP_Text speedCost;
    [SerializeField] private Button shopButton1;
    [SerializeField] private Button shopButton2;
    [SerializeField] private Button shopButton3;
    [SerializeField] private TMP_Text validBuyText;
    [SerializeField] private TMP_Text invalidBuyText;
    private float buyTextDuration = 1.5f;
    private float currentTime;
    private bool madePurchase;

    private ShopCostContainer shopCosts = ShopCostContainer.Instance;
    private int priceIncrease = 100;


    void Start()
    {
        this.walletQuantity = GameObject.Find("Player").GetComponent<PlayerController>().GetWallet();
        this.attackStat = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
        this.healthStat = GameObject.Find("Player").GetComponent<PlayerController>().GetMaxHealth();
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
                if (walletQuantity < this.shopCosts.attackCost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    //Debug.Log("Bought 1");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(this.shopCosts.attackCost);
                    GameObject.Find("Player").GetComponent<PlayerController>().IncreaseAttack(1);
                    this.shopCosts.attackCost += this.priceIncrease + this.priceIncrease * this.shopCosts.attackBought;
                    this.shopCosts.attackBought += 1;
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
            case 2:
                if (walletQuantity < this.shopCosts.healthCost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    //Debug.Log("Bought 2");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(this.shopCosts.healthCost);
                    GameObject.Find("Player").GetComponent<PlayerController>().IncreaseMaxHealth(1);
                    this.shopCosts.healthCost += this.priceIncrease + this.priceIncrease * this.shopCosts.healthBought;
                    this.shopCosts.healthBought += 1;
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
            case 3:
                if (walletQuantity < this.shopCosts.speedCost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    //Debug.Log("Bought 3");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(this.shopCosts.speedCost);
                    GameObject.Find("Player").GetComponent<PlayerController>().IncreaseSpeed(1);
                    this.shopCosts.speedCost += this.priceIncrease + this.priceIncrease * this.shopCosts.speedBought;
                    this.shopCosts.speedBought += 1;
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
        Cursor.visible = false;
        this.gameObject.SetActive(false);
        this.invalidBuyText.gameObject.SetActive(false);
        this.validBuyText.gameObject.SetActive(false);
        FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
    }

    void Update()
    {
        this.walletQuantity = GameObject.Find("Player").GetComponent<PlayerController>().GetWallet();
        this.attackStat = GameObject.Find("Player").GetComponent<PlayerController>().GetAttack();
        this.healthStat = GameObject.Find("Player").GetComponent<PlayerController>().GetMaxHealth();
        this.speedStat = GameObject.Find("Player").GetComponent<PlayerController>().GetSpeed();
        this.walletText.text = this.walletQuantity.ToString();
        this.attackStatText.text = this.attackStat.ToString();
        this.healthStatText.text = this.healthStat.ToString();
        this.speedStatText.text = this.speedStat.ToString();
        this.attackCost.text = this.shopCosts.attackCost.ToString();
        this.healthCost.text = this.shopCosts.healthCost.ToString();
        this.speedCost.text = this.shopCosts.speedCost.ToString();

        if (currentTime >= buyTextDuration && this.madePurchase)
        {
            this.invalidBuyText.gameObject.SetActive(false);
            this.validBuyText.gameObject.SetActive(false);
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
