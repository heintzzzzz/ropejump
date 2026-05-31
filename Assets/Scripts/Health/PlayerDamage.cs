using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{    
    private Health playerHealth;
    protected Character character;
    protected Animator animator;

    private void Start() 
    {
        playerHealth = GetComponent<Health>();    
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //other.gameObject.layer;
        int layerIndex = other.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerIndex);

        // Debug.Log(layerIndex + "AAAAAAA " + other +" Damage_____" + layerName); 
        
        // if (other.CompareTag("Danger") || other.CompareTag("Enemy") || other.CompareTag("Player")) 
        if (other.CompareTag("Danger")) 
        {
            if (playerHealth != null)
            { 
                int damageToApply = other.GetComponent<HazardZone>().damagePerTick;
                // Debug.Log(damageToApply + "PlayerDamage____Danger" + other);   
                playerHealth.TakeDamage(damageToApply); 
            }
        }
        
        /*if (layerName == "ProjectileDisabled")    
        {
            if (other.CompareTag("EnemyProjectile"))
            {
                Debug.Log("Блокировано событие вражеский снаряд");    
            }

            Debug.Log("Блокированое событие");
            return;
        }*/
        
        /*
        if (layerName == "ProjectileDisabled")     
        {
            if (other.CompareTag("EnemyProjectile"))
            {
                Debug.Log("Блокировано событие вражеский снаряд");    
            }

            Debug.Log("Блокированое событие");
            return;
        }
        
        if (other.CompareTag("PlayerMelee")) 
        {
            Debug.Log("PlayerDamage_____PlayerMelee" + other); 
        }
        
        if (playerHealth != null) {
            Debug.Log("PlayerDamage_____playerHealth" + other);   
            if (other.CompareTag("EnemyProjectile")) 
            {
                CharComponents cc = other.GetComponent<CharComponents>();
                Debug.Log(other + "test____" + cc);
        
                Animator anim = cc.animator;  
                
                Debug.Log("PlayerDamage_____EnemyProjectile" + other);  
                int damageToApply = other.GetComponent<Projectile>().currentDamage;
                playerHealth.TakeDamage(damageToApply);

				if (anim != null) { 
					Debug.Log("AAAAAAAAAAAAAAAAA");
				} else {
					Debug.Log("BBBBBBBBBBBBB");
				}
            } else if (other.CompareTag("EnemyMelee")) {
                var data = other.GetComponent<MeleeWeapon>().weaponDataItem; 
                if (data != null)
                { 
                    int damageToApply = (data != null) ? data.WeaponDamage : 0;
                } 
            }
            else
            {
                Debug.Log("PlayerDamage____xxxxxxxxxx444444444" + other);   
            }
        }*/ 

    }
}