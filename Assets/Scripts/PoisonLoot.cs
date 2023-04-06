using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonLoot : MonoBehaviour
{
	[SerializeField] GameObject victory;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		victory.SetActive(true);
		gameObject.SetActive(false);
	}
}
