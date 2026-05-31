using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : Singleton<LevelManager>
{
    private readonly string WEAPON_AMMO_SAVELOAD = "Weapon_";
    private readonly string WEAPON_AMMO_AMOUNT_SAVELOAD = "WeaponAmount_";
    
    private readonly string ITEM_AMOUNT_SAVELOAD = "Item_";

    private readonly string SUPER_WEAPON_AMMO_SAVELOAD = "SUPER_Weapon_";
    private readonly string SUPER_WEAPON_AMMO_AMOUNT_SAVELOAD = "SUPER_WeaponAmount_";

    [Header("Player")]  
    [SerializeField] private Character playableCharacter;
    public Transform spawnPosition;
    public Transform Player { get; set; }
 
    [Header("Player status")] 
    [SerializeField] public string currentItem = null;

    [Header("Weapon Data List")]
    /*[SerializeField] public WeaponDataItem pistolData;
    [SerializeField] public WeaponDataItem rifleData;
    [SerializeField] public WeaponDataItem shotgunData;
    [SerializeField] public WeaponDataItem grenadeData;
    [SerializeField] public WeaponDataItem knifeData;
    [SerializeField] public WeaponDataItem axeData;
    [SerializeField] public WeaponDataItem clawsData;
    [SerializeField] public WeaponDataItem enemyRifleData;*/

    [Header("Item Data List")]
    /*
    [SerializeField] public SpecItemData redKeyData;
    [SerializeField] public SpecItemData emptyItemData;
    */

    [Header("Super Weapon Data List")]
    /*[SerializeField] public string currentSuperWeapon = null;    
    [SerializeField] public SuperWeaponDataItem BoomData;
    [SerializeField] public SuperWeaponDataItem OrionData;
    [SerializeField] public SuperWeaponDataItem FreezeData;*/

    public List<string> weaponAvailable = new List<string>();
    public List<string> itemsAvailable = new List<string>();
    public List<string> superWeaponAvailable = new List<string>();

    public GameObject spawnPlace;

    public ParticleSystem impactPS;

    public enum WeaponTypes
    {
        /*Pistol,
        Rifle,
        Shotgun,
        Grenade,
        Knife,
        Axe,
        Claws*/
    }

    public enum ItemTypes 
    {
        /*RedKey,
        YellowKey,
        GreenKey,
        EmptyItem*/ 
    }

    public enum SuperTypes 
    {
        /*BoomData,
        OrionData,
        FreezeData*/
    }

    private bool isPlayer;
    private bool levelComplete;

    private int body = 0;
    private int head = 0;
    private int legs = 0;
    private int hands = 0;

    public enum bTypes{};

    [Header("Armor list")]
    /*[SerializeField] public ArmorData armorData;
    [SerializeField] public ArmorData armorData2;*/

    [Header("Super weapon")]
    [SerializeField] public string superWeapon;

    /*[SerializeField] public SuperWeaponDataItem boomData;
    [SerializeField] public SuperWeaponDataItem freezeData;
    [SerializeField] public SuperWeaponDataItem orionData;*/

    // [SerializeField] public ArmorItemData greenArmor;
    // [SerializeField] public ArmorItemData blueArmor;

    private EffectManager effectManager;
	private RoundManager roundManager;

	public bool started = false;
    
    private void Awake()
    {
        Player = playableCharacter.transform;

        weaponAvailable.Add("Knife");
        weaponAvailable.Add("Pistol"); 
        weaponAvailable.Add("Rifle"); 
        weaponAvailable.Add("Shotgun"); 
        weaponAvailable.Add("Grenade"); 

        SaveAmmo("Grenade", 1); 
        SaveAmmoAmount("Grenade", 20);          
        
        SaveAmmo("Pistol", 0); 
        SaveAmmoAmount("Pistol", 0); 

        SaveAmmo("Shotgun", 0); 
        SaveAmmoAmount("Shotgun", 60);         
        
        SaveAmmo("Rifle", 0); 
        SaveAmmoAmount("Rifle", 0); 

        /*SaveSuperAmmo("Boom", 0); 
        SaveSuperAmmoAmount("Boom", 0); 

        SaveSuperAmmo("Orion", 0); 
        SaveSuperAmmoAmount("Orion", 0); */

        levelComplete = false; 
        isPlayer = true;

        // set default avialable items ui
        foreach (string w_name in weaponAvailable)
        {
            int ammo = LoadAmmo(w_name);
            int ammoAmount = LoadAmmoAmount(w_name);
            UpdateItemWeaponBlock(w_name, ammo, ammoAmount);
        }
        
        effectManager = FindObjectOfType<EffectManager>(); 
        roundManager = FindObjectOfType<RoundManager>();
    }

    private void Update()
    {
        InternalUpdate();
    }

    private void InternalUpdate()
    {
        if (isPlayer) {
          
        }

			if (started == false) {
            	Debug.Log("Нажато Esc222222!");  
				roundManager.StartRound();	
				started = true;
			}
 /*
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Debug.Log("Нажато Esc!");
			if (started == false) {
            	Debug.Log("Нажато Esc222222!");  
				roundManager.StartRound();	
			}
        } 
*/
    }

    private void Die()
    {

    }
    
    private void ReviveCharacter()
    {
        if (playableCharacter.GetComponent<Health>().CurrentHealth <= 0) 
        {
            playableCharacter.GetComponent<Health>().Revive();
            playableCharacter.transform.position = spawnPosition.position;
        }
    }

    public WeaponDataItem _getDataByType(string weaponType) 
    {
        /*if (weaponType == "Pistol") 
        {
            return pistolData;
        } 
        else if (weaponType == "Rifle") 
        {
            return rifleData;
        } 
        else if (weaponType == "Shotgun")
        {
            return shotgunData;
        }
        else if (weaponType == "Knife")
        {
            return knifeData;
        }
        else if (weaponType == "EnemyRifle") 
        {
            return enemyRifleData;
        }        
        else if (weaponType == "Axe") 
        {
            return axeData;
        }         
        else if (weaponType == "Claws") 
        {
            return clawsData;
        }         
        else if (weaponType == "Grenade") 
        {
            return grenadeData;
        } */

       return null;
    }


    /*public SpecItemData _getDataByItemType(string itemType) 
    {
        if (itemType == "RedKey") 
        {
            return redKeyData;
        } 

       return emptyItemData;
    }*/


    public int LoadAmmo(string weaponType)
    {
        return PlayerPrefs.GetInt(WEAPON_AMMO_SAVELOAD + weaponType);
    }

    public int LoadAmmoAmount(string weaponType)
    {
        return PlayerPrefs.GetInt(WEAPON_AMMO_AMOUNT_SAVELOAD + weaponType);
    }
    
    public void SaveAmmo(string weaponName, int amount)
    {
        PlayerPrefs.SetInt(WEAPON_AMMO_SAVELOAD + weaponName, amount);
    }

    public void SaveAmmoAmount(string weaponName, int currentAmount) 
    {
        PlayerPrefs.SetInt(WEAPON_AMMO_AMOUNT_SAVELOAD + weaponName, currentAmount);
    }

    public void UpdateItemAllAmmoAmount(string weaponname, int ammo, int amount)
    {  
        //UIManager.Instance.UpdateAmmoUI(weaponname, ammo, amount); 
    }

    public void UpdateItemWeaponBlock(string weaponname, int ammo, int amount)
    {              
        //UIManager.Instance.SetWeaponBlockItem(weaponname);
        //UIManager.Instance.UpdateAmmoUI(weaponname, ammo, amount);
    }

    public void UpdateItemAmmo(string weaponname, int ammo)
    {
        int maxAmmoAmount = LevelManager.Instance.LoadAmmoAmount(weaponname);    
        //UIManager.Instance.UpdateAmmoUI(weaponname, ammo, maxAmmoAmount);
    }

    public void UpdateItemSuperAmmo(string superweaponname, int ammo)
    {
        // int maxSuperAmmoAmount = LoadSuperAmmoAmount(superweaponname);    
        // UIManager.Instance.UpdateSuperAmmoUI(superweaponname, ammo, maxSuperAmmoAmount);
    }

        public void UpdateItemAllSuperAmmoAmount(string superweaponname, int ammo, int amount)
    {  
       // UIManager.Instance.UpdateSuperAmmoUI(superweaponname, ammo, amount);
    }

    public int LoadAmount(string itemName)
    {
        return PlayerPrefs.GetInt(ITEM_AMOUNT_SAVELOAD + itemName);
    }

    public void SaveAmount(string itemName, int amount)
    {
        PlayerPrefs.SetInt(ITEM_AMOUNT_SAVELOAD + itemName, amount);
    } 

    // подбор патронов
    public void PickupAmmoAmount(string weaponName, int newMaxAmount)
    {
		int currentAmmo = playableCharacter.GetComponent<CharWeapon>().CurrentWeapon.CurrentAmmo;

		Debug.Log("pickkup");
		Debug.Log(currentAmmo);

        int currentMaxAmmo = LoadAmmoAmount(weaponName);

        WeaponDataItem weaponData = _getDataByType(weaponName);
        int newCurrentAmmoAmount = currentMaxAmmo + newMaxAmount;

        if (newCurrentAmmoAmount > weaponData.AmountSizeLimit) newCurrentAmmoAmount = weaponData.AmountSizeLimit;
        SaveAmmoAmount(weaponName, newCurrentAmmoAmount);
        UpdateItemAllAmmoAmount(weaponName, currentAmmo, newCurrentAmmoAmount); 
    } 


    public void SetCurrentWeaponItem(string weaponName)
    {
        currentItem = weaponName;
    }

    // when weapon is picked up
    public void SetWeapon(string weaponName)
    {
        bool contains = _checkWeaponAvailable(weaponName);
        if (!contains) weaponAvailable.Add(weaponName);
        WeaponDataItem weaponData = _getDataByType(weaponName);

        if (weaponData != null && weaponData.UseMagazine != false) {
            int ammo = LoadAmmo(weaponName); 
            int maxammo = LoadAmmoAmount(weaponName);

            // SaveAmmo(weaponName, ammo); 
            // SaveAmmoAmount(weaponName, maxammo); 
        }
       // UIManager.Instance.SetWeapon(weaponName);
    }

    public void ChangeWeapon(string weaponname) { 
        bool contains = _checkWeaponAvailable(weaponname);
        
        CharWeapon charWeapon = playableCharacter.GetComponent<CharWeapon>();
        if (contains && charWeapon && charWeapon.reloading == false){
            WeaponDataItem weaponData = _getDataByType(weaponname);
            if(weaponData != null) {
               // UIManager.Instance.ChangeActive(currentItem, false);
               // currentItem = weaponname;
                // playableCharacter.GetComponent<CharWeapon>().SetWeaponToUse(weaponData.WeaponToEquip);

                charWeapon.SetWeaponToUse(weaponname);
              //  UIManager.Instance.ChangeActive(weaponname, true);
            }
        }
    }

    public bool _checkWeaponAvailable(string name) {
        bool contains = false;
        if (name != null){
            contains = (weaponAvailable.IndexOf(name) >=0);
        }
        return contains;
    } 

    public bool _checkSuperWeaponAvailable(string super_name) {
        bool contains = false;
        if (super_name != null){
            contains = (superWeaponAvailable.IndexOf(super_name) >=0);
        }
        return contains; 
    } 


    public void AddSpecItem(string itemType) 
    {
        int currentAmount = LoadAmount(itemType);
        itemsAvailable.Add(itemType);
        //UIManager.Instance.SetItem(itemType);

        int newCurrentAmount = currentAmount + 1;
        LevelManager.Instance.SaveAmount(itemType, newCurrentAmount);
    }

    public void RemoveSpecItem(string itemType) 
    {
        itemsAvailable.Remove(itemType);
    }

    private void OnEnable()
    {
        GameEvent.OnEventFired += OnEventResponse;
    }
    
    private void OnDisable()
    {
        GameEvent.OnEventFired -= OnEventResponse;
    }

    private IEnumerator EnemySpawn(EventDataItem eventData)
    {
        
        Debug.Log("SPawn ENEMEY");
        print("Done " + eventData.type);

        // effect!!!
        yield return new WaitForSeconds(3f);
        Transform tr = eventData.pos.GetComponent<Transform>();

        // Vector3 thisAngle = eventData.pos.rotation.eulerAngles;
        Vector3 thisAngle = tr.rotation.eulerAngles;
        GameObject enemy = Instantiate(eventData.item, tr.position, Quaternion.Euler(thisAngle.x, thisAngle.y, 0f));

        // StartCoroutine(OnSpawnEnemy(eventData)); 
    }

    private IEnumerator OnSpawnEnemy(EventDataItem eventData)
    {
        float test = 1f;
        yield return new WaitForSeconds(test);

        // after spawn effect? 
    }

    // private void OnEventResponse(MyLibrary.EventTypes obj)
    private void OnEventResponse(object sender, EventDataItem eventData)
    {
        switch (eventData.type)
        {
            case MyLibrary.EventTypes.EnemySpawn: 
                StartCoroutine(EnemySpawn(eventData)); 
                break;
        }
    }


    public void SetCurrentSuperWeaponItem(string superWeaponName)
    {
        /*currentSuperWeapon = superWeaponName;
        setSuperWeapon(superWeaponName);*/ 
    }

/// <summary>
/// 
/// </summary>
/// <param name="superWeaponName"></param>
    public void setSuperWeapon(string superWeaponName) {
        // SuperWeaponDataItem superWeaponData = _getSuperDataByType(superWeaponName);
        // if (superWeaponData != null) {
          // int ammo = LoadAmmo(superWeaponName); 
          // int maxammo = LoadAmmoAmount(superWeaponName);
        // }
        // UIManager.Instance.SetSuperWeapon(superWeaponName);
    }

    /*public SuperWeaponDataItem _getSuperDataByType(string superWeaponType) 
    {
        /*if (superWeaponType == "boom")  
        {
            return boomData;
        } 
        else if (superWeaponType == "orion") 
        {
            return orionData;
        } 
        else if (superWeaponType == "freeze") 
        {
            return freezeData;
        } #1#

       return null;
    }*/

    public void SetPartsValue(string type, int indexValue) {

        switch (type) 
        {
            case "head":
                head = indexValue;
                break;
            case "body":
                body = indexValue;
                break;
            case "legs":
                legs = indexValue;
                break;
            case "hands":
                hands = indexValue;
                break; 
        }
    }

    public void ChangeSuperWeapon(string superweaponname) { 
            /*SuperWeaponDataItem superWeaponData = _getSuperDataByType(superweaponname);
            if(superWeaponData != null) {
                currentSuperWeapon = superweaponname;
                playableCharacter.GetComponent<CharWeapon>().SetSuperWeaponToUse(superweaponname);
            }*/
    }


    public void SetSuperWeaponValue(string superWeaponName, int value, int amount) { 
        //UIManager.Instance.SetSuperWeaponValue(superWeaponName, value, amount); // 
    }

    public void SetSuperWeaponValueReloading(string superWeaponName, float rel_value, float rel_amount) { 
        // UIManager.Instance.SetSuperWeaponValueReloading(superWeaponName, rel_value, rel_amount); // 
    }    

    public void UpdateHealth( bool isThisMyPlayer, float currentHealth, float maxHealth, float currentShield, float maxShield) 
    {
		if (UIManager.Instance != null) UIManager.Instance.UpdateHealth(isThisMyPlayer, currentHealth, maxHealth, currentShield, maxShield);
    }

   public int LoadSuperAmmo(string superWeaponType)
    {
      //  return PlayerPrefs.GetInt(SUPER_WEAPON_AMMO_SAVELOAD + superWeaponType);
      return 0;
    }

    public int LoadSuperAmmoAmount(string superWeaponType)
    {
      //  return PlayerPrefs.GetInt(SUPER_WEAPON_AMMO_AMOUNT_SAVELOAD + superWeaponType);
      return 0;
    }


    public void PickupSuperAmmoAmount(string superWeaponName, int newMaxAmount)
    {
 
        // int currentSuperAmmo = LoadSuperAmmo(superWeaponName);
        // int currentMaxSuperAmmo = LoadSuperAmmoAmount(superWeaponName);

        // SuperWeaponDataItem superWeaponData = _getSuperDataByType(superWeaponName);
        // int newCurrentAmmoAmount = currentMaxSuperAmmo + newMaxAmount;

        // if (newCurrentAmmoAmount > superWeaponData.AmountSizeLimit) newCurrentAmmoAmount = superWeaponData.AmountSizeLimit;

        // SaveSuperAmmoAmount(superWeaponName, newCurrentAmmoAmount);
        // UpdateItemAllSuperAmmoAmount(superWeaponName, currentSuperAmmo, newCurrentAmmoAmount); 
    }

    public void SaveSuperAmmo(string superWeaponName, int amount)
    {
       // PlayerPrefs.SetInt(SUPER_WEAPON_AMMO_SAVELOAD + superWeaponName, amount);
    }

    public void SaveSuperAmmoAmount(string superWeaponName, int currentAmount) 
    {
      //  PlayerPrefs.SetInt(SUPER_WEAPON_AMMO_AMOUNT_SAVELOAD + superWeaponName, currentAmount);
    }

  	public void SpawnShell(string type, Vector2 pos, Vector2 dir, Material mat, int parts)
    {
      //  effectManager.SpawnShell(type, pos, dir, mat, parts);  
    }

}
