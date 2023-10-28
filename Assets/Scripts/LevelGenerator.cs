using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] float maxYDistance = 8;
    [SerializeField] float minYDistance = 4;
    [SerializeField] float maxXDistance = 8;
    [SerializeField] float minXDistance = -8;
    [SerializeField] int nrOfPlatforms = 20;
    private void GenerateLevel()
    {
        float currentY = 0;
        for (int i = 0; i < nrOfPlatforms; i++)
        {
            currentY += Random.Range(minYDistance, maxYDistance);
            float currentX;
            if (i % 2 == 0) // daca i este par 
            {
                currentX = -Random.Range(minXDistance, maxXDistance);
            }
            else // daca i este impar 
            {
                currentX = Random.Range(minXDistance, maxXDistance);
            }
            var platform = Instantiate(platformPrefab, new Vector3(currentX, currentY, 0), Quaternion.identity);
            // score of the platform 
            platform.GetComponent<Platform>().score = i + 1;
        }
    }

    private void Start()
    {
        GenerateLevel();
    }
}
