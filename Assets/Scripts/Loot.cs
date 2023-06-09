using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
	public LootBonusStat bonusStat;
	public LootBossBonus bossBonus;
	public float value;
	public bool isBossLoot;

	public static event Action<Sprite> OnAnyLootBonusPicked;
	public static event Action OnAnyStatLootBonusPicked;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("PlayerProjectile"))
		{
			if (bonusStat != LootBonusStat.None)
			{
				switch (bonusStat)
				{
					case LootBonusStat.poisonDamage:
						Player.instance.stats.poisonDamage += (int)value;
						break;
					case LootBonusStat.poisonDuration:
						Player.instance.stats.poisonDuration += (int)value;
						break;
					case LootBonusStat.poisonTickTime:
						Player.instance.stats.poisonTickTime -= value;
						break;
					case LootBonusStat.shotDamage:
						Player.instance.stats.shotDamage += value;
						break;
					case LootBonusStat.secondsPerShot:
						Player.instance.stats.secondsPerShot -= value;
						break;
					case LootBonusStat.shotVelocity:
						Player.instance.stats.shotVelocity += (int)value;
						break;
					case LootBonusStat.moveSpeed:
						Player.instance.stats.moveSpeed += value;
						break;
					case LootBonusStat.maxHp:
						Player.instance.stats.maxHp += (int)value;
						Player.instance.stats.hp += (int)value;
						break;
					case LootBonusStat.maxArmor:
						Player.instance.stats.maxArmor += (int)value;
						Player.instance.stats.armor += (int)value;
						break;
					case LootBonusStat.hp:
						Player.instance.stats.hp += (int)value;
						break;
					case LootBonusStat.armor:
						Player.instance.stats.armor += (int)value;
						break;
					default:
						break;
				}
				OnAnyStatLootBonusPicked?.Invoke();
			}


			if (bossBonus != LootBossBonus.None)
			{
				Player.instance.bossBonuses.Add(bossBonus);
			}

			OnAnyLootBonusPicked?.Invoke(isBossLoot ?transform.GetChild(1).GetComponent<SpriteRenderer>().sprite : null);
		}
	}
}
