using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class WeaponBlock : MonoBehaviour
{
    [SerializeField] public WeaponDataItem weaponDataItem; 
    [SerializeField] public string weaponId;
    [SerializeField] private string weaponName;

    [SerializeField] private TextMeshProUGUI _currentAmmoTMP;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private bool activeBlock = false;
    private Image _background;


    private void Awake()
    {
        _currentAmmoTMP = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        _weaponImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _background = transform.GetChild(0).GetComponent<Image>();

        setDataItem(weaponDataItem);
    }

    private void Update()
    {
        
    }
    
    public void setDataItem(WeaponDataItem newData) 
    {
        if (newData) 
        {  
            weaponId = newData.WeaponId;
            weaponName = newData.WeaponName;
            _weaponImage.sprite = newData.WeaponSprite;

            int uiCurrentAmmo = 0;

            if (newData?.UseMagazine != false) {
                uiCurrentAmmo = LevelManager.Instance.LoadAmmo(weaponName);
            }

            int uiCurrentAmmoAmount = 0;
            if (newData?.UseMagazine != false) {
                uiCurrentAmmoAmount = LevelManager.Instance.LoadAmmoAmount(weaponName);
            }

            UpdateAmmoCount(weaponName, uiCurrentAmmo, uiCurrentAmmoAmount);

            if (_currentAmmoTMP != null) {
                // int uiCurrentAmmo = UIManager.Instance.getCurrentAmmo();
                //int uiCurrentAmmoAmount = LevelManager.Instance.LoadAmmoAmount("Pistol");
                //int uiCurrentAmmo = LevelManager.Instance.LoadAmmo("Pistol");
                // _currentAmmoTMP.text = weaponDataItem.MagazineSize.ToString() + "/" + uiCurrentAmmo.ToString();
               // _currentAmmoTMP.text = uiCurrentAmmo.ToString() + "/" + uiCurrentAmmoAmount.ToString();
            }
        }
    }

    public void setActive(bool active)
    {
        activeBlock = active;
        if (activeBlock == true) _background.color = new Color(255,0,0,100); 
        else _background.color = new Color(100,0,100,100);  
    }

    public void UpdateAmmoCount(string weaponname, int ammo, int maxAmmo)
    {
         WeaponDataItem weaponType = LevelManager.Instance._getDataByType(weaponname);
         if (weaponType.UseMagazine == false) _currentAmmoTMP.text = " ";
         else _currentAmmoTMP.text = ammo.ToString() + "/" + maxAmmo.ToString();
    }

    public void UpdateBlock(string weaponname)
    {
         // UpdateAmmoCount(weaponname);
    }
}