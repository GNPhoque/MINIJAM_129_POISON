using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] List<Room> rooms;
	[SerializeField] Spawner spawner;
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject bossPrefab;
	[SerializeField] Cinemachine.CinemachineVirtualCamera enterCamera;

	public List<GameObject> currentLoots;
	
	int remainingEnemies;

	public static GameManager instance;

	#region MONOBEHAVIOUR
	private void Awake()
	{
		if (instance) Destroy(instance.gameObject);
		instance = this;
	}

	private void OnEnable()
	{
		Room.OnRoomClosed += Room_OnRoomClosed;
		Enemy.OnAnyEnemyKilled += Enemy_OnAnyEnemyKilled;
		Boss.OnAnyEnemyKilled += Enemy_OnAnyEnemyKilled;
		Loot.OnAnyLootBonusPicked += Loot_OnAnyLootBonusPicked;
	}

	private void OnDisable()
	{
		Room.OnRoomClosed -= Room_OnRoomClosed;
		Enemy.OnAnyEnemyKilled -= Enemy_OnAnyEnemyKilled;
		Boss.OnAnyEnemyKilled -= Enemy_OnAnyEnemyKilled;
		Loot.OnAnyLootBonusPicked -= Loot_OnAnyLootBonusPicked;
	}

	private void Start()
	{
		remainingEnemies = rooms[0].totalEnemies;
		spawner.spawnLimit = remainingEnemies;
		currentLoots = rooms[0].loots;
		enterCamera.Priority = 0;
	}
	#endregion

	#region EVENTS
	private void Room_OnRoomClosed()
	{
		//Camera.main.transform.position += Vector3.right * 18f;
		rooms[0].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
		spawner.transform.position += Vector3.right * 18f;
		rooms[0].CloseWallE();

		Destroy(rooms[0]);
		rooms.RemoveAt(0);

		//IF BOSS ROOM
		if(rooms.Count == 1)
		{
			spawner.SetSpawnPrefab(bossPrefab);
		}
		else
		{
			spawner.SetSpawnPrefab(enemyPrefab);
		}

		spawner.ResetSpawns(rooms[0].totalEnemies);
		remainingEnemies = rooms[0].totalEnemies;
		spawner.spawnLimit = remainingEnemies;
		currentLoots = rooms[0].loots;

		Player.instance.stats.armor++;
	}

	private void Enemy_OnAnyEnemyKilled()
	{
		remainingEnemies--;
		if(remainingEnemies <= 0)
		{
			rooms[0].ShowLoots();
			if (rooms.Count > 1)
			{
				rooms[0].OpenWallE();
				rooms[1].OpenWallW();
			}
			else
			{
				//next act
			}
		}
	}

	private void Loot_OnAnyLootBonusPicked()
	{
		print("destroy loots");
		foreach (var item in currentLoots)
		{
			Destroy(item.gameObject);
		}
	}
	#endregion

	public void Replay()
	{
		SceneManager.LoadScene(0);
	}
}
