using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] Projectile projectile;
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] Transform heartsParent;
	[SerializeField] Transform ArmorsParent;

	public PlayerStats stats;
	public bool isDead;

	Rigidbody2D rb;
	Animator animator;
	FlashEffect flashEffect;
	PlayerControls inputs;
	Vector2 moveInput;

	float currentTimeBetweenShots;
	bool autoShoot;

	new Transform transform;

	public static Player instance;

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

		//MOVEMENT
		moveInput = inputs.Inputs.Move.ReadValue<Vector2>();
		animator.SetFloat("HorizontalMovement", moveInput.x);
		animator.SetFloat("VerticalMovement", moveInput.y);
		animator.SetFloat("Speed", moveInput.magnitude);

		//ATTACK
		currentTimeBetweenShots -= Time.deltaTime;
		if (currentTimeBetweenShots <= 0)
		{
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

	private void PlayerStats_OnHealthChanged()
	{
		UpdateHealthArmorDisplay();
	}

	private void Shoot_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		autoShoot = true;
	}

	private void Shoot_canceled(InputAction.CallbackContext obj)
	{
		autoShoot = false;
	}

	private void Shoot()
	{
		currentTimeBetweenShots = stats.secondsPerShot;

		Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.value) - (Vector2)transform.position).normalized;
		Vector2 cappedDirection = GetRounded4Directions(dir);
		Instantiate(projectile, (Vector2)transform.position + Vector2.up * .25f, Quaternion.identity).Shoot(cappedDirection);
	}

	Vector2 GetRounded4Directions(Vector2 baseDirection)
	{
		return new Vector2(Mathf.RoundToInt(baseDirection.x), Mathf.RoundToInt(baseDirection.y));
	}

	public void TakeDamage(int damage = 1)
	{
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
