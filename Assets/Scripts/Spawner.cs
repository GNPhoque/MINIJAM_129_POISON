using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;
	[SerializeField] float spawnTime;
	[SerializeField] Transform[] spawnPoints;

	public float currentSpawnTime;

	private void Start()
	{
		currentSpawnTime = spawnTime;
	}

	private void Update()
	{
		currentSpawnTime -= Time.deltaTime;
		if (currentSpawnTime <= 0)
		{
			currentSpawnTime = spawnTime;
			Instantiate(unitPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)]);
		}
	}
}
