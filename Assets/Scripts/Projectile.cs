using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] float damage;

	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, 2f);
	}

	public void Shoot(Vector2 dir)
	{
		AudioManager.instance.PlayWhoosh();
		transform.right = dir;
		rb.AddForce(dir * Player.instance.stats.shotVelocity, ForceMode2D.Impulse);
	}
}
