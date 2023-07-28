using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject[] shards;
    [SerializeField] private float breakingForce;

    private Rigidbody rb;
    private Vector3 prevVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        prevVelocity = rb.velocity;
    }

    private void Update()
    {
        prevVelocity = rb.velocity;
    }

    public void Break()
    {
        foreach (var shard in shards)
        {
            shard.SetActive(true);
            shard.transform.parent = null;

            shard.GetComponent<Rigidbody>().velocity = prevVelocity;
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakingForce)
            Break();
    }
}
