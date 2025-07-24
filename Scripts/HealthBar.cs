using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health playerHealth; // Reference to the Health script on the player

    [SerializeField]
    private UnityEngine.UI.Image totalHealthBar; // The full health bar image

    [SerializeField]
    private UnityEngine.UI.Image currentHealthBar; // The current health bar image

    private void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
