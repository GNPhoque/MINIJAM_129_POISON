using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;
	[SerializeField] float spawnTime;
	[SerializeField] Transform[] spawnPoints;
	
	public int spawnLimit;

	float currentSpawnTime;
	int spawnedEnnemies;

	private void Start()
	{
		currentSpawnTime = spawnTime;
	}

	private void Update()
	{
		if (Player.instance.isDead)
		{
			return;
		}

		if (spawnedEnnemies >= spawnLimit) return;

		currentSpawnTime -= Time.deltaTime;
		if (currentSpawnTime <= 0)
		{
			currentSpawnTime = spawnTime;
			Instantiate(unitPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)]);
			spawnedEnnemies++;
		}
	}

	public void ResetSpawns(int spawns)
	{
		spawnLimit = spawns;
		spawnedEnnemies = 0;
		currentSpawnTime = spawnTime;
	}

	public void SetSpawnPrefab(GameObject newPrefab)
	{
		unitPrefab = newPrefab;
	}
}
