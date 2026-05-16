using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
 
    [SerializeField] private GameObject reticlePrefub;
    public float CurrentAimAngleAbsolute {get; set;}
    public float CurrentAimAngle {get;set;}

    private Camera mainCamera;
    private GameObject reticle;
    private Weapon weapon;
    private Transform ownerTransform;
    private CharFlip charFlip;

    private Vector3 direction;
    private Vector3 mousePosition;
    private Vector3 reticlePosition;
    private Vector3 currentAim = Vector3.zero;
    private Vector3 currentAimAbsolute = Vector3.zero; 
    private Quaternion initialRotation;
   private Quaternion lookRotation;

    public bool autoAim = false;
    public bool blockAim = false;
    private bool isPlayerWeapon = false;
    
    void Start()
    { 
        Cursor.visible = false;
        weapon = GetComponent<Weapon>(); 
        initialRotation = transform.rotation;
        
        if (weapon.WeaponOwner != null) {
            ownerTransform = weapon.WeaponOwner.transform;
            charFlip = weapon.WeaponOwner.GetComponent<CharFlip>();
            isPlayerWeapon = weapon.WeaponOwner.CharacterType == MyLibrary.CharacterTypes.Player;
        }
        
        mainCamera = Camera.main;
        if (reticlePrefub != null)
        {
            reticle = Instantiate(reticlePrefub);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isPlayerWeapon)
        {
            GetMousePosition();
        }
        else if (autoAim)
        {
           EnemyAim();
        }

        // MoveReticle();
        RotateWeapon();
    }

    private void GetMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane + 1f;

        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 ownerPos = ownerTransform.position;
        
        // Всегда вычисляем абсолютное направление
        currentAim = worldMousePos - ownerPos; 
        currentAim.z = 0;
        
        reticlePosition = worldMousePos;
        
        mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        direction = mainCamera.ScreenToWorldPoint(mousePosition);
        direction.z = transform.position.z; 
        reticlePosition = direction; 

        currentAimAbsolute = direction - ownerTransform.position;
        if (weapon.WeaponOwner.GetComponent<CharFlip>().FacingRight)
        {
            currentAim = direction - ownerTransform.position;
        }
        else
        {
            currentAim = ownerTransform.position - direction;
        }

    }
    
    public void RotateWeapon()
    {
        if (currentAim != Vector3.zero && direction != Vector3.zero)
        {
            //Get Angle
            CurrentAimAngle = Mathf.Atan2(currentAim.y, currentAim.x) * Mathf.Rad2Deg;
            CurrentAimAngleAbsolute = Mathf.Atan2(currentAimAbsolute.y, currentAimAbsolute.x) * Mathf.Rad2Deg;

            if (weapon.WeaponOwner.GetComponent<CharFlip>().FacingRight)
            {
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);

            }
            else
            {
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
            }
        }

        if (currentAim != Vector3.zero)
        {
            // Определяем сторону (относительно игрока)
            Vector3 playerPos = weapon.WeaponOwner.transform.position;
            bool mouseOnRight = currentAim.x > 0f; // мышь справа от персонажа

            float targetAngle = mouseOnRight ? 0f : 180f; // 0° вправо, 180° влево

            // Превращаем в Quaternion
            lookRotation = Quaternion.Euler(0f, 0f, targetAngle);

            if (weapon.WeaponOwner.CharacterType == MyLibrary.CharacterTypes.Player)
            {
                if (!blockAim)
                {
                    weapon.transform.rotation = lookRotation;
                }
            }
            else
            {
                transform.rotation = lookRotation;
            }
        }
        else
        {
            // Если нет направления — сбрасываем
            transform.rotation = initialRotation;
        }
    } 
    
    public void RotateWeapon2()
    {
        if (currentAim != Vector3.zero && direction != Vector3.zero)
        {
            //Get Angle
            CurrentAimAngle = Mathf.Atan2(currentAim.y, currentAim.x) * Mathf.Rad2Deg;
            CurrentAimAngleAbsolute = Mathf.Atan2(currentAimAbsolute.y, currentAimAbsolute.x) * Mathf.Rad2Deg;

            float ang = 0f;
            //Clamp our rotation
            if (weapon.WeaponOwner.GetComponent<CharFlip>().FacingRight)
            {
                ang = 0f;
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
                
            } 
            else
            {
                ang = -0f;
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
            }
            
            // Applying out angle;
            // lookRotation = Quaternion.Euler(CurrentAimAngle * Vector3.forward);
            lookRotation = Quaternion.Euler(CurrentAimAngle * Vector3.forward);

            if (weapon.WeaponOwner.CharacterType == MyLibrary.CharacterTypes.Player) {
                if (!blockAim)
                {
                    weapon.transform.rotation = lookRotation; 
                }    
            } else {
                transform.rotation = lookRotation;
            }
        }
        else
        {
                CurrentAimAngle = 0f; 
                transform.rotation = initialRotation;
        } 
    } 
    
    /*public void RotateWeapon()
    { 
        if (currentAim != Vector3.zero && direction != Vector3.zero)
        {
            //Get Angle
            CurrentAimAngle = Mathf.Atan2(currentAim.y, currentAim.x) * Mathf.Rad2Deg;
            CurrentAimAngleAbsolute = Mathf.Atan2(currentAimAbsolute.y, currentAimAbsolute.x) * Mathf.Rad2Deg;

            //Clamp our rotation
            if (weapon.WeaponOwner.GetComponent<CharFlip>().FacingRight)
            {
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
            } 
            else
            {
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
            }
            
            // Applying out angle;
            lookRotation = Quaternion.Euler(CurrentAimAngle * Vector3.forward);
            transform.rotation = lookRotation;
        }
        else 
        { 
            CurrentAimAngle = 0f;
            transform.rotation = initialRotation;
        }  
        
        
        /*if (currentAim == Vector3.zero) 
        {
            transform.rotation = initialRotation;
            return;
        }

        // Вычисляем абсолютный угол
        CurrentAimAngleAbsolute = Mathf.Atan2(currentAim.y, currentAim.x) * Mathf.Rad2Deg;
        
        // Корректируем угол для отражённого персонажа
        CurrentAimAngle = CurrentAimAngleAbsolute;
        
        if (charFlip && !charFlip.FacingRight)
        {
            // Инвертируем угол при отражении персонажа
            CurrentAimAngle = (CurrentAimAngleAbsolute + 180f) % 360f;
        }

        // Применяем ограничения угла
        CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180f, 180f);

        // Создаём вращение
        Quaternion lookRotation = Quaternion.AngleAxis(CurrentAimAngle, Vector3.forward);

        // Применяем вращение с учётом блокировки
        if (!blockAim || !isPlayerWeapon) 
        {
            transform.rotation = lookRotation;
            
            // Корректируем scale для отражённого оружия
            if (charFlip && !charFlip.FacingRight)
            {
                transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }#1#
        
        /*if (currentAim != Vector3.zero && direction != Vector3.zero)
        {
            //Get Angle
            CurrentAimAngle = Mathf.Atan2(currentAim.y, currentAim.x) * Mathf.Rad2Deg;
            CurrentAimAngleAbsolute = Mathf.Atan2(currentAimAbsolute.y, currentAimAbsolute.x) * Mathf.Rad2Deg;

            //Clamp our rotation
            if (weapon.WeaponOwner.GetComponent<CharFlip>().FacingRight)
            {
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
            } 
            else
            {
                CurrentAimAngle = Mathf.Clamp(CurrentAimAngle, -180, 180);
            }
            
            // Applying out angle;
            lookRotation = Quaternion.Euler(CurrentAimAngle * Vector3.forward);

            if (isPlayerWeapon) { 
                if (!blockAim)
                { 
                    weapon.transform.rotation = lookRotation;
                }   
            } else {
                transform.rotation = lookRotation;
            }
        }
        else
        {
            transform.rotation = initialRotation;
        } #1#
    }*/

    public void EnemyAim()  
    {
        
        /*if (!weapon.WeaponTarget) return;
        
        Vector3 targetPos = weapon.WeaponTarget.transform.position;
        Vector3 ownerPos = ownerTransform.position;
        
        currentAim = targetPos - ownerPos;
        currentAim.z = 0;
        
        reticlePosition = targetPos; */
        
        // currentAimAbsolute = currentAim; 
        currentAim = weapon.WeaponOwner.GetComponent<CharFlip>().FacingRight ? currentAim : -currentAim;
        direction = currentAim - transform.position;
    }

    public void SetAim(Vector2 newAim)
    {
        currentAim = newAim;
    }

    private void MoveReticle()
    {
        if (reticle != null) {
            reticle.transform.rotation = Quaternion.identity;
            reticle.transform.position = reticlePosition;
        }
    }

    public void DestroyReticle()
    {
        if (reticle != null)
        {
            Destroy(reticle.gameObject); 
        }
        else
        {
            return;
        }
    }

    public void InitReticle()
    {
        if (reticle != null) return;
        if (reticlePrefub != null)
        {
            reticle = Instantiate(reticlePrefub);
        }
    }

    public void BlockAim(bool value)
    {
        blockAim = value;
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + currentAim.normalized * 2f);
        
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(reticlePosition, 0.2f);
        }
    }
}