using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float acceleration = 0f;
    public int currentDamage = 0;

    public Vector2 Direction {get;set;}
    public bool FacingRight {get;set;}
    public float Speed {get;set;}
    public Character ProjectileOwner { get; set; }
    
    public CharWeapon charWeapon; 

    public Rigidbody2D myRigidbody2D;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D collider2D;
    protected Vector2 movement;
    protected bool canMove;
    protected Transform transform;
	public GameObject explositon;
	public string layerName; 
	
    protected virtual void Awake()
    {
        Speed = speed;
        FacingRight = true;
        canMove = true;

        myRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        transform = GetComponent<Transform>();
    }


    private void FixedUpdate()
    {
        if (canMove) MoveProjectile();
    }

    public virtual void MoveProjectile()
    {
        movement = Direction * (Speed / 10f) * Time.fixedDeltaTime;
        myRigidbody2D.MovePosition(myRigidbody2D.position + movement);

        Speed += acceleration * Time.deltaTime;
    }

    public void FlipProjectile()
    {
        if (spriteRenderer != null) spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void SetDirection(Vector2 newDirection, Quaternion rotation, bool isFacingRight = true)
    {
        Direction = newDirection;

        if (FacingRight != isFacingRight) FlipProjectile();

        transform.rotation = rotation;
    }

    public void ResetProjectile()
    {
        spriteRenderer.flipX = false;
    }

    public void DisableProjectile()
    {
        canMove = false;
        spriteRenderer.enabled = false;
        collider2D.enabled = false;

    }

    public void EnableProjectile() 
    {
        canMove = true;
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void SetDamageValue(int damage)
    {
        currentDamage = damage;
    }

    public int getDamage()
    {
        return currentDamage;
    }
    
	// Корутина для уничтожения объекта после анимации
	private IEnumerator DestroyAfterAnimation(GameObject obj, Animator animator)
	{
    	// Ждем пока анимация не закончится
    	yield return new WaitForSeconds(MyLibrary.GetAnimationLength(animator, "Boom"));
	}

    public void StartAfterMath(Transform trans)
    {
	    canMove = false;
		Quaternion rotation = Quaternion.Euler(trans.position.x, trans.position.y, 0f);
		
		if (explositon != null) 
		{
		GameObject expl = Instantiate(explositon, trans.position, rotation);  
		
		Animator animator = expl.GetComponent<Animator>();


			if (animator != null)
			{
				animator.SetTrigger("Boom"); // Название триггера в аниматоре
				StartCoroutine(DestroyAfterAnimation(expl, animator));
			}
			else
			{
				// Если анимации нет - уничтожаем объект через 1 секунду
				Destroy(expl, 1f);
			}  
		}
    }

	public virtual void HandleCollision2D(Vector2 collisionNormal) {}
	
	public virtual void OnTriggerEnter2D(Collider2D collision) {} 	
	
	public virtual void CalcGrenadeThrowParams() {}  
	
	public void DisProjectile()
	{
		gameObject.layer = LayerMask.NameToLayer("ProjectileDisabled"); 
	}
}

