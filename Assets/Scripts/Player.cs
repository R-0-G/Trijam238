using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float force;
    [SerializeField] private float ballPushForce;
    [SerializeField] private float staminaLostBallPushForce;
    [SerializeField] private float staminaLostForce;
    [SerializeField] private float staminaLossRate;
    [SerializeField] private float staminaRecoveryRate;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private GameObject pushAnim;
    [SerializeField] private GameObject pullAnim;
    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private GameObject alreadyWon;
    [SerializeField] private float deathMagnitude = 2f;
    [SerializeField] private float pauseDuration = 2f;
    [SerializeField] private Rigidbody[] ragdollComponents;
    [SerializeField] private AudioSource ballAudio;
    [SerializeField] private AudioSource deathAudioSource;

    private float stamina = 1f;
    private bool recoverStamina = false;
    private float forceToUse = 0f;
    private float ballForceToUse = 0f;

    private Vector3 initialPosition;
    private Vector3 ballPosition;
    private bool active = false;

    private List<Vector3> rotations = new List<Vector3>();
    private List<Vector3> positions = new List<Vector3>();

    [ContextMenu("ClearPlayerprefs")]
    public void ResetPlayerprefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Awake()
    {
        Game.onBeginGameStop += HandleBeginGameStop;
        initialPosition = transform.position;
        ballPosition = ball.transform.position;

        for (int i = 0; i < ragdollComponents.Length; i++)
        {
            rotations.Add(ragdollComponents[i].transform.rotation.eulerAngles);
            positions.Add(ragdollComponents[i].transform.position);
            ragdollComponents[i].isKinematic = true;
        }

        if (PlayerPrefs.GetInt("WON", 0) == 1)
        {
            alreadyWon.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Game.onBeginGameStop -= HandleBeginGameStop;

    }

    private void HandleBeginGameStop()
    {
        if (!isStopping)
        {
            StartCoroutine(DoStopGame());
            deathAudioSource.Play();
        }
    }

    private bool isStopping = false;

    private IEnumerator DoStopGame()
    {

        isStopping = true;
        pullAnim.SetActive(false);
        pushAnim.SetActive(true);
        for (int i = 0; i < ragdollComponents.Length; i++)
        {
            ragdollComponents[i].isKinematic = false;
        }

        yield return new WaitForSeconds(pauseDuration);

        transform.position = initialPosition;
        ball.transform.position = ballPosition;

        for (int i = 0; i < ragdollComponents.Length; i++)
        {
            ragdollComponents[i].isKinematic = true;
            ragdollComponents[i].transform.rotation = Quaternion.Euler(rotations[i]);
            ragdollComponents[i].transform.position = positions[i];
        }

        Game.TriggerGameStop();

    }

    private void Update()
    {
        if (Game.gameActive)
        {
            active = true;
            ball.isKinematic = false;
            staminaSlider.gameObject.SetActive(stamina < 1f);
            staminaSlider.value = stamina;
            forceToUse = force;
            ballForceToUse = ballPushForce;
            if (stamina <= 0f)
            {
                forceToUse = staminaLostForce;
                ballForceToUse = staminaLostBallPushForce;
            }
            if (Input.GetKey((KeyCode.A)))
            {
                recoverStamina = false;
                pullAnim.SetActive(true);
                pushAnim.SetActive(false);
                rb2d.AddForce(Vector2.right * forceToUse * Time.deltaTime);
                ball.AddForce(Vector2.right * ballForceToUse * Time.deltaTime);
            }
            else if (Input.GetKey((KeyCode.D)) && inContact)
            {
                recoverStamina = true;
                pullAnim.SetActive(false);
                pushAnim.SetActive(true);
                rb2d.AddForce(Vector2.left * forceToUse * Time.deltaTime);
                ball.AddForce(Vector2.left * ballForceToUse * Time.deltaTime);
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
        else if (!recoverStamina && stamina > 0f)
        {
            stamina -= staminaLossRate * Time.fixedDeltaTime;
            if (stamina < 0f)
            {
                stamina = 0f;
            }
        }
    }

    private bool inContact;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Ball")
        {
            inContact = true;
            Debug.LogError(other.relativeVelocity.magnitude);
            if (other.relativeVelocity.magnitude > deathMagnitude)
            {
                Game.TriggerBeginGameStop();

            }
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Ball")
        {
            inContact = false;
        }
    }
}
