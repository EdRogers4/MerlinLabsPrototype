using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private PerlinCameraShake scriptPerlinCameraShake;
    [SerializeField] private float cameraShakeTrauma;

    [Header("Player")]
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerHealthMax;
    [SerializeField] private Image playerHealthFill;
    [SerializeField] private TextMeshProUGUI textPlayerHealth;
    [SerializeField] private Animator animatorPlayer;

    [Header("Enemy")]
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyHealthMax;
    [SerializeField] private Image enemyHealthFill;
    [SerializeField] private TextMeshProUGUI textEnemyHealth;
    [SerializeField] private Animator animatorEnemy;
    private float fillDecrease;

    [Header("Test")]
    [SerializeField] private bool testDamageEnemy;

    private void Start()
    {
        fillDecrease = 1f / enemyHealthMax * enemyHealthFill.rectTransform.sizeDelta.x;
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
            var newWidth = enemyHealthFill.rectTransform.sizeDelta.x - fillDecrease;
            enemyHealthFill.rectTransform.sizeDelta = new Vector2(newWidth, enemyHealthFill.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(0.03f);
            textEnemyHealth.text = enemyHealth + "/" + enemyHealthMax;
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
