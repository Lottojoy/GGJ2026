using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHitDamageEffect
{
    public IEnumerator HitDamageEffect(Color start, Color end , float elapsedTime , float duration , SpriteRenderer spriteRenderer)
    {
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            spriteRenderer.color = Color.Lerp(start, end, t);

            yield return null;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            spriteRenderer.color = Color.Lerp(end, start, t);

            yield return null;
        }
    }

    
}
