using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] public List<Room> rooms;
	[SerializeField] Spawner spawner;
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject bossPrefab;
	[SerializeField] Cinemachine.CinemachineVirtualCamera enterCamera;
	[SerializeField] Transform bonusParent;
	[SerializeField] Transform bonusPrefab;

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
		BaseEnemy.OnAnyEnemyKilled += Enemy_OnAnyEnemyKilled;
		Loot.OnAnyLootBonusPicked += Loot_OnAnyLootBonusPicked;
	}

	private void OnDisable()
	{
		Room.OnRoomClosed -= Room_OnRoomClosed;
		BaseEnemy.OnAnyEnemyKilled -= Enemy_OnAnyEnemyKilled;
		Loot.OnAnyLootBonusPicked -= Loot_OnAnyLootBonusPicked;
	}

	private void Start()
	{
		spawner.SetSpawns(rooms[0].enemies);
		remainingEnemies = rooms[0].enemies.Count;
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

		spawner.SetSpawns(rooms[0].enemies);
		remainingEnemies = rooms[0].enemies.Count;
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

	private void Loot_OnAnyLootBonusPicked(Sprite s)
	{
		if(s) Instantiate(bonusPrefab, bonusParent).GetComponentInChildren<Image>().sprite = s;
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
