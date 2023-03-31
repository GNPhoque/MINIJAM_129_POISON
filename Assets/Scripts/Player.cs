using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] public float hp;
	[SerializeField] public float timeBetweenShots;
	[SerializeField] Projectile projectile;
	[SerializeField] TextMeshProUGUI hpText;

	PlayerControls inputs;
	Vector2 moveInput;
	float currentTimeBetweenShots;
	bool autoShoot;
	bool inputingMovement;

	new Transform transform;
	public static Player instance;

	private void Awake()
	{
		if (instance) Destroy(instance.gameObject);
		instance = this;

		inputs = new PlayerControls();
		transform = GetComponent<Transform>();
		hpText.text = $"Player : {hp}";
	}

	private void OnEnable()
	{
		inputs.Inputs.Move.performed += Move_performed;
		inputs.Inputs.Move.canceled += Move_canceled;
		inputs.Inputs.Shoot.started += Shoot_started;
		inputs.Inputs.Shoot.canceled += Shoot_canceled;
		inputs.Enable();
	}

	private void OnDisable()
	{
		inputs.Inputs.Move.performed += Move_performed;
		inputs.Inputs.Move.canceled += Move_canceled;
		inputs.Inputs.Shoot.started += Shoot_started;
		inputs.Inputs.Shoot.canceled += Shoot_canceled;
		inputs.Enable();
	}

	private void Update()
	{
		currentTimeBetweenShots -= Time.deltaTime;
		if(currentTimeBetweenShots <= 0)
		{
			if (autoShoot)
			{
				Shoot();
			}
		}
		if (inputingMovement)
		{
			moveInput = inputs.Inputs.Move.ReadValue<Vector2>();
			transform.position += new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime;
		}
	}

	private void Shoot_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		autoShoot = true;
	}

	private void Shoot_canceled(InputAction.CallbackContext obj)
	{
		autoShoot = false;
	}

	private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		inputingMovement = false;
	}

	private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		inputingMovement = true;
	}

	private void Shoot()
	{
		currentTimeBetweenShots = timeBetweenShots;

		Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.value) - (Vector2)transform.position).normalized;
		Vector2 cappedDirection = GetRounded4Directions(dir);
		Instantiate(projectile, (Vector2)transform.position + Vector2.up * .25f, Quaternion.identity).Shoot(cappedDirection);
	}

	Vector2 GetRounded4Directions(Vector2 baseDirection)
	{
		return new Vector2(Mathf.RoundToInt(baseDirection.x), Mathf.RoundToInt(baseDirection.y));
	}

	public void TakeDamage(float damage = 1)
	{
		hp -= damage;
		hpText.text = $"Player : {hp}";
	}
}
