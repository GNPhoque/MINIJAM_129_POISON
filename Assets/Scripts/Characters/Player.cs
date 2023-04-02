using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] Projectile projectilePrefab;
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] Transform heartsParent;
	[SerializeField] Transform ArmorsParent;
	[SerializeField] float invincibilityTime;
	[SerializeField] float timeBetweenChargedShots;
	[SerializeField] int maxChargedShots;

	public PlayerStats stats;
	public List<LootBossBonus> bossBonuses;
	public bool isDead;
	
	Rigidbody2D rb;
	Animator animator;
	FlashEffect flashEffect;
	PlayerControls inputs;
	Vector2 moveInput;

	float currentTimeBetweenShots;
	float currentInvincibilityTime;
	float currentShotChargeTime;
	bool autoShoot;
	bool isChargingShot;

	new Transform transform;

	public static Player instance;

	#region MONOBEHAVIOUR
	private void Awake()
	{
		if (instance) Destroy(instance.gameObject);
		instance = this;

		inputs = new PlayerControls();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		transform = GetComponent<Transform>();
		flashEffect = GetComponent<FlashEffect>();
		UpdateHealthArmorDisplay();
	}

	private void OnEnable()
	{
		PlayerStats.OnHealthChanged += PlayerStats_OnHealthChanged;
		inputs.Inputs.Shoot.started += Shoot_started;
		inputs.Inputs.Shoot.canceled += Shoot_canceled;
		inputs.Enable();
	}

	private void OnDisable()
	{
		PlayerStats.OnHealthChanged -= PlayerStats_OnHealthChanged;
		inputs.Inputs.Shoot.started += Shoot_started;
		inputs.Inputs.Shoot.canceled += Shoot_canceled;
		inputs.Enable();
	}

	private void Update()
	{
		if (isDead)
		{
			return;
		}

		if (currentInvincibilityTime > 0)
		{
			currentInvincibilityTime -= Time.deltaTime;
		}

		//MOVEMENT
		moveInput = inputs.Inputs.Move.ReadValue<Vector2>();
		animator.SetFloat("HorizontalMovement", moveInput.x);
		animator.SetFloat("VerticalMovement", moveInput.y);
		animator.SetFloat("Speed", moveInput.magnitude);

		//ATTACK
		currentTimeBetweenShots -= Time.deltaTime;
		if (currentTimeBetweenShots <= 0)
		{
			if (isChargingShot)
			{
				currentShotChargeTime += Time.deltaTime;
			}
			if (autoShoot)
			{
				Shoot();
			}
		}
	}

	private void FixedUpdate()
	{
		if (isDead)
		{
			return;
		}

		rb.MovePosition(rb.position + moveInput.normalized * Time.deltaTime * stats.moveSpeed);
	} 
	#endregion

	#region EVENTS
	private void PlayerStats_OnHealthChanged()
	{
		UpdateHealthArmorDisplay();
	}

	private void Shoot_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		if (bossBonuses.Contains(LootBossBonus.ChargedShot))
		{
			currentShotChargeTime = 0f;
			isChargingShot = true;
		}
		else autoShoot = true; 
	}

	private void Shoot_canceled(InputAction.CallbackContext obj)
	{
		if (bossBonuses.Contains(LootBossBonus.ChargedShot))
		{
			isChargingShot = false;
			StartCoroutine(ShootCharged());
		}
		else autoShoot = false;
	} 
	#endregion

	private void Shoot()
	{
		currentTimeBetweenShots = stats.secondsPerShot;

		Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.value) - ((Vector2)transform.position + Vector2.up * .6f)).normalized;
		if(!bossBonuses.Contains(LootBossBonus.PreciseShot)) dir = GetRounded4Directions(dir);

		Instantiate(projectilePrefab, (Vector2)transform.position + Vector2.up * .6f, Quaternion.identity).Shoot(dir);

	}

	IEnumerator ShootCharged()
	{
		if (currentShotChargeTime >= stats.secondsPerShot)
		{
			currentTimeBetweenShots = 20; //Cant start charging before all shots ended
										  //int shots = Mathf.FloorToInt(Mathf.Clamp(currentShotChargeTime / stats.secondsPerShot, 0, maxChargedShots));
										  //for (int i = 0; i < shots; i++)
			for (int i = 0; i < maxChargedShots; i++)
			{
				Shoot();
				yield return new WaitForSeconds(timeBetweenChargedShots);
			}
			currentTimeBetweenShots = 0;
		}
	}

	Vector2 GetRounded4Directions(Vector2 baseDirection)
	{
		float absX = Mathf.Abs(baseDirection.x);
		float absY = Mathf.Abs(baseDirection.y);

		if (absX > absY)
		{
			return baseDirection.x > 0 ? Vector2.right : Vector2.left;
		}
		else if (absX < absY)
		{
			return baseDirection.y > 0 ? Vector2.up : Vector2.down;
		}
		return Vector2.zero;
	}

	public void TakeDamage(int damage = 1)
	{
		if (currentInvincibilityTime > 0) return;

		currentInvincibilityTime = invincibilityTime;
		flashEffect.Flash();
		while (damage > 0)
		{
			if (stats.armor > 0)
			{
				stats.armor--;
				damage--;
				continue;
			}
			stats.hp -= damage;
			break;
		}

		UpdateHealthArmorDisplay();

		if (stats.hp <= 0)
		{
			isDead = true;
			gameOverPanel.SetActive(true);
		}
	}

	public void UpdateHealthArmorDisplay()
	{
		int emptyHearts = stats.maxHp;
		int hearts = stats.hp;
		int emptyArmors = stats.maxArmor;
		int armors = stats.armor;

		for (int i = 0; i < stats.MAX_HEARTS_POSSIBLE; i++)
		{
			Transform heart = heartsParent.GetChild(i);
			heart.gameObject.SetActive(i < emptyHearts);
			heart.GetChild(0).gameObject.SetActive(i < hearts);
		}

		for (int i = 0; i < stats.MAX_ARMORS_POSSIBLE; i++)
		{
			Transform heart = ArmorsParent.GetChild(i);
			heart.gameObject.SetActive(i < emptyArmors);
			heart.GetChild(0).gameObject.SetActive(i < armors);
		}
	}
}
