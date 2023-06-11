using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Slider healthSlider;
    [SerializeField] private Color low;
    [SerializeField] private Color high;

    public void Start()
    {
        // healthSlider is the only child object
        healthSlider = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>();
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;

        healthSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(this.low, this.high, this.healthSlider.normalizedValue);
    }

    void Update()
    {
        int health = this.player.GetComponent<PlayerController>().GetHealth();
        int maxHealth = this.player.GetComponent<PlayerController>().GetMaxHealth();
        SetHealth(health, maxHealth);
    }
}
