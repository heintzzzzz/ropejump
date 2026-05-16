using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; 

  
public class CharWeapon : CharComponents
{
    public static Action OnStartShooting;

    [Header("Weapon Settings")]
    [SerializeField] public String weaponToUse;
    [SerializeField] public Transform weaponHolderPosition;

    public Weapon CurrentWeapon {get; set;}
    public int WeaponType; 

    public WeaponAim WeaponAim {get;set;}

    public bool reloading;
    
    public bool isChanging;
    public float changingTime = 0f;
    
    private float reloadDuration; 

    private float changeWeaponTimout = 0.3f; // таймаут на переключение оружия что бы не было глюков
    
    protected override void Start() 
    { 
        base.Start();
        EquipWeapon(weaponToUse);
        
        reloading = false;
        isChanging = false;
    }

    protected override void HandleInput()
    {
        if (character.CharacterType == MyLibrary.CharacterTypes.Player)
        {
            if (Input.GetMouseButtonDown(0) && reloading == false)
            {
                Shoot(); 
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && reloading == false) 
            {
                Reload(); 
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                bool contains = LevelManager.Instance._checkWeaponAvailable("fists");
                if (reloading == false && isChanging == false) { 
                    if (CurrentWeapon == null || (CurrentWeapon && CurrentWeapon.WeaponName != "fists" && contains)) 
                    {
                        EquipWeapon("fists");
                        updateChanging(changeWeaponTimout);
                    } 
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) 
            {
                bool contains = LevelManager.Instance._checkWeaponAvailable("bottle");
                if (reloading == false && isChanging == false) { 
                    if (CurrentWeapon == null || (CurrentWeapon && CurrentWeapon.WeaponName != "bottle" && contains)) 
                    {
                        EquipWeapon("bottle");
                        updateChanging(changeWeaponTimout);
                    }
                }
            }
        }

        if (WeaponType == 1)
        {
            if (CurrentWeapon != null)
            {
                WeaponAim = CurrentWeapon.GetComponent<WeaponAim>();
                if (WeaponAim != null) 
                { 
                    if (reloading != false)
                    {
                        WeaponAim.BlockAim(true);
                        Quaternion angleR = Quaternion.Euler(0f, 0f, 0f);
                        CurrentWeapon.transform.rotation = angleR;
                    }
                    else
                    {
                        WeaponAim.BlockAim(false); 
                    }
                }   
            }

        }

        if (isChanging == true && changingTime < Time.time)
        {
           isChanging = false;
        }
    }

    public void Shoot()
    {
        if (CurrentWeapon == null)
        {
            return;
        }

        CurrentWeapon.UseWeapon();

        if (character.CharacterType == MyLibrary.CharacterTypes.Player)
        {


            OnStartShooting?.Invoke();
            LevelManager.Instance.UpdateItemAmmo(CurrentWeapon.WeaponName, CurrentWeapon.CurrentAmmo);
        }
    }

    public void StopWeapon()
    {
        if (CurrentWeapon == null)
        {
            return;
        }
        CurrentWeapon.StopWeapon();
    }

    public IEnumerator WeaponReloadingAnimation(Animator anim) {
        anim.SetBool("Reloading", true);
        string weaponType = CurrentWeapon.WeaponName;
        int currentAmmo = CurrentWeapon.CurrentAmmo;
        int currentFullAmmo = LevelManager.Instance.LoadAmmoAmount(weaponType);
        GameObject progressBar = CurrentWeapon.progressBar;
        float lerpDuration = 0.86f;
        if (anim != null && anim.GetCurrentAnimatorClipInfo (0) != null) {
            while (anim.GetCurrentAnimatorClipInfo(0).Length == 0) {
                print ("*******THE ARRAY IS ZERO, LETS WAIT");
                // print ("*******THE ARRAY IS ZERO, LETS WAIT");
                yield return null;
            }  
            AnimationClip currentClip = anim.GetCurrentAnimatorClipInfo(0)[0].clip;
            float currentCliplenght = currentClip.length;
            
            if (progressBar != null) { 
                progressBar.SetActive(true); 
                int val = 100;

                Image bar = progressBar.transform.GetChild(1).GetComponent<Image>();

                if (bar != null) { 
                    float startValue = 0f;  
                    float endValue = 1f;   
                    StartCoroutine(MyLibrary.ReloadProgress(bar, lerpDuration, startValue, endValue));
                    yield return new WaitForSeconds(currentCliplenght); 
                }                  
            }
            else
            {
              yield return new WaitForSeconds(currentCliplenght); 
            } 
            
            anim.SetBool("Reloading", false);
            reloading = false;
            if (progressBar != null) progressBar.SetActive(false);
        }

        int diffNeeded = CurrentWeapon.MagazineSize - currentAmmo;  
        int plusAmmo = ((currentFullAmmo - diffNeeded) <= 0) ? currentFullAmmo : diffNeeded;  
        int newCurrentAmmo = currentAmmo + plusAmmo;
        int newFullAmmo = ((currentFullAmmo - diffNeeded) <= 0) ? 0 : currentFullAmmo - diffNeeded;     

        LevelManager.Instance.SaveAmmo(weaponType, newCurrentAmmo);
        LevelManager.Instance.SaveAmmoAmount(weaponType, newFullAmmo);

        CurrentWeapon.CurrentAmmo = newCurrentAmmo;
        // SoundManager.Instance.PlaySound(SoundManager.Instance.ReloadGunClip, 0.8f);
        LevelManager.Instance.UpdateItemAllAmmoAmount(weaponType, newCurrentAmmo, newFullAmmo);
    } 
    
    public void Reload() 
    {
        if (CurrentWeapon == null)
        {
            return;
        }

        if (character.CharacterType == MyLibrary.CharacterTypes.Player)
        {
            string weaponType = CurrentWeapon.WeaponName;
            int currentAmmo = CurrentWeapon.CurrentAmmo;
            int currentFullAmmo = LevelManager.Instance.LoadAmmoAmount(weaponType);

            if (CurrentWeapon.autoReload) 
            {
                CurrentWeapon.Reload();
            }
            else
            {
                if (currentAmmo < CurrentWeapon.MagazineSize) { 
                    if (currentFullAmmo > 0) {
                        float speenAnim = 1.0f;
                    
                        reloading = true;
                    
                        StartCoroutine(WeaponReloadingAnimation(CurrentWeapon.animator));
                        AnimatorClipInfo[] stInfos = CurrentWeapon.animator.GetCurrentAnimatorClipInfo(0);
                    } else {
                        // Play empty sound
                    }  
                }   
            }
        } else {
            CurrentWeapon.Reload();
        }
    }

    public IEnumerator setProgress (float value) {
        if (CurrentWeapon.progressBar != null) {
            CurrentWeapon.progressBar.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
    }
    
    public void updateChanging(float inter)
    {
        changingTime = Time.time + inter;
        isChanging = true;
    }

    
    
    public void EquipWeapon(String weaponT)
    {
        if (CurrentWeapon != null)
        {
            WeaponAim = CurrentWeapon.GetComponent<WeaponAim>();

            if (CurrentWeapon.isPlayerWeapon)
            {
                string weaponType = CurrentWeapon.WeaponName;
                LevelManager.Instance.SaveAmmo(weaponType, CurrentWeapon.CurrentAmmo);
                WeaponAim?.DestroyReticle();
                Destroy(GameObject.Find("Pool"));
                Destroy(CurrentWeapon.gameObject);
            }
            else {
                // Destroy(GameObject.Find("Pool"));
                Destroy(CurrentWeapon.gameObject); 
                // CurrentWeapon.CurrentAmmo = weapon.MagazineSize;
            }
        }
        
        if (weaponT != null) { 
            WeaponDataItem weaponData = LevelManager.Instance._getDataByType(weaponT);
            if (!weaponData) return;

            if (character.CharacterType != MyLibrary.CharacterTypes.Player) LevelManager.Instance.ChangeWeapon(weaponData.WeaponName);
            WeaponType = weaponData.weaponMode; 
            Quaternion angleR = Quaternion.Euler(0f, 0f, 0f); 

            if (weaponHolderPosition != null)
            {
                
                CurrentWeapon = Instantiate(weaponData.WeaponToEquip, weaponHolderPosition.position, angleR);
                CurrentWeapon.transform.parent = weaponHolderPosition; 
                CurrentWeapon.SetOwner(character); 
                CurrentWeapon.WeaponOwner.GetComponent<CharFlip>().SetFlipMode(weaponData.weaponMode);
                WeaponAim = CurrentWeapon.GetComponent<WeaponAim>(); 


                Animator weaponAnimator = CurrentWeapon.GetComponent<Animator>();

                // if (animController != null && weaponAnimator != null) animController.setWeaponAnimController(weaponAnimator);

                if (CurrentWeapon.isPlayerWeapon)   
                {
                  //   WeaponAim?.InitReticle(); 
                }     

                if (weaponData != null && weaponData.flipMode != null)  {
                    int fl = (weaponData.flipMode == MyLibrary.FlipMode.WeaponDirection) ? 1 : 0;
                    CurrentWeapon.WeaponOwner.GetComponent<CharFlip>().SetFlipMode(fl); 
                }
            }
        }

        if (weaponT != null) {
            if (character.CharacterType == MyLibrary.CharacterTypes.Player)
            {
                LevelManager.Instance.SetCurrentWeaponItem(weaponT);

                if (CurrentWeapon.UseMagazine) {
                    int savedAmmo = LevelManager.Instance.LoadAmmo(weaponT);
                    int savedAvialableAmmo = LevelManager.Instance.LoadAmmoAmount(weaponT);
                   
                    CurrentWeapon.CurrentAmmo = savedAmmo;

                    LevelManager.Instance.UpdateItemAllAmmoAmount(CurrentWeapon.WeaponName, savedAmmo, savedAvialableAmmo);
                }

                WeaponDataItem magObj = LevelManager.Instance._getDataByType(CurrentWeapon.WeaponName);
                if (magObj && magObj.UseMagazine) {
                    CurrentWeapon.magazineSize = magObj.MagazineSize;
                }
            }
        }

    } 

    public void SetWeaponToUse(String weapon)
    {    
        weaponToUse = weapon;
    }

    public void UpdateCurrentAmmoMax(string weaponName, int fullAmmo)
    {
        if (CurrentWeapon != null && CurrentWeapon.UseMagazine) { 
            LevelManager.Instance.SaveAmmoAmount(weaponName, fullAmmo);
            LevelManager.Instance.UpdateItemAmmo(weaponName, CurrentWeapon.CurrentAmmo);
        }
    }
}
