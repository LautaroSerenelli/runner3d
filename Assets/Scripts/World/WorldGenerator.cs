using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public List<GameObject> blockPrefabs;
    public List<GameObject> obstaclePrefabs;
    public List<GameObject> powerUpPrefabs;

    private Transform lastEndPoint;
    public Transform player;

    public int initialBlockCount = 5;
    public float spawnDistanceAhead = 30f;
    public float destroyDistanceBehind = 20f;

    public float powerUpSpawnChance = 0.1f;

    public float worldSpeed = 5f;
    public float maxWorldSpeed = 15f;
    public float accelerationRate = 0.1f;
    private float speedModifier = 1f;
    private float modifierEndTime = 0f;

    private List<GameObject> activeBlocks = new List<GameObject>();

    void Start()
    {
        GameObject firstBlock = Instantiate(blockPrefabs[0], Vector3.zero, Quaternion.identity);
        lastEndPoint = firstBlock.transform.Find("EndPoint");
        activeBlocks.Add(firstBlock);

        for (int i = 0; i < initialBlockCount - 1; i++)
        {
            SpawnNextBlock();
        }
    }

    void Update()
    {
        MoveBlocks();
        IncreaseWorldSpeed();
        UpdateSpeedModifier();

        float distanceToEndPoint = Vector3.Distance(player.position, lastEndPoint.position);

        if (distanceToEndPoint < spawnDistanceAhead)
        {
            SpawnNextBlock();
        }

        CleanupOldBlocks();
    }

    void SpawnNextBlock()
    {
        GameObject blockPrefab = blockPrefabs[Random.Range(1, blockPrefabs.Count)];
        GameObject newBlock = Instantiate(blockPrefab);

        Transform newStart = newBlock.transform.Find("StartPoint");
        Vector3 offset = lastEndPoint.position - newStart.position;
        newBlock.transform.position += offset;
        lastEndPoint = newBlock.transform.Find("EndPoint");
        activeBlocks.Add(newBlock);

        List<Transform> apparitionPoints = new List<Transform>();
        foreach (Transform child in newBlock.GetComponentsInChildren<Transform>())
        {
            if (child.name.StartsWith("ApparitionPoint"))
            {
                apparitionPoints.Add(child);
            }
        }

        foreach (Transform point in apparitionPoints)
        {
            if (Random.value < 0.5f)
            {
                GameObject obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
                GameObject newObstacle = Instantiate(obstacle, point.position, Quaternion.identity, newBlock.transform);
                newObstacle.transform.rotation = obstacle.transform.rotation;

                Vector3 newPosition = newObstacle.transform.position;
                newPosition.y = point.position.y + obstacle.transform.localPosition.y;
                newObstacle.transform.position = newPosition;
            }
        }

        List<Transform> powerUpPoints = new List<Transform>();
        foreach (Transform child in newBlock.GetComponentsInChildren<Transform>())
        {
            if (child.name.StartsWith("PowerUpPoint"))
            {
                powerUpPoints.Add(child);
            }
        }

        foreach (Transform point in powerUpPoints)
        {
            if (Random.value < powerUpSpawnChance)
            {
                GameObject powerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)];
                GameObject newPowerUp = Instantiate(powerUp, point.position, Quaternion.identity, newBlock.transform);
                
                Vector3 newPosition = newPowerUp.transform.position;
                newPosition.y = point.position.y + powerUp.transform.localPosition.y;
                newPowerUp.transform.position = newPosition;
            }
        }
    }

    void CleanupOldBlocks()
    {
        for (int i = activeBlocks.Count - 1; i >= 0; i--)
        {
            GameObject block = activeBlocks[i];
            if (block == null) continue;

            Transform endPoint = block.transform.Find("EndPoint");
            if (endPoint == null) continue;

            float distanceBehind = player.position.z - endPoint.position.z;

            if (distanceBehind > destroyDistanceBehind)
            {
                Destroy(block);
                activeBlocks.RemoveAt(i);
            }
        }
    }

    void MoveBlocks()
    {
        foreach (GameObject block in activeBlocks)
        {
            if (block != null)
            {
                block.transform.Translate(Vector3.back * worldSpeed * speedModifier * Time.deltaTime);
            }
        }
    }

    void IncreaseWorldSpeed()
    {
        if (worldSpeed < maxWorldSpeed)
        {
            worldSpeed += accelerationRate * Time.deltaTime;
            if (worldSpeed > maxWorldSpeed)
                worldSpeed = maxWorldSpeed;
        }
    }

    public void ApplySpeedModifier(float modifier, float duration)
    {
        speedModifier = modifier;
        modifierEndTime = Time.time + duration;
    }

    private void UpdateSpeedModifier()
    {
        if (Time.time > modifierEndTime)
        {
            speedModifier = 1f;
        }
    }
}