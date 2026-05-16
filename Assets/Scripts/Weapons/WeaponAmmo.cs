using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    private Weapon weapon;

    private readonly string WEAPON_AMMO_SAVELOAD = "Weapon_";

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        LoadWeaponMagazineSize();
    }

    public void LoadWeaponMagazineSize()
    {
        if (weapon.isPlayerWeapon)
        {
            int savedAmmo = LevelManager.Instance.LoadAmmo(weapon.WeaponName);
            int savedAvialableAmmo = LevelManager.Instance.LoadAmmoAmount(weapon.WeaponName);

            weapon.CurrentAmmo = savedAmmo;
            weapon.CurrentAmmo = savedAvialableAmmo;
        }
    }


    public int LoadAmmo()
    {
        return LevelManager.Instance.LoadAmmo(weapon.WeaponName);
    }

    public int LoadAmmoAmount()
    {
        return LevelManager.Instance.LoadAmmoAmount(weapon.WeaponName);
    }
    

    public void SaveAmmo()
    {
        LevelManager.Instance.SaveAmmo(weapon.WeaponName, weapon.MagazineSize);
    }


    public bool CanUseWeapon()
    {
        if (weapon.CurrentAmmo > 0)
        {
            return true;
        }

        return false;
    }

    public void RefillAmmo()
    {
        if (weapon.UseMagazine)
        {
            weapon.CurrentAmmo = weapon.MagazineSize;
        }
    }
 
    public void ConsumeAmmo()
    {
        if (weapon.UseMagazine) weapon.CurrentAmmo -= 1;
    }
}