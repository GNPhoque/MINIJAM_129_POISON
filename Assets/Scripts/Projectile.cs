using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] float damage;

	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, 2f);
	}

	public void Shoot(Vector2 dir)
	{
		transform.right = dir;
		rb.AddForce(dir * speed, ForceMode2D.Impulse);
	}
}
