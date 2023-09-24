using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float force;
    [SerializeField] private float staminaLostForce;
    [SerializeField] private float staminaLossRate;
    [SerializeField] private float staminaRecoveryRate;
    [SerializeField] private Slider staminaSlider;
   
    private float stamina = 1f;
    private bool recoverStamina = false;
    private float forceToUse = 0f;
    private void Update()
    {
        
        if (Game.gameActive)
        {
            staminaSlider.gameObject.SetActive(stamina<1f);
            staminaSlider.value = stamina;
            forceToUse = force;
            if (stamina <= 0f)
            {
                forceToUse = staminaLostForce;
            }
            if (Input.GetKey((KeyCode.A)))
            {
                rb2d.AddForce(Vector2.left * forceToUse * Time.deltaTime);
            }
            if (Input.GetKey((KeyCode.D)))
            {
                rb2d.AddForce(Vector2.right * forceToUse * Time.deltaTime);
            }
        }
        else
        {
            staminaSlider.gameObject.SetActive(false);
            stamina = 1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                recoverStamina = true;
                Game.TriggerGameStart();
            }
        }
    }

    private void FixedUpdate()
    {
        if (recoverStamina && stamina < 1f)
        {
            stamina += staminaRecoveryRate * Time.fixedDeltaTime;
            if (stamina > 1f)
            {
                stamina = 1f;
            }
        }
        else if ( !recoverStamina && stamina > 0f)
        {
            
            stamina -= staminaLossRate * Time.fixedDeltaTime;
            if (stamina < 0f)
            {
                stamina = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name =="Ball")
        {
            recoverStamina = false;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name =="Ball")
        {
            recoverStamina = true;
        }
    }
}
