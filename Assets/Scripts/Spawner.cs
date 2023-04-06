using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] float spawnTime;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] List<BaseEnemy> enemies;
	
	float currentSpawnTime;

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

		if (enemies.Count == 0) return;

		currentSpawnTime -= Time.deltaTime;
		if (currentSpawnTime <= 0)
		{
			currentSpawnTime = spawnTime;
			Instantiate(enemies[0], spawnPoints[Random.Range(0, spawnPoints.Length)]);
			enemies.RemoveAt(0);
		}
	}

	public void SetSpawns(List<BaseEnemy> _enemies)
	{
		enemies = _enemies;
	}
}
