using System;
using UnityEngine;

[Serializable]
public class PlayerStats
{
	public static event Action OnHealthChanged;

	public int poisonDamage;
	public int poisonDuration;
	public float poisonTickTime;

	public int shotDamage;
	public float secondsPerShot;
	public int shotVelocity;

	public int moveSpeed;
	[SerializeField] private int _maxHp;
	[SerializeField] private int _maxArmor;

	[SerializeField] private int _hp;
	[SerializeField] private int _armor;

	public int MAX_HEARTS_POSSIBLE = 20;
	public int MAX_ARMORS_POSSIBLE = 10;

	public int armor { get => _armor; set { _armor = (int)MathF.Min(value, MAX_ARMORS_POSSIBLE); OnHealthChanged?.Invoke(); } }
	public int hp { get => _hp; set { _hp = (int)MathF.Min(value, MAX_HEARTS_POSSIBLE); OnHealthChanged?.Invoke(); } }

	public int maxHp { get => _maxHp; set { _maxHp = (int)MathF.Min(value, MAX_HEARTS_POSSIBLE); OnHealthChanged?.Invoke(); } }
	public int maxArmor { get => _maxArmor; set { _maxArmor = (int)MathF.Min(value, MAX_HEARTS_POSSIBLE); OnHealthChanged?.Invoke(); } }
}
