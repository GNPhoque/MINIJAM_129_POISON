using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] float damageTime;
	[SerializeField] float speed;
	[SerializeField] float hp;
	[SerializeField] TextMeshProUGUI hpText;

	float currentDamageTime;

	private void Start()
	{
		hpText.text = $"Enemy : {hp}";
	}

	private void Update()
	{
		transform.position += (Player.instance.transform.position - transform.position).normalized * Time.deltaTime * speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			currentDamageTime = damageTime;
		}

		if (collision.gameObject.CompareTag("PlayerProjectile"))
		{
			TakeDamage();
			Destroy(collision.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		currentDamageTime -= Time.deltaTime;
		if(currentDamageTime <= 0)
		{
			currentDamageTime = damageTime;
			Player.instance.TakeDamage();
		}
	}

	public void TakeDamage(float damage = 1)
	{
		hp -= damage;
		hpText.text = $"Enemy : {hp}";
		if (hp <= 0)
		{
			Destroy(gameObject);
		}
	}
}
