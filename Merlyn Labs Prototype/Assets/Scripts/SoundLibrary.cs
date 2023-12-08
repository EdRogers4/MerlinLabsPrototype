using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Cards")]
    [SerializeField] private AudioClip[] cardDrop;
    [SerializeField] private AudioClip[] cardHighlight;
    [SerializeField] private AudioClip cardSpawn;

    [Header("UI")]
    [SerializeField] private AudioClip endTurn;
    [SerializeField] private AudioClip potionClose;
    [SerializeField] private AudioClip potionDiscard;
    [SerializeField] private AudioClip potionSelect;
    [SerializeField] private AudioClip potionUse;
    [SerializeField] private AudioClip tooltipHide;
    [SerializeField] private AudioClip tooltipShow;

    [Header("Player")]
    [SerializeField] private AudioClip[] playerSlash;
    [SerializeField] private AudioClip[] playerHit;
    [SerializeField] private AudioClip[] playerBlock;
    [SerializeField] private AudioClip playerArmorBreak;
    [SerializeField] private AudioClip playerDeath;

    [Header("Enemy")]
    [SerializeField] private AudioClip[] enemySlash;
    [SerializeField] private AudioClip[] enemyHit;
    [SerializeField] private AudioClip enemyWakeUp;

    [Header("Abilities")]
    [SerializeField] private AudioClip[] lightning;
    [SerializeField] private AudioClip gainArmor;
    [SerializeField] private AudioClip gainVengeance;
    [SerializeField] private AudioClip sleep;
    [SerializeField] private AudioClip heal;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SoundPlayerAttack()
    {
        audioSource.PlayOneShot(playerSlash[Random.Range(0, playerSlash.Length)], 0.7F);
        audioSource.PlayOneShot(playerHit[Random.Range(0, playerHit.Length)], 0.7F);
    }

    public void SoundEnemyAttack()
    {
        audioSource.PlayOneShot(enemySlash[Random.Range(0, enemySlash.Length)], 0.7F);
        audioSource.PlayOneShot(enemyHit[Random.Range(0, enemySlash.Length)], 0.7F);
    }
}
