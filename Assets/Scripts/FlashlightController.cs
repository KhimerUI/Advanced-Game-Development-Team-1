using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public float maxBattery = 100f;
    public float drainRate = 5f;
    public float lowBatteryThreshold = 25f;
    public float flickerSpeed = 0.08f;

    private Light flashlight;
    private float currentBattery;
    private bool isOn = false;
    private float flickerTimer = 0f;

    void Start()
    {
        flashlight = GetComponent<Light>();
        currentBattery = maxBattery;
        flashlight.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isOn && currentBattery > 0)
            {
                isOn = true;
                flashlight.enabled = true;
            }
            else
            {
                isOn = false;
                flashlight.enabled = false;
            }
        }

        if (isOn)
        {
            currentBattery -= drainRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            if (currentBattery <= 0)
            {
                isOn = false;
                flashlight.enabled = false;
                return;
            }

            if (currentBattery <= lowBatteryThreshold)
            {
                flickerTimer -= Time.deltaTime;
                if (flickerTimer <= 0f)
                {
                    flashlight.enabled = !flashlight.enabled;
                    flickerTimer = flickerSpeed + Random.Range(-0.02f, 0.04f);
                }
            }
        }
    }

    void OnGUI()
    {
        float percent = currentBattery / maxBattery;

        // Pick color based on battery level
        Color barColor;
        if (percent > 0.5f)
            barColor = Color.green;
        else if (percent > 0.25f)
            barColor = Color.yellow;
        else
            barColor = Color.red;

        // Background (dark bar)
        GUI.color = new Color(0, 0, 0, 0.6f);
        GUI.DrawTexture(new Rect(20, 20, 204, 24), Texture2D.whiteTexture);

        // Battery fill
        GUI.color = barColor;
        GUI.DrawTexture(new Rect(22, 22, 200 * percent, 20), Texture2D.whiteTexture);

        // Label
        GUI.color = Color.white;
        GUI.Label(new Rect(20, 46, 200, 20), "BATTERY: " + Mathf.RoundToInt(currentBattery) + "%");

        // Flashlight status
        GUI.color = isOn ? Color.white : new Color(1f, 1f, 1f, 0.4f);
        GUI.Label(new Rect(20, 64, 200, 20), isOn ? "[ F ] Flashlight: ON" : "[ F ] Flashlight: OFF");

        // Dead battery warning
        if (currentBattery <= 0)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(20, 84, 200, 20), "!! BATTERY DEAD !!");
        }

        GUI.color = Color.white; // reset
    }
}