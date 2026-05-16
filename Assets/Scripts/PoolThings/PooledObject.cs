using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    protected Material material; // object material ??? 
    protected float lifetime = 3f; // time for object existing

    protected float spawnTime; // ???? 

    private ObjectPoolCtrl pool;

    public void setPoolObj(ObjectPoolCtrl p)
    {
        pool = p;
    }

    public virtual void Initialize(Material mat, float lifetime)
    {
        this.material = mat;
        this.lifetime = lifetime;
        GetComponent<SpriteRenderer>().material = mat;
    }

    public virtual void Activate(Vector2 position, Vector2 direction, float timer)
    {
        transform.position = position; 
        spawnTime = Time.time + lifetime; // time of creation to count existing time
        OnActivated(position, direction, timer); // behaviour on activation  
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        if (pool != null) pool.ReturnObject(gameObject);
    }

    protected abstract void OnActivated(Vector2 position, Vector2 direction, float timer);

    protected virtual void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            Deactivate();
        }
    }
}