using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;


public class UIManager : Singleton<UIManager>
{

    private float playerCurrentHealth;
    private float playerMaxHealth;

    private float playerCurrentShield = 0f;
    private float playerMaxShield;
    private bool isPlayer;

    private int playerCurrentAmmo = 1;
    private int playerMaxAmmo = 1;

    [Header("Settings")] 
    [SerializeField] private Image bar;
    private RectTransform _rectTransform;

    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI currentHealthTMP;

    [SerializeField] private Image shieldBar;
    [SerializeField] private TextMeshProUGUI currentShieldTMP;
    // [SerializeField] private TextMeshProUGUI currentShieldValueTMP;

    [Header("Super weapon name")] 
    [SerializeField] private string _currentSuperWeapon = "";

    [Header("Super weapon count")] 
    [SerializeField] public float currentSuperValue = 0f;
    [SerializeField] public float currentSuperAmount = 0f;

    [Header("Super weapon reloading")] 
    [SerializeField] private TextMeshProUGUI playerCurrentSuperValueTMP;
    [SerializeField] public float currentReload = 0f; 
    [SerializeField] public float amountReload = 0f;
    [SerializeField] private Image superReloadingBar;

    [Header("Super block")] 
    [SerializeField] private GameObject superBlock;

    [Header("Super reloading field")] 
    [SerializeField] private GameObject supField;
    private RectTransform _superFieldRect;
    [SerializeField] private GameObject sb;


    [Header("Block for item")] 
    [SerializeField] private GameObject wb;
    [SerializeField] private GameObject itemBlock;
    
    [Header("Weapon Slots")]
    private List<GameObject> _weaponSlotList = new List<GameObject>();

    [SerializeField] private GameObject itemSlot;

    [SerializeField] private GameObject superSlot;

    [Header("Points")]
    [SerializeField] private TextMeshProUGUI pointsTMP;

    [Header("Weapon Data List")]
    [SerializeField] public WeaponDataItem pistolData; 
    [SerializeField] public WeaponDataItem rifleData;
    [SerializeField] public WeaponDataItem shotgunData;
    [SerializeField] public WeaponDataItem knifeData;
    [SerializeField] public WeaponDataItem grenadeData;

    [SerializeField] public bool SpecMode = false;

    private int cellWidth = 80;
    private int cellHeight;
    
    protected override void Awake() 
    {
        if (bar != null) { 
		_rectTransform = bar.GetComponent<RectTransform>();
        cellHeight = cellWidth / 2;
        
        for (var i = 0; i < 9; ++i)
        {
            int left = (i == 0) ? cellWidth / 2 : (i * cellWidth) + (cellWidth / 2);
            int  bottom = cellWidth / 2;
            Vector3 blockPositionN = new Vector3(_rectTransform.anchoredPosition.x + left, _rectTransform.anchoredPosition.y + bottom); 
            var wSlot = Instantiate(wb, blockPositionN, Quaternion.identity);   
            wSlot.transform.SetParent(bar.transform);  
            _weaponSlotList.Add(wSlot);
        }
        
        // TODO spec things
        Vector3 itemPosition = new Vector3(_rectTransform.anchoredPosition.x + 50, _rectTransform.anchoredPosition.y + 150);
        Vector3 superPosition = new Vector3(_rectTransform.anchoredPosition.x + 150, _rectTransform.anchoredPosition.y + 150);

        // TODO make special weapon bar and items bar
        itemSlot = Instantiate(itemBlock, itemPosition, Quaternion.identity); 
        superSlot = Instantiate(superBlock, superPosition, Quaternion.identity); 

        itemSlot.transform.SetParent(bar.transform);
        superSlot.transform.SetParent(bar.transform);

        string firstWeaponEquiped = LevelManager.Instance.currentItem;
        SetWeaponBlockItem(firstWeaponEquiped);
        ChangeActive(firstWeaponEquiped, true);

        _superFieldRect = supField.GetComponent<RectTransform>();

        for(int i = 0; i < currentSuperAmount; i++) 
        {
            Vector3 shPos = new Vector3(supField.transform.position.x + (((i == 0) ? 14 : 14) + (i * 26)), supField.transform.position.y); 
            GameObject sbSlot = Instantiate(sb, shPos, Quaternion.identity); 
            sbSlot.transform.SetParent(supField.transform); 
        }
		}
    }  

    protected void Update()
    {
        InternalUpdate();
    }

    public void SetWeaponBlockItem(string weaponType)
    {
        int i = getBlockNum(weaponType);
        GameObject itemName = _getSlotObj(i); 
        WeaponDataItem weaponData = LevelManager.Instance._getDataByType(weaponType);
        if (weaponData) itemName.GetComponent<WeaponBlock>().setDataItem(weaponData);
    }   

    private void _setWeaponBlockItemAmmo(string weaponType, int ammo, int maxAmmo)
    {
		if (weaponType != null) {
        	int i = getBlockNum(weaponType);
        	GameObject itemName = _getSlotObj(i);
       		itemName.GetComponent<WeaponBlock>().UpdateAmmoCount(weaponType, ammo, maxAmmo);
		}
    }  

    public void toggleSpecMode() {
        var img = _rectTransform.GetComponent<Image>();
        if (SpecMode == true) {
            SpecMode = false;
            img.color = Color.green;
        } else {
            SpecMode = true;
            img.color = Color.red;
        }
    }

    private GameObject _getSlotObj(int slotIndex)
    {
        return MyLibrary.IsIndexValid(_weaponSlotList, slotIndex) ? _weaponSlotList[slotIndex] : null;
    }
     
    public IEnumerable<GameObject> AllObjects => _weaponSlotList;
    
    private GameObject _getSlot(int blockNumber)
    {
        if (blockNumber == 5)
        {
            return itemSlot;
        }
                else if (blockNumber == 6)
        {
            return superSlot;
        }
        else 
        {
            return itemSlot;
        }
    }

    private void _setItemBlockItem(string itemType) 
    {
        int i = getBlockNum(itemType); 
        GameObject itemName = _getSlot(5);
        // SpecItemData itemData = LevelManager.Instance._getDataByItemType(itemType);
        // if (itemData) itemName.GetComponent<ItemBlock>().setDataItem(itemData);
    }  

    private void _setSuperBlockItem(string superType) 
    {
        GameObject superName = _getSlot(6);
        /*SuperWeaponDataItem superData = LevelManager.Instance._getSuperDataByType(superType);
        if (superData) superName.GetComponent<SuperWeaponBlock>().setDataItem(superData);*/
    }  

    private void _setSuperWeaponBlockItemAmmo(string superWeaponType, int ammo, int maxAmmo)
    {
        int i = getBlockNum(superWeaponType);
        //GameObject superName = _getSlot(6);
        //superName.GetComponent<SuperWeaponBlock>().UpdateAmmoCount(superWeaponType, ammo, maxAmmo);
    }  

    private void InternalUpdate()
    {

            // pointsTMP.text = "Points: " + PointsManager.Instance.Points.ToString();

            if (isPlayer) {
                healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, playerCurrentHealth / playerMaxHealth, 10f * Time.deltaTime);
                currentHealthTMP.text = playerCurrentHealth.ToString() + "/" + playerMaxHealth.ToString();
                
                /* Shield things */
                float thisValue = 0.1f;
                if (playerCurrentShield == null || playerCurrentShield == 0 || playerCurrentShield < 0) {
                    thisValue = 0f;
                } else {
                    thisValue = playerCurrentShield / playerMaxShield;
                }

                if (thisValue > 1f) thisValue = 1f;

                shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, thisValue, 10f * Time.deltaTime);
                currentShieldTMP.text = playerCurrentShield.ToString() + "/" + playerMaxShield.ToString();

                // Set reloading super weapon
                /*if (_currentSuperWeapon != "") {
                    if (currentSuperValue <= 1) {
                        float curVal = currentReload / amountReload;
                        curVal = curVal + 0.0001f;
                        superReloadingBar.fillAmount = Mathf.Lerp(superReloadingBar.fillAmount, curVal, 10f * Time.deltaTime);
                        playerCurrentSuperValueTMP.text = currentReload.ToString() + "/" + amountReload.ToString();
                    } 
                }*/
            }
    }   

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        playerCurrentAmmo = currentAmmo;
        playerMaxAmmo = maxAmmo;
    } 

    public void UpdateWeaponSprite(Sprite weaponSprite)
    {
        //weaponImage.sprite = weaponSprite;
        //weaponImage.SetNativeSize();
    }

    public int getCurrentAmmo() 
    {
        return playerCurrentAmmo;
    }

    public void UpdateHealth( float currentHealth, float maxHealth, float currentShield, float maxShield, bool isThisMyPlayer)
    {
        playerCurrentHealth = currentHealth;
        playerMaxHealth = maxHealth;
        playerCurrentShield = currentShield;
        playerMaxShield = maxShield;
        isPlayer = isThisMyPlayer;
    }  

    public void UpdateAmmoUI(string weaponName, int ammo, int maxAmmo)
    {
        _setWeaponBlockItemAmmo(weaponName, ammo, maxAmmo);
    }

    public int getBlockNum(string weaponType) 
    {
        if (weaponType == "Knife") 
        {
            return 0;
        } 
        else if (weaponType == "Pistol")
        {
            return 1;
        }
        else if (weaponType == "Shotgun")
        {
            return 2;
        }   
        else if (weaponType == "Rifle") 
        {
            return 3;
        } 
        else if (weaponType == "Grenade")
        {
            return 4; 
        }

       return 0;
    }


    public void SetWeapon(string weaponType) 
    {
        SetWeaponBlockItem(weaponType);
    }

    public void SetItem(string itemType) 
    {
        _setItemBlockItem(itemType);
    }

    public void SetSuperWeapon(string superWeaponType) 
    {
        _currentSuperWeapon = superWeaponType;
        _setSuperBlockItem(superWeaponType);
    }

    public void ChangeActive(string weaponType, bool active)
    {
        int i = getBlockNum(weaponType);
        // GameObject slot = _getSlot(i);
        GameObject slot = _getSlotObj(i);
        slot.GetComponent<WeaponBlock>().setActive(active);
    }

    public void setPlayerShieldThings(float currentShield, float maxShield, bool isThisMyPlayer) {
        playerCurrentShield = currentShield;
        playerMaxShield = maxShield;
        isPlayer = isThisMyPlayer;
    }

    public void setPlayerSuperCurrentAmount(int currentValue, int maxAmount, bool isThisMyPlayer) {
        currentSuperValue = currentValue;
        currentSuperAmount = maxAmount;
        isPlayer = isThisMyPlayer;
    }

    public void SetSuperWeaponValue(string superWeaponName, int value, int maxAmount) {
        currentSuperValue = value;
        currentSuperAmount = maxAmount;
        _setSuperWeaponBlockItemAmmo(superWeaponName, value, maxAmount);
    }    

    //////////
 
    public void SetSuperWeaponValueReloading(string superWeaponName, float rel_value, float rel_amount) { 
        isPlayer = true;
        currentReload = rel_value;
        amountReload = rel_amount; 
    }     

    public void UpdateSuperAmmoUI(string superWeaponName, int ammo, int maxAmmo)
    {
        _setSuperWeaponBlockItemAmmo(superWeaponName, ammo, maxAmmo);
    }

}
