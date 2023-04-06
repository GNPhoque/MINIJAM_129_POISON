using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
	public static event Action OnAnyEnemyKilled;
	public abstract void TakeDamage(Material mat, float damage);
	public abstract void AddPoison();
	protected void TriggerDeadEvent()
	{
		OnAnyEnemyKilled?.Invoke();
	}
}
