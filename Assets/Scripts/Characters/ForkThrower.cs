using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForkThrower : BaseEnemy
{
	[SerializeField] int attackDamage;
	[SerializeField] float spawnTime;
	[SerializeField] float attackTime;
	[SerializeField] float attackDistance;
	[SerializeField] float rangedAttackDistance;
	[SerializeField] float speed;
	[SerializeField] float hp;
	[SerializeField] TextMeshProUGUI hpText;
	[SerializeField] Color poisonedColor;
	[SerializeField] Material poisonMaterial;
	[SerializeField] Fork projectile;

	Rigidbody2D rb;
	Animator animator;
	FlashEffect flashEffect;
	SpriteRenderer spriteRenderer;
	float currentAttackTime;

	float currentPoisonDuration;
	float currentPoisonTickTime;
	bool isPoisoned;
	bool isReady;
	bool forkThrown;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		flashEffect = GetComponent<FlashEffect>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		hpText.text = $"{hp}";
		StartCoroutine(ReadyUp());
	}

	private void Update()
	{
		if (!isReady || Player.instance.isDead)
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
		float playerDistance = Vector3.Distance(Player.instance.transform.position, transform.position);
		if (Vector3.Distance(Player.instance.transform.position, transform.position) <= attackDistance)
		{
			currentAttackTime -= Time.deltaTime;
			if (currentAttackTime <= 0f)
			{
				Attack();
			}
		}
		else if (!forkThrown)
		{
			if (playerDistance < rangedAttackDistance)
			{
				currentAttackTime -= Time.deltaTime;
				if (currentAttackTime <= 0f)
				{
					RangedAttack();
				}
			}
		}
		else
		{
			currentAttackTime = attackTime;
		}
	}

	private void FixedUpdate()
	{
		if (!isReady || Player.instance.isDead) return;

		//Move to player
		float playerDistance = Vector3.Distance(Player.instance.transform.position, transform.position);
		if (forkThrown || playerDistance > rangedAttackDistance)
		{
			Vector2 dir = (Player.instance.transform.position - transform.position).normalized;
			rb.MovePosition(rb.position + dir * Time.deltaTime * speed);

			animator.SetFloat("HorizontalMovement", dir.x);
			animator.SetFloat("Speed", dir.magnitude);
		}
		else
		{
			animator.SetFloat("HorizontalMovement", 0);
			animator.SetFloat("Speed", 0);
		}
	}

	IEnumerator ReadyUp()
	{
		while (spriteRenderer.color.a < 1)
		{
			Color color = spriteRenderer.color;
			color.a += Time.deltaTime / spawnTime;
			spriteRenderer.color = color;
			yield return null;
		}
		isReady = true;
	}

	void RangedAttack()
	{
		currentAttackTime = attackTime;
		forkThrown = true;
		animator.SetBool("ForkShot", true);

		Vector2 dir = (((Vector2)Player.instance.transform.position) + Vector2.up * .6f - ((Vector2)transform.position + Vector2.up * .6f)).normalized;
		Instantiate(projectile, (Vector2)transform.position + Vector2.up * .6f, Quaternion.identity).Shoot(dir);
	}

	void Attack()
	{
		Player.instance.TakeDamage(attackDamage);
		currentAttackTime = attackTime;
	}

	public override void TakeDamage(Material mat = null, float damage = 1)
	{
		flashEffect.Flash(mat);
		hp -= damage;
		hpText.text = $"{hp}";
		if (hp <= 0)
		{
			TriggerDeadEvent();
			AudioManager.instance.PlayEnemyDeath();
			Destroy(gameObject);
		}
	}

	public override void AddPoison()
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