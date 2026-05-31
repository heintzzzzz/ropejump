using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharComponents : MonoBehaviour
{
    public Animator animator;
	
    protected CharController controller;
    protected CharMovement charMovement;
    protected Character character;
    protected CharWeapon charWeapon;
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    protected RubberBandController гubberCtrl;

    
    protected float horizontalInput = 0f;
    protected float verticalInput = 0f;
    public bool isPlayer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        controller = GetComponent<CharController>();
        character = GetComponent<Character>();
        charMovement = GetComponent<CharMovement>();
        charWeapon = GetComponent<CharWeapon>();
        rb = GetComponent<Rigidbody2D>(); 
        boxCollider = GetComponent<BoxCollider2D>(); 
        
        animator = GetComponent<Animator>();

        isPlayer = character.CharacterType == MyLibrary.CharacterTypes.Player;

        гubberCtrl = GetComponent<RubberBandController>();
    }

    protected void Update()
    {
        HandleAbility();
    }

    protected virtual void HandleAbility()
    {
        InternalInput();
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        if (isPlayer)
        {

        }    
    } 

    protected virtual void InternalInput()
    {
        if (isPlayer)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical"); 
        }

    }
}