using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Character character;
    private CharController controller;
    private Collider2D collider2D;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;
    
    [Header("Health")]
    [SerializeField] private float initialHealth = 20f;
    [SerializeField] private float maxHealth = 25f;
    
    private bool isPlayer;
    
    [Header("Settings")] 
    [SerializeField] private bool destroyObject;
    
    public float CurrentHealth { get; set; }

    private void Awake()
    {
        character = GetComponent<Character>();
        controller = GetComponent<CharController>();
        collider2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        CurrentHealth = initialHealth;

        if (character != null) isPlayer = character.CharacterType == MyLibrary.CharacterTypes.Player;

        UpdateCharacterHealth();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth -= damage;
        UpdateCharacterHealth();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (character != null)
        {
            Debug.Log("DEAD_!_____");
            
            // collider2D.enabled = false;
            spriteRenderer.enabled = false;
            character.enabled = false;
            controller.enabled = false;
            
            rb.gravityScale = 0f;
            

            StateController stateController = GetComponent<StateController>();
            CharWeapon charWeapon = GetComponent<CharWeapon>();
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            Transform transform = GetComponent<Transform>();

            var animator = character.CharacterAnimator;

            /*
            if (animator != null && animator.HasParameter("Fallen"))
            {
                character.CharacterAnimator.SetBool("Fallen", true);
            }
            */
 
            if (enemyHealth != null) 
            {
                //   enemyHealth.enemyHealthBarPrefub.SetActive(false);  

                GameObject bar = GetComponent<Transform>().GetChild(1).gameObject;
                if (bar != null) bar.SetActive(false);  
            }
            
            // if (charWeapon != null) charWeapon.EquipWeapon(null);
            
            // GetComponent<CharMovement>().SetFollow(false, null); 
            
            if (stateController != null) stateController.enabled = false;
        }

        // if (bossBaseShot != null)
        // {
        //     OnBossDead?.Invoke();
        // }
        
        if (destroyObject)
        {
            DestroyObject();
        }
    }
    
    public void Revive()
    {
        if (character != null)
        {
            collider2D.enabled = true;
            spriteRenderer.enabled = true;
            character.enabled = true;
            controller.enabled = true;

            var animator = character.CharacterAnimator;

            /*if (animator != null && animator.HasParameter("Fallen"))
            {
                character.CharacterAnimator.SetBool("Fallen", false);
            }*/ 
            
            rb.gravityScale = 10f; 
        }
        
        gameObject.SetActive(true);

        CurrentHealth = initialHealth;
        
        UpdateCharacterHealth();
    }
    
    public void GainHealth(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        UpdateCharacterHealth();
    }
    
    private void DestroyObject()
    {
        gameObject.SetActive(false);
    }
    
    private void UpdateCharacterHealth() 
    {
        if (enemyHealth != null)
        {
            enemyHealth.UpdateEnemyHealth(CurrentHealth, maxHealth);
        }
        
        if (character && isPlayer)
        {
           //  LevelManager.Instance.UpdateHealth(CurrentHealth, maxHealth, isPlayer);
        }
    }
    
    public void SetHealthData( float currentHealthAmount, float maxHealthAmount, bool isThisMyPlayer){
        if (!isThisMyPlayer)
        {
            CurrentHealth = currentHealthAmount; 
            maxHealth = maxHealthAmount; 
        }
    }
}