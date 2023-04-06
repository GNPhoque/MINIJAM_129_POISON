using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fork : MonoBehaviour
{
	[SerializeField] float damage;
	[SerializeField] Collider2D col;
	[SerializeField] PhysicsMaterial2D bouncyMat;

	Rigidbody2D rb;
	bool hit;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, 2f);
		if (Player.instance.bossBonuses.Contains(LootBossBonus.BouncyShot))
		{
			col.sharedMaterial = bouncyMat;
		}
	}

	private void Update()
	{
		transform.right = rb.velocity;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (hit) return;
		if (collision.gameObject.CompareTag("Player"))
		{
			int damage = 1;

			Player.instance.TakeDamage(damage);
			Destroy(gameObject);
		}
	}

	public void Shoot(Vector2 dir)
	{
		AudioManager.instance.PlayWhoosh();
		transform.right = dir;
		rb.AddForce(dir * Player.instance.stats.shotVelocity, ForceMode2D.Impulse);
	}
}
