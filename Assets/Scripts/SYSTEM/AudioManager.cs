using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioClip bgm;
	[SerializeField] AudioClip[] whooshes;
	[SerializeField] AudioClip[] playerHits;
	[SerializeField] AudioClip[] playerDeaths;

	[SerializeField] AudioClip[] arrows;
	[SerializeField] AudioClip[] enemyDeaths;
	[SerializeField] AudioClip[] bossDeaths;

	[SerializeField] AudioSource bgmSource;
	[SerializeField] AudioSource sfxSource;

	public static AudioManager instance;

	private void Awake()
	{
		if (instance) Destroy(instance);
		instance = this;
	}

	private void Start()
	{
		bgmSource.clip = bgm;
		bgmSource.loop = true;
		bgmSource.Play();
	}

	public void PlayWhoosh()
	{
		sfxSource.PlayOneShot(whooshes[Random.Range(0, whooshes.Length)]);
	}

	public void PlayPlayerHit()
	{
		sfxSource.PlayOneShot(playerHits[Random.Range(0, playerHits.Length)]);
	}

	public void PlayPlayerDeath()
	{
		sfxSource.PlayOneShot(playerDeaths[Random.Range(0, playerDeaths.Length)]);
	}

	public void PlayArrow()
	{
		sfxSource.PlayOneShot(arrows[Random.Range(0, arrows.Length)]);
	}

	public void PlayEnemyDeath()
	{
		sfxSource.PlayOneShot(enemyDeaths[Random.Range(0, enemyDeaths.Length)]);
	}

	public void PlayBossDeath()
	{
		sfxSource.PlayOneShot(bossDeaths[Random.Range(0, bossDeaths.Length)]);
	}
}
