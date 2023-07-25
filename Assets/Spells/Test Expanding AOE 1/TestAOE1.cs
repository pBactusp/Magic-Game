using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAOE1 : BaseAreaOfEffect
{
    [SerializeField] float startScale;
    [SerializeField] float targetScale;
    [SerializeField] AnimationCurve expansionRate;

    private Coroutine expand;

    protected override void OnSpawn()
    {
        base.OnSpawn();
        transform.localScale = Vector3.one * startScale;
    }

    protected override void OnLaunch()
    {
        base.OnLaunch();
        expand = StartCoroutine(Expand());
    }


    private IEnumerator Expand()
    {
        while (true)
        {
            float newScale = Mathf.Lerp(startScale, targetScale, expansionRate.Evaluate(normalizedLifetime));

            transform.localScale = Vector3.one * newScale;

            yield return null;
        }

        //Die();
    }

    public override void Die()
    {
        StopCoroutine(expand);
        base.Die();
    }

    
}
