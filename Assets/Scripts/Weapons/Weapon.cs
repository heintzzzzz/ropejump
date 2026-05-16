using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Name")]
    [SerializeField] private string weaponId = "";
    [SerializeField] private string weaponName = "";

    [Header("Settings")]
    [SerializeField] private float timeBtwShots = 0.5f;

    [Header("Weapon")]
    [SerializeField] public bool useMagazine = true;
    [SerializeField] public int magazineSize = 10;
    [SerializeField] public bool autoReload = true;

    [SerializeField] public int weaponMode;

    [Header("Recoil")]
    [SerializeField] private bool useRecoil = true;
    [SerializeField] private int recoilForce = 5;
    
    [Header("Effects")] 
    [SerializeField] public ParticleSystem muzzlePS;

    public string WeaponId => weaponId;
    public string WeaponName => weaponName;
    public bool UseMagazine => useMagazine;
    public int MagazineSize => magazineSize;
    public float TimeBtwShots => timeBtwShots;
    
    public Character WeaponOwner { get; set; } // ссылка на объект персонажа

    public int CurrentAmmo {get; set;} // текущее кол-во патронов
    public bool CanShoot { get; set; } // блокировка по таймеру
    public WeaponAmmo WeaponAmmo {get; set;} // используемое оружие
    public WeaponAim WeaponAim {get;set;} // прицеливаемое
    protected readonly int weaponUseParameter = Animator.StringToHash("WeaponUse");  
    protected readonly int weaponReloadParameter = Animator.StringToHash("Reloading"); 
    private CharController controller;
    public Animator animator;
    private float nextShotTime;
    public bool isPlayerWeapon = false;
    public GameObject progressBar;
    public WeaponDataItem weaponDataItem;
    
    protected virtual void Awake()
    {
        WeaponAmmo = GetComponent<WeaponAmmo>();
        WeaponAim = GetComponent<WeaponAim>();
        animator = GetComponent<Animator>();
Debug.Log("Start1" + animator);
        if (weaponDataItem != null)
        {
            magazineSize = weaponDataItem.MagazineSize;
        }
    }
    
    protected virtual void Update()
    {
        WeaponCanShoot();
        RotateWeapon();
    }
    
    protected virtual void WeaponCanShoot()
    {
        if (Time.time > nextShotTime)
        {
            CanShoot = true;
            nextShotTime = Time.time + timeBtwShots;
        }
    }

    public virtual void UseWeapon()
    {
        StartShooting(); 
    }
    
    public void StartShooting()
    {
        if (useMagazine)
        {
            if (WeaponAmmo != null)
            {
                if (WeaponAmmo.CanUseWeapon())
                {
                    RequestShot();
                }
                else
                {
                    if (autoReload)
                    {
                        Reload();
                    }
                }
            }
        }
        else 
        {
            RequestShot();
        }
    }
    
    protected virtual void RequestShot()
    {
        if (!CanShoot) 
        {
            return;
        }
        
        if (useRecoil)
        {
            Recoil();
        }

        // animator.SetTrigger(weaponUseParameter); 
        
        if (useMagazine)
        {
            WeaponAmmo.ConsumeAmmo();
        }
    }
    
    public void Reload()
    {
        if (WeaponAmmo != null)
        {
            if (useMagazine)
            {
                WeaponAmmo.RefillAmmo();
            }
        }
    }
    
    protected virtual void RotateWeapon()
    {
        if (WeaponOwner.GetComponent<CharFlip>().FacingRight) 
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    private void Recoil()
    {
        if (WeaponOwner != null)
        {
            /*
            if (WeaponOwner.GetComponent<CharFlip>().FacingRight)
            {
                 controller.ApplyRecoil(Vector2.left, recoilForce);
            }
            else
            {
                 controller.ApplyRecoil(Vector2.right, recoilForce);
            }
            */
        }
    }
    
    public void StopWeapon()
    {
        if (useRecoil)
        {
            // controller.ApplyRecoil(Vector2.one, 0f);
        }
    }
    
    public void SetOwner(Character owner)
    {
        WeaponOwner = owner;
        controller = WeaponOwner.GetComponent<CharController>();
        if (WeaponOwner != null && WeaponOwner.CharacterType == MyLibrary.CharacterTypes.Player)
        {
            isPlayerWeapon = true;
        }
    } 
}
