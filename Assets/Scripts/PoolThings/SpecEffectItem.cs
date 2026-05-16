using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// это класс для префаба для пула
public class SpecEffectItem : PooledObject
{
    private bool isActive;    // Активное состояние
    private float disableTime = 5f;
    protected override void OnActivated(Vector2 pos, Vector2 direction, float timer)
    {
        transform.position = pos; 
        isActive = true;
        disableTime = timer; 
    }

    private void Update()
    {
        if (!isActive) return;
        
        // Вычисляем текущее время полёта
        float elapsedTime = Time.time - spawnTime;
        // Отключаем объект, если время жизни истекло
        if (elapsedTime > lifetime)
        {
            StopAction();
            return;
        }
    }
    
    private void StopAction()
    {
        isActive = false;
        StartCoroutine(DisabelItem());
    }
    
    public IEnumerator DisabelItem()
    {
        float timer = 0;

        while (timer < disableTime)
        {
            timer += 1f * Time.deltaTime;
            yield return null;
        }
        
        gameObject.SetActive(false);
    }
 
    private void ActivateSparkle(int num) 
    {
        // transform.GetChild(1).active = true;
    }
}