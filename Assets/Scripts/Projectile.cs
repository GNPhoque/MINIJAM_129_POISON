using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
		if (collision.gameObject.CompareTag("Enemy"))
		{
			int damage = Player.instance.stats.shotDamage;
			if (Player.instance.bossBonuses.Contains(LootBossBonus.FatalShot))
			{
				if (Random.value <= .05) damage = 100000;
			}

			BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
			enemy.TakeDamage(null, damage);
			enemy.AddPoison();
			if (!Player.instance.bossBonuses.Contains(LootBossBonus.BouncyShot))
			{
				hit = true;
				Destroy(gameObject);
			}
		}

	}
	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//	if (hit) return;
	//	if (collision.gameObject.CompareTag("Enemy"))
	//	{
	//		int damage = Player.instance.stats.shotDamage;
	//		if (Player.instance.bossBonuses.Contains(LootBossBonus.FatalShot))
	//		{
	//			if (Random.value <= .05) damage = 100000;
	//		}

	//		BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
	//		enemy.TakeDamage(null, damage);
	//		enemy.AddPoison();
	//		if (!Player.instance.bossBonuses.Contains(LootBossBonus.BouncyShot))
	//		{
	//			hit = true; 
	//			Destroy(gameObject); 
	//		}
	//	}
	//}

	public void Shoot(Vector2 dir, bool applyMultiShot = true)
	{
		AudioManager.instance.PlayWhoosh();
		transform.right = dir;
		rb.AddForce(dir * Player.instance.stats.shotVelocity, ForceMode2D.Impulse);

		if (applyMultiShot)
		{

		}
	}
}
