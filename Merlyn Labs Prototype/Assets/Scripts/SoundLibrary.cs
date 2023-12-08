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
    [SerializeField] private AudioClip targetEnemy;

    [Header("Player")]
    [SerializeField] private AudioClip[] playerSlash;
    [SerializeField] private AudioClip[] playerHit;
    [SerializeField] private AudioClip[] playerBlock;
    [SerializeField] private AudioClip playerArmorBreak;
    [SerializeField] private AudioClip playerDeath;

    [Header("Enemy")]
    [SerializeField] private AudioClip[] enemySlash;
    [SerializeField] private AudioClip[] enemyHit;
    [SerializeField] private AudioClip[] enemyAttack;
    [SerializeField] private AudioClip[] enemyTakeDamage;
    [SerializeField] private AudioClip[] enemyDeath;
    [SerializeField] private AudioClip[] enemySpawn;
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

    public void SoundCardDrop()
    {
        audioSource.PlayOneShot(cardDrop[Random.Range(0, cardDrop.Length)], 1.0F);
    }

    public void SoundCardHighlight()
    {
        audioSource.PlayOneShot(cardHighlight[Random.Range(0, cardHighlight.Length)], 1.0F);
    }

    public void SoundCardSpawn()
    {
        if (cardSpawn != null)
        {
            audioSource.PlayOneShot(cardSpawn, 1.0F);
        }
    }

    public void SoundEndTurn()
    {
        audioSource.PlayOneShot(endTurn, 1.0F);
    }

    public void SoundPotionClose()
    {
        audioSource.PlayOneShot(potionClose, 1.0F);
    }

    public void SoundPotionDiscard()
    {
        audioSource.PlayOneShot(potionDiscard, 1.0F);
    }

    public void SoundPotionSelect()
    {
        audioSource.PlayOneShot(potionSelect, 1.0F);
    }

    public void SoundPotionUse()
    {
        audioSource.PlayOneShot(potionUse, 1.0F);
    }

    public void SoundTooltipHide()
    {
        audioSource.PlayOneShot(tooltipHide, 1.0F);
    }

    public void SoundTooltipShow()
    {
        audioSource.PlayOneShot(tooltipShow, 1.0F);
    }

    public void SoundTargetEnemy()
    {
        audioSource.PlayOneShot(targetEnemy, 0.7f);
    }

    public void SoundPlayerAttack()
    {
        audioSource.PlayOneShot(playerSlash[Random.Range(0, playerSlash.Length)], 1.0F);
        audioSource.PlayOneShot(playerHit[Random.Range(0, playerHit.Length)], 1.0F);
    }

    public void SoundPlayerBlock()
    {
        audioSource.PlayOneShot(playerBlock[Random.Range(0, playerBlock.Length)], 1.0F);
    }

    public void SoundPlayerArmorBreak()
    {
        audioSource.PlayOneShot(playerArmorBreak, 1.0F);
    }

    public void SoundPlayerDeath()
    {
        audioSource.PlayOneShot(playerDeath, 1.0F);
    }

    public void SoundEnemySlash ()
    {
        audioSource.PlayOneShot(enemySlash[Random.Range(0, enemySlash.Length)], 1.0F);
        audioSource.PlayOneShot(enemyHit[Random.Range(0, enemySlash.Length)], 1.0F);
    }

    public void SoundEnemyAttack()
    {
        audioSource.PlayOneShot(enemyAttack[Random.Range(0, enemyAttack.Length)], 1.0F);
    }

    public void SoundEnemyTakeDamage()
    {
        audioSource.PlayOneShot(enemyTakeDamage[Random.Range(0, enemyTakeDamage.Length)], 1.0F);
    }

    public void SoundEnemyDeath()
    {
        audioSource.PlayOneShot(enemyDeath[Random.Range(0, enemyDeath.Length)], 1.0F);
    }

    public void SoundEnemySpawn()
    {
        audioSource.PlayOneShot(enemySpawn[Random.Range(0, enemySpawn.Length)], 1.0F);
    }

    public void SoundEnemyWakeUp()
    {
        audioSource.PlayOneShot(enemyWakeUp, 1.0F);
    }

    public void SoundLightning()
    {
        for (int i = 0; i < lightning.Length; i++)
        {
            audioSource.PlayOneShot(lightning[i], 0.7F);
        }
    }

    public void SoundGainArmor()
    {
        audioSource.PlayOneShot(gainArmor, 1.0F);
    }

    public void SoundGainVengeance()
    {
        audioSource.PlayOneShot(gainVengeance, 1.0F);
    }

    public void SoundSleep()
    {
        audioSource.PlayOneShot(sleep, 1.0F);
    }

    public void SoundHeal()
    {
        audioSource.PlayOneShot(heal, 1.0F);
    }
}
