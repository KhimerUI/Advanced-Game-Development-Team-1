using UnityEngine;
using UnityEngine.UI;

public class SprintController : MonoBehaviour
{
    public float sprintMultiplier = 1.75f;
    public float maxStamina = 100f;
    public float drainRate = 25f;
    public float regenRate = 15f;
    public Image staminaFill;

    private float currentStamina;
    private PlayerController playerController;
    private float baseSpeed;
    private bool exhausted;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        baseSpeed = playerController.speed;
        currentStamina = maxStamina;
        exhausted = false;
    }

    void Update()
    {
        if (currentStamina <= 0f)
        {
            exhausted = true;
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            exhausted = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f && !exhausted)
        {
            playerController.speed = baseSpeed * sprintMultiplier;
            currentStamina -= drainRate * Time.deltaTime;
            if (currentStamina < 0f) currentStamina = 0f;
        }
        else
        {
            playerController.speed = baseSpeed;
            currentStamina += regenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }

        if (staminaFill != null)
        {
            staminaFill.fillAmount = currentStamina / maxStamina;
        }
    }
}