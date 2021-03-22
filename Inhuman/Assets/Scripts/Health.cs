using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    
 
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
       
        
    }
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        { TakeDamage(10); }
    }
     void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Monster") 
        {
            TakeDamage(20);
        
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        CheckDeath();
    }


    void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    
    }

    
}
