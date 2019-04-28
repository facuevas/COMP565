using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //handlers
    public float maxHealth;
    public float maxEnergy;
    private float currentHealth;
    private float currentEnergy;

    public float energyRegenRate;
    public float energyRegenAmount;
    private float regenerateEnergyTimer;

    public bool canTakeDamage = false;
    //components
    //PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<PlayerController>();
        currentEnergy = maxEnergy;
        currentHealth = maxHealth;
    }

    void Update()
    {
        regenerateEnergyTimer = Time.time + energyRegenRate;
        RegenEnergy();   
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void UseEnergy(int energy)
    {
        currentEnergy -= energy;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrenyEnergy()
    {
        return currentEnergy;
    }

    public void RegenEnergy()
    {
        if (Time.time < regenerateEnergyTimer && currentEnergy < 100)
        {
            currentEnergy += energyRegenAmount;
        }
    }
}
