using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int health;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


    //this function gets called when the enemy takes damage.
    public void HurtEnemy(int damage)
    {
        currentHealth -= damage;
        Debug.Log(string.Format("Enemy took {0} damage, current health is now {1}", damage, currentHealth));
    }
}
