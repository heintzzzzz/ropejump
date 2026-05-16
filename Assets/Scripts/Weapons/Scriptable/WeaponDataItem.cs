using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon/WeaponDataItem", fileName = "Weapon Data Item")]
public class WeaponDataItem : ScriptableObject
{

    /*public enum FlipMode
    {
        MovementDirection,
        WeaponDirection 
    }*/

    public string WeaponName; 
    public string WeaponId;
    public Sprite WeaponSprite; 
    public int MagazineSize;
    public Weapon WeaponToEquip;
    public int WeaponDamage;
    public bool UseMagazine; 
    public int AmountSizeLimit; 
    public MyLibrary.FlipMode flipMode = MyLibrary.FlipMode.WeaponDirection;
    public bool isItem; 
    public int weaponMode;
} 