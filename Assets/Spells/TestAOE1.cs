using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAOE1 : BaseAreaOfEffect
{
    [SerializeField] float startScale;
    [SerializeField] float targetScale;
    [SerializeField] AnimationCurve expansionRate;

    protected override void Behavior()
    {
        StartCoroutine(Expand());
    }


    private IEnumerator Expand()
    {
        float time = 0;

        while (time < lifeTime)
        {
            float newScale = Mathf.Lerp(startScale, targetScale, expansionRate.Evaluate(time / lifeTime));

            transform.localScale = Vector3.one * newScale;

            time += Time.deltaTime;
            yield return null;
        }

        Die();
    }
}
