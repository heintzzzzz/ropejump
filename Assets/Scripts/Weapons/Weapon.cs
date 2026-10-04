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

    [Header("Targets")]
    [Tooltip("Who this weapon damages. Overridden by CharWeapon target layers when equipped.")]
    [SerializeField] private LayerMask targetLayers;

    public string WeaponId => weaponId;
    public string WeaponName => weaponName;
    public bool UseMagazine => useMagazine;
    public int MagazineSize => magazineSize;
    public float TimeBtwShots => timeBtwShots;
    public WeaponDataItem Data => weaponDataItem;
    public LayerMask TargetLayers { get => targetLayers; set => targetLayers = value; }

    public Character WeaponOwner { get; set; } // ссылка на объект персонажа
    protected CharFlip OwnerFlip { get; private set; }

    // кто нанёс урон: владелец, а если его нет — само оружие
    protected GameObject DamageSource => WeaponOwner != null ? WeaponOwner.gameObject : gameObject;

    // откуда спавнятся эффекты атаки (у стрелкового — дуло)
    protected virtual Transform EffectPoint => transform;

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

        if (weaponDataItem != null)
        {
            ApplyData(weaponDataItem);
        }
    }

    // CharWeapon вызывает при экипировке: оружие целиком настраивается из ассета
    public void Setup(WeaponDataItem data)
    {
        if (data == null) return;
        weaponDataItem = data;
        ApplyData(data);
    }

    protected virtual void ApplyData(WeaponDataItem data)
    {
        weaponId = data.WeaponId;
        weaponName = data.WeaponName;
        timeBtwShots = data.TimeBtwShots;
        useMagazine = data.UseMagazine;
        magazineSize = data.MagazineSize;
        autoReload = data.ReloadType == ReloadType.AutoOnEmpty;
        weaponMode = data.weaponMode;

        if (data.WorldSprite != null)
        {
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.sprite = data.WorldSprite;
        }
    }

    protected virtual void Update()
    {
        WeaponCanShoot();
        RotateWeapon();
    }
    
    protected virtual void WeaponCanShoot()
    {
        if (!CanShoot && Time.time >= nextShotTime)
        {
            CanShoot = true;
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

        // запускаем кулдаун с момента выстрела
        CanShoot = false;
        nextShotTime = Time.time + timeBtwShots;

        if (useRecoil)
        {
            Recoil();
        }

        // animator.SetTrigger(weaponUseParameter); 
        
        if (useMagazine)
        {
            WeaponAmmo.ConsumeAmmo();
        }

        PlayAttackEffects();
        Attack();
    }

    // вызывается только после проверки кулдауна и патронов — конкретная атака в наследниках
    protected virtual void Attack() {}

    private void PlayAttackEffects()
    {
        if (muzzlePS != null)
        {
            muzzlePS.Play();
        }
        else if (weaponDataItem != null)
        {
            MyLibrary.SpawnParticles(weaponDataItem.AttackEffect, EffectPoint.position, EffectPoint.rotation);
        }

        if (weaponDataItem != null && weaponDataItem.AttackSound != null)
        {
            AudioSource.PlayClipAtPoint(weaponDataItem.AttackSound, transform.position);
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
        // оружие без владельца (просто лежит на сцене) не поворачиваем
        if (OwnerFlip == null) return;

        if (OwnerFlip.FacingRight)
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
        OwnerFlip = WeaponOwner.GetComponent<CharFlip>();
        if (WeaponOwner != null && WeaponOwner.CharacterType == MyLibrary.CharacterTypes.Player)
        {
            isPlayerWeapon = true;
        }
    } 
}
