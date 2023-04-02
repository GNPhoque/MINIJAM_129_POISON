using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
	public abstract void TakeDamage(Material mat, int damage);
	public abstract void AddPoison();
}
