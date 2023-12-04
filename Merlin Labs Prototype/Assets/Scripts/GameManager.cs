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

    [Header("Camera")]
    [SerializeField] private PerlinCameraShake scriptPerlinCameraShake;
    [SerializeField] private float cameraShakeTrauma;

    [Header("Player")]
    [SerializeField] private int playerArmor;
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerHealthMax;
    [SerializeField] private Image playerHealthFill;
    [SerializeField] private GameObject armorDisplay;
    [SerializeField] private TextMeshProUGUI textPlayerArmor;
    [SerializeField] private TextMeshProUGUI textPlayerHealth;
    [SerializeField] private Animator animatorPlayer;
    public int playerEnergy;
    private float playerFillDecrease;

    [Header("Enemy")]
    [SerializeField] private int enemyDamage;
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyHealthMax;
    [SerializeField] private Image enemyHealthFill;
    [SerializeField] private TextMeshProUGUI textEnemyHealth;
    [SerializeField] private Animator animatorEnemy;
    private float enemyFillDecrease;

    [Header("Energy")]
    [SerializeField] private TextMeshProUGUI textPlayerEnergy;

    [Header("Test")]
    [SerializeField] private bool testDamageEnemy;

    private void Start()
    {
        playerFillDecrease = 1f / playerHealthMax * playerHealthFill.rectTransform.sizeDelta.x;
        enemyFillDecrease = 1f / enemyHealthMax * enemyHealthFill.rectTransform.sizeDelta.x;
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
        for (int i = scriptCardPositioner.listCardTransform.Count - 1; i >= 0; i--)
        {
            var thisCard = scriptCardPositioner.listCardTransform[i].gameObject;
            scriptCardPositioner.listCardTransform.Remove(scriptCardPositioner.listCardTransform[i]);
            scriptCardPositioner.AssignCurrentCardPositions(scriptCardPositioner.listCardTransform.Count);
            Destroy(thisCard);
        }

        StartCoroutine(EnemyAttack(enemyDamage));
    }

    private IEnumerator EnemyAttack(int damage)
    {
        yield return new WaitForSeconds(1.0f);
        animatorEnemy.SetBool("isAttack", true);
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
                    var remainingDamage = enemyDamage - (i + 1);

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
        scriptCardSpawner.SpawnCards();
        FillEnergy();
    }

    public void PlayerAttack(int damage)
    {
        animatorPlayer.SetBool("isAttack", true);
        StartCoroutine(DamageEnemy(damage));
    }

    public IEnumerator DamageEnemy(int damage)
    {
        yield return new WaitForSeconds(0.75f);
        animatorEnemy.SetBool("isTakeDamage", true);

        for (int i = 0; i < damage; i++)
        {
            scriptPerlinCameraShake.CameraShake(cameraShakeTrauma);
            enemyHealth -= 1;
            var newWidth = enemyHealthFill.rectTransform.sizeDelta.x - enemyFillDecrease;
            enemyHealthFill.rectTransform.sizeDelta = new Vector2(newWidth, enemyHealthFill.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(0.03f);
            textEnemyHealth.text = enemyHealth + "/" + enemyHealthMax;
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
    }

    private void Update()
    {
        if (testDamageEnemy)
        {
            testDamageEnemy = false;
            PlayerAttack(15);
        }
    }
}
