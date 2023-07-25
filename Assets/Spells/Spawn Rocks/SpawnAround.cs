using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnAround : BaseProjectile
{
    [Header("Spawned Objects Properties")]
    [SerializeField] private float spawnedObjectsFloatTime;
    [SerializeField] private float minAmount;
    [SerializeField] private float maxAmount;
    [SerializeField] private float minScaleMultiplier;
    [SerializeField] private float maxScaleMultiplier;
    [SerializeField] private float radius;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 heightOffset;

    [SerializeField] private GameObject[] prefabs;



    [Header("For Material")]
    [SerializeField] private float fadeTime;


    private Rigidbody[] rbs;
    private Collider[] cols;
    private Material material;


    protected override void OnSpawn()
    {
        base.OnSpawn();

        material = GetComponent<Renderer>().material;
        StartCoroutine(HandleShaderFade());
    }

    protected override void OnLaunch()
    {
        base.OnLaunch();

        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        transform.localScale = Vector3.one * radius * 2;
        material.SetFloat("_Scale", radius);

        var endPositions = GetRandomPositions(transform.position);
        var startPositions = new Vector3[endPositions.Length];

        rbs = new Rigidbody[endPositions.Length];
        cols = new Collider[endPositions.Length];

        for (int i = 0; i < endPositions.Length; i++)
        {
            startPositions[i] = endPositions[i] + spawnOffset;
            endPositions[i] = endPositions[i] + heightOffset;
            var obj = SpawnObject(startPositions[i]);

            rbs[i] = obj.GetComponent<Rigidbody>();
            cols[i] = obj.GetComponent<Collider>();
        }

        StartCoroutine(InitiateSpawnedObjects(startPositions, endPositions));
    }

    private IEnumerator InitiateSpawnedObjects(Vector3[] startPositions, Vector3[] endPositions)
    {
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
            rbs[i].useGravity = false;
            var torque = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
            rbs[i].AddTorque(torque, ForceMode.Impulse);
        }

        float time = 0;

        while (time < spawnedObjectsFloatTime)
        {
            float normalizedTime = time / spawnedObjectsFloatTime;

            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].position = Vector3.Lerp(startPositions[i], endPositions[i], normalizedTime);
            }

            time += Time.deltaTime;

            yield return null;
        }


        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
            rbs[i].useGravity = true;
        }
    }

    private Vector3[] GetRandomPositions(Vector3 origin)
    {
        int amount = (int)Random.Range(minAmount, maxAmount);
        var positions = new Vector3[amount];

        for (int i = 0; i < amount; i++)
        {
            float newX = Random.Range(-radius, radius);
            float newZ = Random.Range(-radius, radius);

            positions[i] = origin + new Vector3(newX, 0, newZ);
        }

        return positions;
    }

    private GameObject SpawnObject(Vector3 position)
    {
        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        var obj = Instantiate(prefab, position, transform.rotation);
        obj.transform.localScale = obj.transform.localScale * Random.Range(minScaleMultiplier, maxScaleMultiplier);
        return obj;
    }


    private IEnumerator HandleShaderFade()
    {
        float time = 0;

        while (time < fadeTime)
        {
            material.SetFloat("_Transparency_Factor", Mathf.Lerp(0f, 1f, time / fadeTime));
            time += Time.deltaTime;
            yield return null;
        }

        while (time < lifeTime - fadeTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        while (time < fadeTime)
        {
            material.SetFloat("_Transparency_Factor", Mathf.Lerp(1f, 0f, time / fadeTime));
            time += Time.deltaTime;
            yield return null;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + spawnOffset, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + heightOffset, radius);
    }
}
