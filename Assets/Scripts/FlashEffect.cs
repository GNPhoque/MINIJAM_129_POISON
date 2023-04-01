using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
	[SerializeField] Material flashMaterial;
	[SerializeField] float duration;

	SpriteRenderer spriteRenderer;
	Material originalMaterial;
	Coroutine flashCoroutine;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalMaterial = spriteRenderer.material;
	}

	public void Flash(Material mat = null)
	{
		if (flashCoroutine != null)
		{
			StopCoroutine(flashCoroutine);
		}

		flashCoroutine = StartCoroutine(FlashCoroutine(mat ?? flashMaterial));
	}

	IEnumerator FlashCoroutine(Material mat)
	{
		spriteRenderer.material = mat;
		yield return new WaitForSeconds(duration);

		spriteRenderer.material = originalMaterial;
		flashCoroutine = null;
	}
}