using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioClip bgm;
	[SerializeField] AudioClip[] whooshes;

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
}
