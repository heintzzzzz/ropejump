using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public GameObject enemyHealthBarPrefub; 
    [SerializeField] private Vector3 offSet = new Vector3(0f, 1.3f, 0f);
    private GameObject enemyBar; // currentHealtbar instance
    // private Image enemyHealthBar; // healthbar schedule
    private Slider enemyHealthBar; // healthbar schedule 
    private Health enemyHealthComp; 
    private float enemyCurrentHealth = 1f;
    private float enemyMaxHealth = 1f;
    [SerializeField] private TextMeshProUGUI _currentHealthTMP;

    public HealthDataItem healthDataItem;

    void Start()
    {
        enemyHealthComp = GetComponent<Health>();
        if (healthDataItem != null) 
        {
            enemyCurrentHealth = healthDataItem.initialHealth;
            enemyMaxHealth = healthDataItem.maxHealth; 
            
            if (enemyHealthComp != null)
            {
                enemyHealthComp.SetHealthData(enemyCurrentHealth, enemyMaxHealth, false);
            }

        } 
        
        if (enemyHealthBarPrefub != null)
        {
            enemyBar = Instantiate(enemyHealthBarPrefub, transform.position + offSet, Quaternion.identity);
            enemyBar.transform.parent = transform;

			enemyHealthBar = enemyBar.GetComponentInChildren<Slider>(); 
            
            if (enemyHealthBar != null) 
            {
                _currentHealthTMP = enemyBar.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>();      
            } 
        }
    }

    void Update()
    {
        UpdateHealth();
    }

    public void OnTriggerEnter2D(Collider2D other) {

        int layerIndex = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerIndex);

        Debug.Log(layerIndex + "AAAAAAA " + other +" Damage_____" + layerName); 
        
        // if (other.CompareTag("Danger") || other.CompareTag("Enemy") || other.CompareTag("Player")) 
        
/*
		if (other.CompareTag("Danger")) 
        { 
                int damageToApply = other.GetComponent<HazardZone>().damagePerTick;
                Debug.Log(damageToApply + "EnemyDamage____Danger" + other);   
                TakeDamage(damageToApply); 
				return; 
        }
*/  

        /*   if (other.CompareTag("PlayerProjectile") || other.CompareTag("PlayerMelee"))
                {
                    int damageToApply = 0;

                    if (other.CompareTag("PlayerProjectile")) damageToApply = other.GetComponent<Projectile>().currentDamage;

                    if (other.CompareTag("PlayerMelee")) {
                        // WeaponDataItem data = LevelManager.Instance._getDataByType("Fists");
                        // damageToApply = (data != null) ? data.WeaponDamage : 0;
                    }

                    TakeDamage(damageToApply);
                }

        */
    }

    private void TakeDamage(int damage) { 
       // enemyHealth.TakeDamage(damage);
	   
	  //  int value = currentHealth - damage;
		enemyHealthComp.TakeDamage(damage);
	
    }

    private void UpdateHealth() 
    {
        if (enemyHealthBar != null)
        {
        	enemyHealthBar.value = enemyCurrentHealth / enemyMaxHealth;  
            _currentHealthTMP.text = enemyCurrentHealth.ToString() + "/" + enemyMaxHealth.ToString();  
        }
    }

    public void UpdateEnemyHealth(float currentHealth, float maxHealth)
    {
        enemyCurrentHealth = currentHealth;
        enemyMaxHealth = maxHealth;
    }
}