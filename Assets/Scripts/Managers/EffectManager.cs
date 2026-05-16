using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    
    /* pool scripts */
    [SerializeField] private ObjectPoolCtrl splashPool;
    [SerializeField] private ObjectPoolCtrl weaponSplashPool; 
    [SerializeField] private ObjectPoolCtrl armorBlockEffectPool;
    [SerializeField] private ObjectPoolCtrl charHitPool;  
    
    /* pool objects */
    [SerializeField] private GameObject splashPoolObj; // prefub for hearts effect
    [SerializeField] private GameObject weaponItemPoolObj; // prefub for weapon take effect
    [SerializeField] private GameObject armorBlockEffectPoolObj; // prefub for blocking by wall, armor, weapon
    [SerializeField] private GameObject charHitPoolObj; // prefub for hit person blood type
    
    
    public bool isPlayer; 
    
    private void Awake()
    {
        GameObject heartPoolObject = Instantiate(splashPoolObj);
        GameObject weaponItemPoolObject = Instantiate(weaponItemPoolObj);

        splashPool = heartPoolObject.GetComponent<ObjectPoolCtrl>();  
        weaponSplashPool = weaponItemPoolObject.GetComponent<ObjectPoolCtrl>();  
        
        GameObject armorBlockEffectPoolObject = Instantiate(armorBlockEffectPoolObj);
        GameObject charHitPoolObject = Instantiate(charHitPoolObj); 

        armorBlockEffectPool = armorBlockEffectPoolObject.GetComponent<ObjectPoolCtrl>();  
        charHitPool = charHitPoolObject.GetComponent<ObjectPoolCtrl>();  
    } 
    
    private void Update()
    {
        InternalUpdate();
    }

    private void InternalUpdate()
    {
        if (isPlayer) {
          
        }
    }
    
    public void SpawnEffect(string type, Vector2 position, Vector2 direction, float timer = 0.1f, int parts = 1, Material material = null)
    {
        switch(type)
        {
            case "SpecEffect": 
                SpawnSpecEffect(position, direction, material, timer);
                break; 
            case "SpecEffect2":  
                SpawnSpecEffect2(position, direction, material, timer);
                break;             
            case "SpawnSplash":  
                SpawnSplash(position, direction, material, timer); 
                break;  
            
            case "BlockSplash":  
                BlockSplash(position, direction, material, timer); 
                break;  
            case "HitSplash":  
                HitSplash(position, direction, material, timer); 
                break;  
        }
    }
    
    public void SpawnSpecEffect(Vector2 position, Vector2 direction, Material material, float timer)
    { 

        Debug.Log("SpawnSpecEffect");
        var obj = splashPool.GetObject();
        var spItem = obj.GetComponent<SpecEffectItem>();  
        if (spItem != null) 
        {
            spItem.setPoolObj(splashPool); 
            Material mat = material;
            if (mat == null) mat = new Material(Shader.Find("Sprites/Default"));
            spItem.Initialize(mat, 0.01f);  
            spItem.Activate(position, direction, timer);  
        }
    } 
    
    public void SpawnSpecEffect2(Vector2 position, Vector2 direction, Material material, float timer)
    { 

        Debug.Log("SpawnSpecEffect2"); 
        var obj = weaponSplashPool.GetObject();
        var spItem = obj.GetComponent<SpecEffectItem>();  
        if (spItem != null) 
        {
            spItem.setPoolObj(weaponSplashPool); 
            Material mat = material; 
            if (mat == null) mat = new Material(Shader.Find("Sprites/Default"));
            spItem.Initialize(mat, 0.01f);  
            spItem.Activate(position, direction, timer);  
        }
    } 
    
    public void BlockSplash(Vector2 position, Vector2 direction, Material material, float timer, int parts = 1)
    {  
        Debug.Log("BlockSplash");
        var obj = armorBlockEffectPool.GetObject();
        var spItem = obj.GetComponent<SpecEffectItem>();   
        if (spItem != null) 
        {
            
            Debug.Log(spItem + "_________Test22");   
            StartCoroutine(SpawnCoroutine(parts, armorBlockEffectPool, obj, position, direction, material)); 
            
            /*spItem.setPoolObj(splashPool); 
            Material mat = material;
            if (mat == null) mat = new Material(Shader.Find("Sprites/Default"));
            spItem.Initialize(mat, 0.01f);  
            spItem.Activate(position, direction, timer);  */
        }
    } 
    
    public void HitSplash(Vector2 position, Vector2 direction, Material material, float timer)
    { 
        Debug.Log("HitSplash");
        var obj = charHitPool.GetObject();
        var spItem = obj.GetComponent<SpecEffectItem>();  
        if (spItem != null) 
        {
            spItem.setPoolObj(charHitPool); 
            Material mat = material;
            if (mat == null) mat = new Material(Shader.Find("Sprites/Default"));
            spItem.Initialize(mat, 0.01f);  
            spItem.Activate(position, direction, timer);  
        }
    }  
    
    public void PlayEffect(string type, Vector2 position, Quaternion ident) {
        
    }
    
    public void SpawnSplash(Vector2 position, Vector2 direction, Material material, float timer, int parts = 1)
    {
        Debug.Log("Test1____________" + position); 
        if (splashPool != null)
        {
            Debug.Log("Test2____________" + position);
            GameObject obj = splashPool.GetObject();
            // StartCoroutine(SpawnCoroutine(parts, charHitPool, obj, position, direction, material)); 
        }
    } 

    
    public IEnumerator SpawnCoroutine(int partsCount, ObjectPoolCtrl pool, GameObject obj, Vector2 position, Vector2 direction, Material material)
    {
        Debug.Log("!!!!!!!!!!____________");
        for (int i = 0; i < partsCount; ++i)
        {
            var splashItem = obj.GetComponent<SpecEffectItem>();
            Debug.Log(obj + "Test4____________|||__" + splashItem); 
            if (splashItem != null)
            { 
                /*splashItem.setPoolObj(pool);  
                Material mat = material ?? DefaultMaterialProvider.SpriteDefault;
                splashItem.Initialize(mat, 0.01f);  
                splashItem.Activate(position, direction, 0.5f);   */
                yield return new WaitForSeconds(1f);   
            }
        }
    } 
} 