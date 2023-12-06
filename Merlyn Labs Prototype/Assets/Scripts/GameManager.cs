using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private CardPositioner scriptCardPositioner;
    [SerializeField] private CardSpawner scriptCardSpawner;
    [SerializeField] private CardsLibrary scriptCardsLibrary;
    public bool isPlantFeet;
    public bool isHiltPunch;
    public int modifierPlantFeet;
    public int modifierHiltPunch;

    [Header("Camera")]
    [SerializeField] private PerlinCameraShake scriptPerlinCameraShake;
    [SerializeField] private float cameraShakeTrauma;

    [Header("Game Status")]
    public bool isCardUnavailableToPlay;
    [SerializeField] private bool isEndTurnUnavailable;

    [Header("Player")]
    public int playerArmor;
    [SerializeField] private int playerVengeance;
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerHealthMax;
    [SerializeField] private Image playerHealthFill;
    [SerializeField] private GameObject armorDisplay;
    [SerializeField] private GameObject vengeanceDisplay;
    [SerializeField] private TextMeshProUGUI textPlayerArmor;
    [SerializeField] private TextMeshProUGUI textPlayerVengeance;
    [SerializeField] private TextMeshProUGUI textPlayerHealth;
    [SerializeField] private Animator animatorPlayer;
    public int playerEnergy;
    private float playerFillDecrease;

    [Header("Enemy")]
    public bool[] isEnemyDead;
    [SerializeField] private int[] enemyDamage;
    [SerializeField] private int[] enemyHealth;
    [SerializeField] private int[] enemyHealthMax;
    [SerializeField] private Image[] enemyHealthFill;
    [SerializeField] private TextMeshProUGUI[] textEnemyHealth;
    [SerializeField] private Animator[] animatorEnemy;
    private float enemyFillDecrease;

    [Header("Energy")]
    [SerializeField] private TextMeshProUGUI textPlayerEnergy;

    [Header("Test")]
    [SerializeField] private bool testDamageEnemy;

    private void Start()
    {
        playerFillDecrease = 1f / playerHealthMax * playerHealthFill.rectTransform.sizeDelta.x;
        enemyFillDecrease = 1f / enemyHealthMax[0] * enemyHealthFill[0].rectTransform.sizeDelta.x;
        FillEnergy();
    }

    private void FillEnergy()
    {
        playerEnergy = 3;
        textPlayerEnergy.text = playerEnergy + "";
    }

    public void UseEnergy(int cost)
    {
        playerEnergy = playerEnergy - cost;
        textPlayerEnergy.text = playerEnergy + "";
    }

    public void EndTurn()
    {
        if (!isEndTurnUnavailable)
        {
            isEndTurnUnavailable = true;

            for (int i = scriptCardPositioner.listCardTransform.Count - 1; i >= 0; i--)
            {
                var thisCard = scriptCardPositioner.listCardTransform[i].gameObject;
                scriptCardPositioner.listCardTransform.Remove(scriptCardPositioner.listCardTransform[i]);
                scriptCardPositioner.AssignCurrentCardPositions(scriptCardPositioner.listCardTransform.Count);
                Destroy(thisCard);
            }

            StartCoroutine(EnemyAttack(enemyDamage[0]));
        }
    }

    private void CheckCardToPlayStatus()
    {
        if (isCardUnavailableToPlay)
        {
            isCardUnavailableToPlay = false;
        }
    }

    private IEnumerator EnemyAttack(int damage)
    {
        for (int h = 0; h < 2; h++)
        {
            if (!isEnemyDead[h])
            {
                yield return new WaitForSeconds(1.0f);
                animatorEnemy[h].SetBool("isAttack", true);
                yield return new WaitForSeconds(0.75f);
                animatorPlayer.SetBool("isTakeDamage", true);

                if (playerArmor <= 0)
                {
                    for (int i = 0; i < damage; i++)
                    {
                        playerHealth -= 1;
                        var newWidth = playerHealthFill.rectTransform.sizeDelta.x - playerFillDecrease;
                        playerHealthFill.rectTransform.sizeDelta = new Vector2(newWidth, playerHealthFill.rectTransform.sizeDelta.y);
                        yield return new WaitForSeconds(0.03f);
                        textPlayerHealth.text = playerHealth + "/" + playerHealthMax;
                    }
                }
                else
                {
                    for (int i = 0; i < damage; i++)
                    {
                        if (playerArmor <= 0)
                        {
                            var remainingDamage = enemyDamage[h] - (i + 1);

                            for (int j = 0; j < remainingDamage; j++)
                            {
                                playerHealth -= 1;
                                var newWidth = playerHealthFill.rectTransform.sizeDelta.x - playerFillDecrease;
                                playerHealthFill.rectTransform.sizeDelta = new Vector2(newWidth, playerHealthFill.rectTransform.sizeDelta.y);
                                yield return new WaitForSeconds(0.03f);
                                textPlayerHealth.text = playerHealth + "/" + playerHealthMax;
                            }
                            break;
                        }

                        playerArmor -= 1;
                        textPlayerArmor.text = playerArmor + "";

                        if (playerArmor <= 0)
                        {
                            armorDisplay.SetActive(false);
                        }

                        yield return new WaitForSeconds(0.1f);
                    }
                }

                yield return new WaitForSeconds(1.25f);
            }
        }

        scriptCardSpawner.SpawnCards();
        FillEnergy();
        isEndTurnUnavailable = false;
    }

    public void PlayerAttack(int damage, int enemyToAttack)
    {
        animatorPlayer.SetBool("isAttack", true);
        StartCoroutine(DamageEnemy(damage, enemyToAttack));
    }

    public IEnumerator DamageEnemy(int damage, int enemyToAttack)
    {
        yield return new WaitForSeconds(0.75f);
        animatorEnemy[enemyToAttack].SetBool("isTakeDamage", true);

        for (int i = 0; i < damage; i++)
        {
            scriptPerlinCameraShake.CameraShake(cameraShakeTrauma);
            enemyHealth[enemyToAttack] -= 1;
            var newWidth = enemyHealthFill[enemyToAttack].rectTransform.sizeDelta.x - enemyFillDecrease;
            enemyHealthFill[enemyToAttack].rectTransform.sizeDelta = new Vector2(newWidth, enemyHealthFill[enemyToAttack].rectTransform.sizeDelta.y);
            textEnemyHealth[enemyToAttack].text = enemyHealth[enemyToAttack] + "/" + enemyHealthMax[enemyToAttack];
            yield return new WaitForSeconds(0.1f);

            if (enemyHealth[enemyToAttack] <= 0)
            {
                animatorEnemy[enemyToAttack].SetBool("isDeath", true);
                isEnemyDead[enemyToAttack] = true;

                if (enemyToAttack == 0)
                {
                    scriptCardPositioner.enemyTargeted = 2;
                }
                else
                {
                    scriptCardPositioner.enemyTargeted = 1;
                }

                if (playerVengeance > 0)
                {
                    yield return new WaitForSeconds(2.0f);
                    StartCoroutine(GainHealth());
                }

                if (isEnemyDead[0] && isEnemyDead[1])
                {
                    StartCoroutine(RespawnEnemies());
                }

                break;
            }
        }

        if (isHiltPunch)
        {
            isHiltPunch = false;

            if (modifierHiltPunch > 1)
            {
                modifierHiltPunch -= 1;
                scriptCardsLibrary.cardDescription[5] = "Deal " + modifierHiltPunch + " damage.  Lower this cards damage by 1 each use during this combat.";
            }
        }

        CheckCardToPlayStatus();
    }

    public IEnumerator GainHealth()
    {
        for (int i = 0; i < playerVengeance; i++)
        {
            if (playerHealth >= playerHealthMax)
            {
                break;
            }

            playerHealth += 1;
            var newWidth = playerHealthFill.rectTransform.sizeDelta.x + playerFillDecrease;
            playerHealthFill.rectTransform.sizeDelta = new Vector2(newWidth, playerHealthFill.rectTransform.sizeDelta.y);
            textPlayerHealth.text = playerHealth + "/" + playerHealthMax;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public IEnumerator GainArmor(int defense)
    {
        for (int i = 0; i < defense; i++)
        {
            playerArmor += 1;

            if (playerArmor > 0)
            {
                armorDisplay.SetActive(true);
            }

            textPlayerArmor.text = "" + playerArmor;
            yield return new WaitForSeconds(0.1f);
        }

        if (isPlantFeet)
        {
            isPlantFeet = false;
            modifierPlantFeet = modifierPlantFeet * 2;
            scriptCardsLibrary.cardDescription[3] = "Gain " + modifierPlantFeet + " defense.  Doubled each use for the remainder of combat.";
        }

        CheckCardToPlayStatus();
    }

    public IEnumerator GainVengeance(int vengeance, int enemyTargeted)
    {
        for (int i = 0; i < vengeance; i++)
        {
            playerVengeance += 1;

            if (!vengeanceDisplay.active)
            {
                vengeanceDisplay.SetActive(true);
            }

            textPlayerVengeance.text = "" + playerVengeance;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.25f);
        PlayerAttack(6, enemyTargeted);
    }

    private IEnumerator RespawnEnemies()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++)
        {
            animatorEnemy[i].SetBool("isDeath", false);
            enemyHealth[i] = enemyHealthMax[i];
            enemyHealthFill[i].rectTransform.sizeDelta = new Vector2(250, enemyHealthFill[i].rectTransform.sizeDelta.y);
            textEnemyHealth[i].text = enemyHealth[i] + "/" + enemyHealthMax[i];
            isEnemyDead[i] = false;
        }
    }

    private void Update()
    {
    }
}
