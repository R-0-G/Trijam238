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
    [SerializeField] private GameObject pushAnim;
    [SerializeField] private GameObject pullAnim;
    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private GameObject alreadyWon;
   
    private float stamina = 1f;
    private bool recoverStamina = false;
    private float forceToUse = 0f;

    private Vector3 initialPosition;
    private Vector3 ballPosition;
    private bool active = false;

    [ContextMenu("ClearPlayerprefs")]
    public void ResetPlayerprefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Awake()
    {
        initialPosition = transform.position;
        ballPosition = ball.transform.position;

        if (PlayerPrefs.GetInt("WON", 0) == 1)
        {
            alreadyWon.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Game.gameActive)
        {
            active = true;
            ball.isKinematic = false;
            staminaSlider.gameObject.SetActive(stamina<1f);
            staminaSlider.value = stamina;
            forceToUse = force;
            if (stamina <= 0f)
            {
                forceToUse = staminaLostForce;
            }
            if (Input.GetKey((KeyCode.A)))
            {
                pullAnim.SetActive(true);
                pushAnim.SetActive(false);
                rb2d.AddForce(Vector2.left * forceToUse * Time.deltaTime);
            }
            else if (Input.GetKey((KeyCode.D)))
            {
                pullAnim.SetActive(false);
                pushAnim.SetActive(true);
                rb2d.AddForce(Vector2.right * forceToUse * Time.deltaTime);
            }
            else
            {
                pullAnim.SetActive(false);
                pushAnim.SetActive(true);
            }
        }
        else
        {
            if (active)
            {
                transform.position = initialPosition;
                ball.transform.position = ballPosition;
            }
            ball.isKinematic = true;
            pullAnim.SetActive(false);
            pushAnim.SetActive(true);
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
