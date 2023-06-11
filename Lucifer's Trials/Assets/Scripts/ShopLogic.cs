using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopLogic : MonoBehaviour
{
    // Get player wallet
    private int walletQuantity;
    [SerializeField] private TMP_Text walletText;
    [SerializeField] private Button shopButton1;
    [SerializeField] private Button shopButton2;
    [SerializeField] private Button shopButton3;
    [SerializeField] private TMP_Text validBuyText;
    [SerializeField] private TMP_Text invalidBuyText;
    private float buyTextDuration = 1.5f;
    private float currentTime;
    private bool madePurchase;

    private int item1Cost = 300;
    private int item2Cost = 50;
    private int item3Cost = 300;


    void Start()
    {
        walletQuantity = GameObject.Find("Player").GetComponent<PlayerController>().GetWallet();
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
                if (walletQuantity < item1Cost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    Debug.Log("Bought 1");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(item1Cost);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
            case 2:
                if (walletQuantity < item2Cost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    Debug.Log("Bought 2");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(item2Cost);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
                }
                break;
            case 3:
                if (walletQuantity < item3Cost)
                {
                    this.invalidBuyText.gameObject.SetActive(true);
                    this.validBuyText.gameObject.SetActive(false);
                    FindObjectOfType<SoundManager>().PlaySoundEffect("Bad Select");
                }
                else
                {
                    Debug.Log("Bought 3");
                    this.invalidBuyText.gameObject.SetActive(false);
                    this.validBuyText.gameObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<PlayerController>().DecreaseWallet(item3Cost);
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
        FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
    }

    void Update()
    {
        walletQuantity = GameObject.Find("Player").GetComponent<PlayerController>().GetWallet();
        this.walletText.text = this.walletQuantity.ToString();

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
