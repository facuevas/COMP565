using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerStats playerStats;
    public Slider healthbar, endurancebar;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = playerStats.GetCurrentHealth() / playerStats.maxHealth;
        endurancebar.value = playerStats.GetCurrenyEnergy() / playerStats.maxEnergy;
        Debug.Log("Healthbar value: " + healthbar.value);
    }
}
