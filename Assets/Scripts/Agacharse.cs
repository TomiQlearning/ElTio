using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agacharse : MonoBehaviour
{
    public float smoothCrouch;
    public bool crouch;

    public Image StaminaBar;
    public float RunCost;
    public float MaxStamina = 100f;
    public float RechargeRate = 2f; // Tasa de recarga más lenta
    private float currentStamina;
    private bool isShiftActive = true;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = MaxStamina;
        StaminaBar.fillAmount = currentStamina / MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Agacharse
        crouch = Input.GetKey(KeyCode.LeftControl);

        float targetLocalScaleY = crouch ? 0.65f : 1f;
        float newScaleY = Mathf.Lerp(transform.localScale.y, targetLocalScaleY, Time.deltaTime * smoothCrouch);

        transform.localScale = new Vector3(1, newScaleY, 1);

        // Stamina
        if (isShiftActive && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("corriendo");

            currentStamina -= RunCost * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isShiftActive = false; // Desactiva la tecla Shift cuando la estamina se agota
            }
            StaminaBar.fillAmount = currentStamina / MaxStamina;
        }
        else
        {
            // Recuperar estamina cuando no se está corriendo
            currentStamina += RechargeRate * Time.deltaTime;
            if (currentStamina >= MaxStamina)
            {
                currentStamina = MaxStamina;
                isShiftActive = true; // Reactiva la tecla Shift cuando la estamina está completamente recargada
            }
            StaminaBar.fillAmount = currentStamina / MaxStamina;
        }
    }
}







       

