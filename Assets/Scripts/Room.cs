using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField] GameObject wallW;
	[SerializeField] GameObject wallE;

	public CinemachineVirtualCamera VirtualCamera;
	public List<GameObject> loots;
	public List<BaseEnemy> enemies;

	public static event Action OnRoomClosed;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!wallW.activeSelf && collision.gameObject.CompareTag("Player"))
		{
			CloseWallW();
			OnRoomClosed?.Invoke();
		}
	}

	public void OpenWallE()
	{
		wallE.SetActive(false);
	}

	public void CloseWallE()
	{
		wallE.SetActive(true);
	}
	public void OpenWallW()
	{
		wallW.SetActive(false);
	}
	public void CloseWallW()
	{
		wallW.SetActive(true);
	}

	public void ShowLoots()
	{
		foreach (var item in loots)
		{
			item.SetActive(true);
		}
	}
}
