using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private CardPositioner scriptCardPositioner;
    [SerializeField] private CardSpawner scriptCardSpawner;
    [SerializeField] private CardsLibrary scriptCardsLibrary;
    public bool isPlantFeet;
    public bool isHiltPunch;
    public bool isKnightsResolve;
    public bool isFlurry;
    public int modifierPlantFeet;
    public int modifierHiltPunch;
    public int counterRoundhouse;

    [Header("Camera")]
    [SerializeField] private PerlinCameraShake scriptPerlinCameraShake;
    [SerializeField] private float cameraShakeTrauma;

    [Header("Game Status")]
    public bool isCardUnavailableToPlay;
    [SerializeField] private bool isEndTurnUnavailable;
    [SerializeField] private GameObject buttonEndTurn;
    [SerializeField] private GameObject deathScreen;

    [Header("Player")]
    public bool isPlayerDead;
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
    [SerializeField] private ParticleSystem[] particlePlayerDamage;
    public int playerEnergy;
    private float playerFillDecrease;

    [Header("Enemy")]
    public bool[] isEnemyDead;
    [SerializeField] private int[] enemyDamage;
    [SerializeField] private int[] enemyHealth;
    [SerializeField] private int[] enemyHealthMax;
    [SerializeField] private Image[] enemyHealthFill;
    [SerializeField] private TextMeshProUGUI[] textEnemyHealth;
    [SerializeField] private TextMeshProUGUI[] textEnemyZZZ;
    [SerializeField] private Animator[] animatorEnemy;
    [SerializeField] private bool[] isEnemySleep;
    [SerializeField] private ParticleSystem[] particleSlashEnemy0;
    [SerializeField] private ParticleSystem[] particleSlashEnemy1;
    private float enemyFillDecrease;
    private bool isShowEnemyZZZ;
    private int countEnemyZZZ;

    [Header("Energy")]
    [SerializeField] private TextMeshProUGUI textPlayerEnergy;

    [Header("Potions")]
    public bool isUsePotion;
    [SerializeField] private GameObject[] potions;
    [SerializeField] private GameObject potionMenu;
    [SerializeField] private Animator animatorSelectEnemy;
    private int selectedPotion;

    [Header("Popups")]
    [SerializeField] private GameObject tooltipAbility;
    [SerializeField] private TextMeshProUGUI textTooltipDescription;
    [SerializeField] private Animator[] animatorPopupTextEnemy;
    [SerializeField] private Animator animatorPopupTextPlayerDamage;
    [SerializeField] private Animator animatorPopupTextPlayerHealth;
    [SerializeField] private TextMeshProUGUI[] textPopupDamageEnemy;
    [SerializeField] private TextMeshProUGUI textPopupHealthPlayer;

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
            if (!isEnemyDead[h] && !isEnemySleep[h])
            {
                yield return new WaitForSeconds(1.0f);
                animatorEnemy[h].SetBool("isAttack", true);
                particlePlayerDamage[Random.Range(0, particlePlayerDamage.Length)].Play();
                yield return new WaitForSeconds(0.75f);
                animatorPlayer.SetBool("isTakeDamage", true);
                animatorPopupTextPlayerDamage.SetBool("isShow", true);

                if (playerArmor <= 0)
                {
                    for (int i = 0; i < damage; i++)
                    {
                        playerHealth -= 1;
                        var newWidth = playerHealthFill.rectTransform.sizeDelta.x - playerFillDecrease;
                        playerHealthFill.rectTransform.sizeDelta = new Vector2(newWidth, playerHealthFill.rectTransform.sizeDelta.y);
                        yield return new WaitForSeconds(0.03f);
                        textPlayerHealth.text = playerHealth + "/" + playerHealthMax;

                        if (playerHealth <= 0)
                        {
                            animatorPlayer.SetBool("isDeath", true);
                            isPlayerDead = true;
                            buttonEndTurn.SetActive(false);
                            yield return new WaitForSeconds(1.0f);
                            deathScreen.SetActive(true);
                            break;
                        }
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

                yield return new WaitForSeconds(0.5f);
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
        if (isFlurry)
        {
            UseEnergy(1);
        }

        if (counterRoundhouse > 0)
        {
            counterRoundhouse -= 1;
        }

        if (enemyToAttack == 0)
        {
            particleSlashEnemy0[Random.Range(0, particleSlashEnemy0.Length)].Play();
        }
        else
        {
            particleSlashEnemy1[Random.Range(0, particleSlashEnemy0.Length)].Play();
        }

        yield return new WaitForSeconds(0.75f);

        if (isEnemySleep[enemyToAttack])
        {
            isEnemySleep[enemyToAttack] = false;
            textEnemyZZZ[enemyToAttack].text = "";
            animatorEnemy[enemyToAttack].SetBool("isSleep", false);
        }

        animatorEnemy[enemyToAttack].SetBool("isTakeDamage", true);
        animatorPopupTextEnemy[enemyToAttack].SetBool("isShow", true);
        textPopupDamageEnemy[enemyToAttack].text = "+" + damage + " Damage!";

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

        if (isFlurry && !isEnemyDead[enemyToAttack] && playerEnergy > 0)
        {
            PlayerAttack(6, enemyToAttack);
        }
        else if (counterRoundhouse > 0 && !isEnemyDead[enemyToAttack])
        {
            PlayerAttack(7, enemyToAttack);
        }
        else
        {
            if (isFlurry)
            {
                isFlurry = false;
            }

            CheckCardToPlayStatus();
        }
    }

    public IEnumerator GainHealth()
    {
        animatorPopupTextPlayerHealth.SetBool("isShow", true);
        textPopupHealthPlayer.text = "+" + playerVengeance + "HP!";

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
        if (isKnightsResolve)
        {
            UseEnergy(1);
        }

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

        if (isKnightsResolve && playerEnergy > 0)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(GainArmor(6));
        }
        else
        {
            if (isKnightsResolve)
            {
                isKnightsResolve = false;
            }

            CheckCardToPlayStatus();
        }
    }

    public IEnumerator GainVengeance(int vengeance, int enemyTargeted)
    {
        for (int i = 0; i < vengeance; i++)
        {
            playerVengeance += 1;

            if (!vengeanceDisplay.activeSelf)
            {
                vengeanceDisplay.SetActive(true);
            }

            textPlayerVengeance.text = "" + playerVengeance;
            yield return new WaitForSeconds(0.1f);
        }

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

    public void TogglePotionMenu(int currentPotion)
    {
        selectedPotion = currentPotion;

        if (potionMenu.activeSelf)
        {
            potionMenu.SetActive(false);
        }
        else
        {
            potionMenu.SetActive(true);
        }
    }

    public void UsePotion()
    {
        isUsePotion = true;
        TogglePotionMenu(selectedPotion);
        animatorSelectEnemy.SetBool("isShow", true);
    }

    public void DiscardPotion()
    {
        Destroy(potions[selectedPotion]);
        TogglePotionMenu(selectedPotion);
    }

    public void PutEnemyToSleep(int index)
    {
        if (isUsePotion)
        {
            isUsePotion = false;
            isEnemySleep[index] = true;
            animatorSelectEnemy.SetBool("isShow", false);
            animatorEnemy[index].SetBool("isSleep", true);
            Destroy(potions[selectedPotion]);

            if (!isShowEnemyZZZ)
            {
                isShowEnemyZZZ = true;
                StartCoroutine(EnemyShowZZZ());
            }
        }
    }

    private IEnumerator EnemyShowZZZ()
    {
        for (int i = 0; i < isEnemySleep.Length; i++)
        {
            if (isEnemySleep[i])
            {
                switch (countEnemyZZZ)
                {
                    case 0:
                        textEnemyZZZ[i].text = "z";
                        break;
                    case 1:
                        textEnemyZZZ[i].text = "zZ";
                        break;
                    case 2:
                        textEnemyZZZ[i].text = "zZz";
                        break;
                    case 3:
                        textEnemyZZZ[i].text = "";
                        break;
                }
            }
        }

        if (countEnemyZZZ >= 3)
        {
            countEnemyZZZ = 0;
        }
        else
        {
            countEnemyZZZ += 1;
        }

        yield return new WaitForSeconds(0.5f);

        if (isEnemySleep[0] || isEnemySleep[1])
        {
            StartCoroutine(EnemyShowZZZ());
        }
        else
        {
            isShowEnemyZZZ = false;
        }
    }

    public void ShowTooltip()
    {
        tooltipAbility.SetActive(true);
        textTooltipDescription.text = "Each time you kill an enemy, gain " + playerVengeance + " HP";
    }

    public void HideTooltip()
    {
        tooltipAbility.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }    

    private void Update()
    {
        
    }
}
