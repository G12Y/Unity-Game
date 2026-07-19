using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public List<Transform> queueSlots;
    public int totalCustomersInLevel = 20;

    [Header("Spawn Timing")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;

    private int customersSpawned = 0;
    private List<GameObject> customersInLine = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (customersSpawned < totalCustomersInLevel)
        {
            if (customersInLine.Count < queueSlots.Count)
            {
                float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
                yield return new WaitForSeconds(randomDelay);

                if (customersInLine.Count < queueSlots.Count)
                {
                    SpawnNewCustomer();
                }
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void SpawnNewCustomer()
    {
        if (customersSpawned >= totalCustomersInLevel)
            return;

        customersSpawned++;

        GameObject newCustomer = Instantiate(
            customerPrefab,
            spawnPoint.position,
            Quaternion.identity);

        customersInLine.Add(newCustomer);

        UpdateQueuePositions();
    }

    public void ServeCustomer()
    {
        if (customersInLine.Count == 0)
            return;

        GameObject firstCustomer = customersInLine[0];

        customersInLine.RemoveAt(0);

        Destroy(firstCustomer);

        UpdateQueuePositions();
    }

    void UpdateQueuePositions()
    {
        for (int i = 0; i < customersInLine.Count; i++)
        {
            Customer customer = customersInLine[i].GetComponent<Customer>();

            if (customer != null)
            {
                customer.targetPosition = queueSlots[i].position;
            }
        }
    }

    public GameObject GetFirstCustomer()
    {
        if (customersInLine.Count == 0)
            return null;

        return customersInLine[0];
    }

    public int CustomerCount()
    {
        return customersInLine.Count;
    }
}