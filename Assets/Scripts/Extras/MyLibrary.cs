using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public static class MyLibrary
{
    public enum CharacterTypes
    {
        Player,
        AI
    }
    
    public static bool CheckLayer(int layer, LayerMask objectMask)
    {
        return ((1 << layer) & objectMask) != 0;
    }

    public static IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float duration, float desiredAlpha, UnityAction onComplete = null)
    {
        float currentAlpha = canvasGroup.alpha;
        float timer = 0;
               
        while (timer < 1f)
        {
            float targetAlpha = Mathf.SmoothStep(currentAlpha, desiredAlpha, timer);
            canvasGroup.alpha = targetAlpha; 

            timer += duration * Time.deltaTime;
            yield return null;
        } 

        canvasGroup.alpha = desiredAlpha;
        onComplete?.Invoke();   
    }

    public enum WeaponTypes
    {
        Bottle,
        Scull,
    }

    public enum EventTypes
    {
        EnemySpawn,
        ItemSpawn, 
    }
    
    public static bool IsIndexValid<T>(this List<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }
    
    // Метод для получения длины анимации
    public static float GetAnimationLength(Animator animator, string name)
    {
        if (animator.runtimeAnimatorController == null) return 1f;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name.Contains(name)) // Название вашего анимационного клипа
            {
                return clip.length;
            }
        }
        return 1f; // Значение по умолчанию
    }
    
    public static IEnumerator ReloadProgress(Image bar, float lerpDuration, float startValue, float endValue ) {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            bar.fillAmount = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        bar.fillAmount = endValue;  
    }
    
    public enum FlipMode
    {
        MovementDirection,
        WeaponDirection
    }
    
    public static bool HasReachedTrigger2D(Vector3 npcPosition, Vector3 triggerPosition, 
        float horizontalThreshold = 0.3f, float verticalThreshold = 0.2f)
    {
        float horizontalDistance = Mathf.Abs(npcPosition.x - triggerPosition.x); 
        float verticalDistance = Mathf.Abs(npcPosition.y - triggerPosition.y);
        return horizontalDistance <= horizontalThreshold;  
    }
    
    public static void SetAlphaValue(SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = Mathf.Clamp01(alpha); // Ограничение 0-1
        renderer.color = color;
    }
}