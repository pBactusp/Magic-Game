using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHat : BaseGravitySpell
{
    [SerializeField] protected Vector3 offset;

    private Vector3 startPosition;

    protected override void OnLaunch()
    {
        base.OnLaunch();
        startPosition = transform.localPosition + offset;
        transform.localPosition = startPosition;

        StartCoroutine(SetHeightDynamically());
    }

    private IEnumerator SetHeightDynamically()
    {
        while (isAlive)
        {
            transform.localPosition = startPosition + Vector3.up * (pullAbleObjects.Length / 30f); // new Vector3(0, pullAbleObjects.Length / 40f, 0);
            yield return new WaitForSeconds(updatePullAblesDelay);
        }
    }
}
