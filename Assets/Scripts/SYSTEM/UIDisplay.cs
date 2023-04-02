using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
	[SerializeField] Transform heartsParent, shieldsParent;
	[SerializeField] TextMeshProUGUI psnDmgText, psnDurText, psnTickText, atkDmgText, atkTimeText, atkVelText, moveSpdText;
	[SerializeField] GameObject gameOverPanel, bossHealthbar;
	[SerializeField] Image bossHealthbarFill;

	private void Start()
	{
		Loot_OnAnyStatLootBonusPicked();
		PlayerStats_OnHealthChanged();
	}

	private void OnEnable()
	{
		PlayerStats.OnHealthChanged += PlayerStats_OnHealthChanged;
		Player.OnDeath += Player_OnDeath;
		Boss.OnAnyBossSpawn += Boss_OnAnyBossSpawn;
		Boss.OnBossHealthChanged += Boss_OnBossHealthChanged;
		Boss.OnAnyEnemyKilled += Boss_OnAnyEnemyKilled;
		Loot.OnAnyStatLootBonusPicked += Loot_OnAnyStatLootBonusPicked;
	}

	private void OnDisable()
	{
		PlayerStats.OnHealthChanged -= PlayerStats_OnHealthChanged;
		Player.OnDeath -= Player_OnDeath;
		Boss.OnAnyBossSpawn -= Boss_OnAnyBossSpawn;
		Boss.OnBossHealthChanged -= Boss_OnBossHealthChanged;
		Boss.OnAnyEnemyKilled -= Boss_OnAnyEnemyKilled;
		Loot.OnAnyStatLootBonusPicked -= Loot_OnAnyStatLootBonusPicked;
	}

	public void PlayerStats_OnHealthChanged()
	{
		int emptyHearts = Player.instance.stats.maxHp;
		int hearts = Player.instance.stats.hp;
		int emptyArmors = Player.instance.stats.maxArmor;
		int armors = Player.instance.stats.armor;

		for (int i = 0; i < Player.instance.stats.MAX_HEARTS_POSSIBLE; i++)
		{
			Transform heart = heartsParent.GetChild(i);
			heart.gameObject.SetActive(i < emptyHearts);
			heart.GetChild(0).gameObject.SetActive(i < hearts);
		}

		for (int i = 0; i < Player.instance.stats.MAX_ARMORS_POSSIBLE; i++)
		{
			Transform heart = shieldsParent.GetChild(i);
			heart.gameObject.SetActive(i < emptyArmors);
			heart.GetChild(0).gameObject.SetActive(i < armors);
		}
	}

	private void Player_OnDeath()
	{
		gameOverPanel.SetActive(true);
	}

	private void Boss_OnBossHealthChanged(float current, float max)
	{
		bossHealthbarFill.fillAmount = current / max;
	}

	private void Boss_OnAnyBossSpawn()
	{
		bossHealthbar.SetActive(true);
	}

	private void Boss_OnAnyEnemyKilled()
	{
		bossHealthbar.SetActive(false);
	}

	private void Loot_OnAnyStatLootBonusPicked()
	{
		psnDmgText.text = Player.instance.stats.poisonDamage.ToString();
		psnDurText.text = Player.instance.stats.poisonDuration.ToString();
		psnTickText.text = Player.instance.stats.poisonTickTime.ToString();
		atkDmgText.text = Player.instance.stats.shotDamage.ToString();
		atkTimeText.text = Player.instance.stats.secondsPerShot.ToString();
		atkVelText.text = Player.instance.stats.shotVelocity.ToString();
		moveSpdText.text = Player.instance.stats.moveSpeed.ToString();
	}
}
