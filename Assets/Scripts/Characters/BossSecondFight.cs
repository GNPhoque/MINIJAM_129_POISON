using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossSecondFight : BaseEnemy
{
	[SerializeField] int attackDamage;
	[SerializeField] float spawnTime;
	[SerializeField] float attackTime;
	[SerializeField] float attackDistance;
	[SerializeField] float walkSpeed;
	[SerializeField] float rushSpeed;
	[SerializeField] float rushMaxSpeed;
	[SerializeField] float walkTimeMin;
	[SerializeField] float walkTimeMax;
	[SerializeField] float rushTime;
	[SerializeField] float restTime;
	[SerializeField] float hp;
	[SerializeField] float maxDirectionChangeAngle;
	[SerializeField] float normalRushChance;
	[SerializeField] Color poisonedColor;
	[SerializeField] Material poisonMaterial;

	Rigidbody2D rb;
	Animator animator;
	FlashEffect flashEffect;
	SpriteRenderer spriteRenderer;

	Vector2 oldMovementDirection;

	float currentHp;
	float currentPoisonDuration;
	float currentPoisonTickTime;
	bool isPoisoned;
	bool isReady;
	bool isDead;

	//PHASES
	bool isResting;
	bool isWalking;
	bool isRushing;
	bool isRushingAlt;
	float currentSpeed;
	float currentWalkTime;
	float currentRushTime;
	float currentRestingTime;

	public static event Action OnAnyBossSpawn;
	public static event Action<float, float> OnBossHealthChanged;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		flashEffect = GetComponent<FlashEffect>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine(ReadyUp());
		currentHp = hp;
		OnAnyBossSpawn?.Invoke();
	}

	private void Update()
	{
		if (!isReady || isDead || Player.instance.isDead)
		{
			animator.SetBool("IsRushing", false);
			animator.SetBool("IsResting", false);
			rb.velocity = Vector2.zero;
			return;
		}

		if (isWalking)
		{
			if (currentWalkTime <= 0) StopWalking();
			else currentWalkTime -= Time.deltaTime;
		}

		if (isRushing || isRushingAlt)
		{
			if (currentRushTime <= 0) StopRushing();
			else currentRushTime -= Time.deltaTime;
		}

		if (isResting)
		{
			if (currentRestingTime <= 0) StopResting();
			else currentRestingTime -= Time.deltaTime;
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


	}

	private void FixedUpdate()
	{
		if (!isReady || isDead || isResting || Player.instance.isDead)
		{
			rb.velocity = Vector2.zero;
			animator.SetFloat("Speed", 0);
			return;
		}

		//Move to player
		Vector2 dir = (Player.instance.transform.position - transform.position).normalized;
		if (isRushing)
		{
			dir = Vector3.RotateTowards(oldMovementDirection, dir, maxDirectionChangeAngle, 10f);
		}
		else if (isRushingAlt)
		{
			dir = oldMovementDirection;
		}
		rb.MovePosition(rb.position + dir * Time.deltaTime * currentSpeed);
		oldMovementDirection = dir;

		animator.SetFloat("HorizontalMovement", dir.x);
		animator.SetFloat("VerticalMovement", dir.y);
		animator.SetFloat("Speed", currentSpeed);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (isDead) return;
		if (collision.gameObject.CompareTag("Player"))
		{
			Player.instance.TakeDamage(attackDamage);
		}
		if (collision.gameObject.CompareTag("Walls") && isRushingAlt)
		{
			CameraShake.instance.StartShake();
			StopRushing();
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
		StartWalking();
	}

	void StartResting()
	{
		isResting = true;
		animator.SetBool("IsResting", isResting);
		currentRestingTime = restTime;
	}

	void StopResting()
	{
		isResting = false;
		animator.SetBool("IsResting", isResting);
		StartWalking();
	}

	void StartWalking()
	{
		isWalking = true;
		currentWalkTime = UnityEngine.Random.Range(walkTimeMin, walkTimeMax);
		currentSpeed = walkSpeed;
	}

	void StopWalking()
	{
		isWalking = false;
		StartRushing();
	}

	void StartRushing()
	{
		if (UnityEngine.Random.value < normalRushChance)
		{
			isRushing = true;
			currentSpeed = rushSpeed;
		}
		else
		{
			isRushingAlt = true;
			currentSpeed = rushMaxSpeed;
		}
		animator.SetBool("IsRushing", true);
		oldMovementDirection = (Player.instance.transform.position - transform.position).normalized;
		currentRushTime = rushTime;
	}

	void StopRushing()
	{
		isRushing = false;
		isRushingAlt = false;
		animator.SetBool("IsRushing", false);
		StartResting();
	}

	public override void TakeDamage(Material mat = null, float damage = 1)
	{
		if (damage > 10000) damage = Player.instance.stats.shotDamage;
		flashEffect.Flash(mat);
		currentHp -= damage;
		OnBossHealthChanged?.Invoke(currentHp, hp);
		if (currentHp <= 0)
		{
			transform.parent = null;
			isDead = true;
			spriteRenderer.color = Color.white;
			TriggerDeadEvent();
			AudioManager.instance.PlayBossDeath();
			animator.SetBool("IsDead", isDead);
			//Destroy(gameObject);
		}
	}

	#region POISON
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
	#endregion
}