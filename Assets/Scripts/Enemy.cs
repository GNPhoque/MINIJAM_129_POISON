using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] int attackDamage;
	[SerializeField] float attackTime;
	[SerializeField] float attackDistance;
	[SerializeField] float speed;
	[SerializeField] float hp;
	[SerializeField] TextMeshProUGUI hpText;
	[SerializeField] Color poisonedColor;
	[SerializeField] Material poisonMaterial;

	Rigidbody2D rb;
	Animator animator;
	FlashEffect flashEffect;
	SpriteRenderer spriteRenderer;
	float currentAttackTime;

	float currentPoisonDuration;
	float currentPoisonTickTime;
	bool isPoisoned;

	public static event Action OnAnyEnemyKilled;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		flashEffect = GetComponent<FlashEffect>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		hpText.text = $"{hp}";
	}

	private void Update()
	{
		if (Player.instance.isDead)
		{
			rb.velocity = Vector2.zero;
			return;
		}

		//Update Poison
		if (currentPoisonDuration > 0)
		{
			currentPoisonDuration -= Time.deltaTime;
			if (currentPoisonDuration <= 0)
			{
				RemovePoison();
			}
			else
			{
				currentPoisonTickTime -= Time.deltaTime;
				if (currentPoisonTickTime <= 0)
				{
					ApplyPoison();
				}
			}
		}

		//Attack player
		if (Vector3.Distance(Player.instance.transform.position, transform.position) < attackDistance)
		{
			currentAttackTime -= Time.deltaTime;
			if (currentAttackTime <= 0f)
			{
				Attack();
			}
		}
		else
		{
			currentAttackTime = attackTime;
		}
	}

	private void FixedUpdate()
	{
		//Move to player
		Vector2 dir = (Player.instance.transform.position - transform.position).normalized;
		rb.MovePosition(rb.position + dir * Time.deltaTime * speed);

		animator.SetFloat("HorizontalMovement", dir.x);
		animator.SetFloat("VerticalMovement", dir.y);
		animator.SetFloat("Speed", dir.magnitude);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("PlayerProjectile"))
		{
			AddPoison();
			TakeDamage(null, Player.instance.stats.shotDamage);
			Destroy(collision.gameObject);
		}
	}

	void Attack()
	{
		Player.instance.TakeDamage(attackDamage);
		currentAttackTime = attackTime;
	}

	public void TakeDamage(Material mat = null, int damage = 1)
	{
		flashEffect.Flash(mat);
		hp -= damage;
		hpText.text = $"{hp}";
		if (hp <= 0)
		{
			OnAnyEnemyKilled?.Invoke();
			Destroy(gameObject);
		}
	}

	public void AddPoison()
	{
		isPoisoned = true;
		currentPoisonDuration = Player.instance.stats.poisonDuration;
		currentPoisonTickTime = Player.instance.stats.poisonTickTime;
		spriteRenderer.color = poisonedColor;
	}

	void ApplyPoison()
	{
		TakeDamage(poisonMaterial, Player.instance.stats.poisonDamage);
		currentPoisonTickTime = Player.instance.stats.poisonTickTime;
	}

	void RemovePoison()
	{
		isPoisoned = false;
		spriteRenderer.color = Color.white;
	}
}