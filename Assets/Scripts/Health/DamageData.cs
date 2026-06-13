using UnityEngine;

public struct DamageData
{
    public int Damage;
    public Vector2 HitPoint;
    public Vector2 HitDirection;
    public GameObject Source;

    public DamageData(
        int damage,
        Vector2 hitPoint,
        Vector2 hitDirection,
        GameObject source)
    {
        Damage = damage;
        HitPoint = hitPoint;
        HitDirection = hitDirection;
        Source = source;
    }
}  