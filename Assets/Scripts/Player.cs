using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] float speed;

	PlayerControls inputs;
	Vector2 moveInput;
	bool inputingMovement;

	private void Awake()
	{
		inputs = new PlayerControls();
	}

	private void OnEnable()
	{
		inputs.Inputs.Move.performed += Move_performed;
		inputs.Inputs.Move.canceled += Move_canceled;
		inputs.Enable();
	}

	private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		inputingMovement = false;
	}

	private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		inputingMovement = true;
	}

	private void Update()
	{
		if (inputingMovement)
		{
			moveInput = inputs.Inputs.Move.ReadValue<Vector2>();
			transform.position += new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime;
		}
	}
}
