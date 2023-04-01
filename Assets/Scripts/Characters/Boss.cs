using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
	[SerializeField] int attackDamage;
	[SerializeField] float spawnTime;
	[SerializeField] float attackTime;
	[SerializeField] float attackDistance;
	[SerializeField] float walkSpeed;
	[SerializeField] float rushSpeed;
	[SerializeField] float walkTimeMin;
	[SerializeField] float walkTimeMax;
	[SerializeField] float rushTime;
	[SerializeField] float restTime;
	[SerializeField] float hp;
	[SerializeField] float maxDirectionChangeAngle;
	[SerializeField] TextMeshProUGUI hpText;
	[SerializeField] Color poisonedColor;
	[SerializeField] Material poisonMaterial;

	Rigidbody2D rb;
	Animator animator;
	FlashEffect flashEffect;
	SpriteRenderer spriteRenderer;

	Vector2 oldMovementDirection;

	float currentPoisonDuration;
	float currentPoisonTickTime;
	bool isPoisoned;
	bool isReady;

	//PHASES
	bool isResting;
	bool isWalking;
	bool isRushing;
	float currentSpeed;
	float currentWalkTime;
	float currentRushTime;
	float currentRestingTime;

	public static event Action OnAnyEnemyKilled;

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
		print(currentSpeed);
		if (!isReady) return;

		if (Player.instance.isDead)
		{
			rb.velocity = Vector2.zero;
			return;
		}

		if (isWalking)
		{
			if (currentWalkTime <= 0) StopWalking();
			else currentWalkTime -= Time.deltaTime;
		}

		if(isRushing)
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
		if (!isReady || isResting || Player.instance.isDead)
		{
			rb.velocity = Vector2.zero;
			return;
		}

		//Move to player
		Vector2 dir = (Player.instance.transform.position - transform.position).normalized;
		if (isRushing)
		{
			dir = Vector3.RotateTowards(oldMovementDirection, dir, maxDirectionChangeAngle, 10f);
		}
		rb.MovePosition(rb.position + dir * Time.deltaTime * currentSpeed);
		oldMovementDirection = dir;

		animator.SetFloat("HorizontalMovement", dir.x);
		animator.SetFloat("VerticalMovement", dir.y);
		animator.SetFloat("Speed", dir.magnitude);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player.instance.TakeDamage(attackDamage);
		}

		if (collision.gameObject.CompareTag("PlayerProjectile"))
		{
			AddPoison();
			TakeDamage(null, Player.instance.stats.shotDamage);
			Destroy(collision.gameObject);
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
		currentRestingTime = restTime;
	}

	void StopResting()
	{
		isResting = false;
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
		isRushing = true;
		oldMovementDirection = (Player.instance.transform.position - transform.position).normalized;
		currentSpeed = rushSpeed;
		currentRushTime = rushTime;
	}

	void StopRushing()
	{
		isRushing = false;
		StartResting();
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

	#region POISON
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
	#endregion
}