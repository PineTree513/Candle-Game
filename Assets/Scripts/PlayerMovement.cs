using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 respawnPosition;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpTime;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float wallFallSpeed;
    private bool breakingFallSpeed;
    [SerializeField] private float jumpQueuedTime;
    [SerializeField] private float kickOff;
    [SerializeField] private float brakeSpeed;
    [SerializeField] private bool testing;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashSpeed;
    private float dashDurationTimer;
    private float dashCooldownTimer;
    private float jumpQueuedTimer;
    private float jumpTimer = 0;
    private bool jumping = false;
    public LayerMask ground;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject playerLight;
    private DialogueData dialogueData;
    
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;

    [Header("Camera - Use Main Camera Tag")]
    public Transform cameraTransform;
    [SerializeField] private Camera cameraScript;
    [SerializeField] private float cameraFollowStrength;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float cameraFOVTarget;
    [SerializeField] private float cameraFOVDefault;
    [SerializeField] private float cameraZoomSpeed;
    [SerializeField] private float fallingFOV;
    [SerializeField] private float fallingModeTime;
    private float fallingModeTimer;


    [Header("UI")]
    public GameObject panel;
    private Image img;
    [SerializeField] private float opacity;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private GameObject pauseMenu;
    public bool isPaused = false;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueBoxText;
    [SerializeField] private int currentLine;
    [SerializeField] private float typewriterTime;
    private float typewriterTimer;

    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem wallBurst;
    [SerializeField] private ParticleSystem wallParticles;

    [HideInInspector] public string scene;

    private GameObject optionalExit;
    public bool transition = false;
    [HideInInspector] public bool entered = false;
    private bool working = false;

    private enum MovementState {idle, running, jumping, falling}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        img = panel.GetComponent<Image>();
        cameraTransform = Camera.main.transform;
        cameraScript = Camera.main.GetComponent<Camera>();

        cameraTarget = transform.Find("Camera Target");
        cameraTransform.position = cameraTarget.position;

        if (!testing)
        {
            transform.position = new Vector3(SaveDataStatic.saveData.playerX, SaveDataStatic.saveData.playerY, 0);
        }
        respawnPosition = transform.position;

        StartCoroutine(nameof(ScreenFade), -1);
    }

    private void FixedUpdate()
    {
        CamController();
    }

    void Update()
    {
        PauseLogic();

        img.color = new Color(0, 0, 0, opacity);

        if (Time.timeSinceLevelLoad > .1 & !transition)
        {
            entered = true;
        }

        if (!transition & !isPaused)
        {
            cameraTarget = transform.Find("Camera Target");
            RecordTime();

            if (optionalExit != null & Input.GetKeyDown(KeyCode.W))
            {
                OptionalExit();
            }

            MoveLogic();
            JumpLogic();
        }

        if (dialogueData != null)
        {
            CutsceneLogic();
        }

        MovementState state;

        if (rb.velocity.x > .1f)
        {
            //spriteRenderer.flipX = true;
            transform.rotation = new Quaternion(0, 0, 0, 1);
            state = MovementState.running;
        }
        else if (rb.velocity.x < -.1f)
        {
            //spriteRenderer.flipX = false;
            transform.rotation = new Quaternion(0, 180, 0, 1);
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (!OnGround())
        {
            state = MovementState.falling;

            if (rb.velocity.y > 0)
            {
                state = MovementState.jumping;
            }
        }

        anim.SetInteger("state", (int)state);

        //DashLogic();
    }

    private void CutsceneLogic()
    {
        Typewriter();

        if (Input.GetButtonDown("Jump"))
        {
            if (dialogueBoxText.text.Length < dialogueData.dialogue[currentLine].Length)
            {
                dialogueBoxText.text = dialogueData.dialogue[currentLine];
            }
            else
            {
                currentLine++;
                dialogueBoxText.text = "";

                if (currentLine == dialogueData.dialogue.Length)
                {
                    ExitCutscene();
                }
            }
        }
    }

    private void Typewriter()
    {
        typewriterTimer -= Time.deltaTime;

        if (dialogueBoxText.text.Length < dialogueData.dialogue[currentLine].Length && typewriterTimer < 0)
        {
            dialogueBoxText.text = dialogueData.dialogue[currentLine].Substring(0, dialogueBoxText.text.Length + 1);
            typewriterTimer = typewriterTime;
        }
    }

    private void ExitCutscene()
    {
        transition = false;
        dialogueData.gameObject.SetActive(false);
        dialogueData = null;
        cameraFOVTarget = cameraFOVDefault;
        dialogueBox.SetActive(false);
    }

    private void DashLogic()
    {
        dashDurationTimer -= Time.deltaTime;

        if (dashCooldownTimer <= 0 & Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashCooldownTimer = dashCooldown;
            dashDurationTimer = dashDuration;
        }
        
        if (dashDurationTimer > 0)
        {
            rb.velocity = new Vector2(-1 * dashSpeed, 0);

            if (spriteRenderer.flipX)
            {
                rb.velocity = new Vector2(dashSpeed, 0);
            }
        }
        else
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void PauseLogic()
    {
        pauseMenu.SetActive(isPaused);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        Time.timeScale = 1f;
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
    }

    private void CamController()
    {
        if (breakingFallSpeed)
        {
            breakingFallSpeed = false;
            fallingModeTimer += Time.fixedDeltaTime;
        }
        else
        {
            fallingModeTimer = 0;
        }

        if (fallingModeTimer > fallingModeTime)
        {
            cameraTarget = transform.Find("Falling Camera Target");
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(cameraTarget.position.x, cameraTarget.position.y, -10), cameraFollowStrength * Time.deltaTime);
            cameraScript.fieldOfView = Mathf.Lerp(cameraScript.fieldOfView, fallingFOV, cameraZoomSpeed * Time.deltaTime);
        }
        else
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(cameraTarget.position.x, cameraTarget.position.y, -10), cameraFollowStrength * Time.deltaTime);
            //cameraScript.orthographicSize = Mathf.Lerp(cameraScript.orthographicSize, cameraSizeTarget, cameraZoomSpeed * Time.deltaTime);
            cameraScript.fieldOfView = Mathf.Lerp(cameraScript.fieldOfView, cameraFOVTarget, cameraZoomSpeed * Time.deltaTime);
        }
    }

    private void MoveLogic()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(maxSpeed * xInput) | xInput * rb.velocity.x < 0)
        {
            rb.AddForce(Vector2.right * xInput * acceleration * Time.deltaTime);
        }

        if (xInput == 0)
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, new Vector2(0, rb.velocity.y), brakeSpeed * Time.deltaTime);
        }
    }

    private void JumpLogic()
    {
        var em = dust.emission;

        if (Input.GetButtonDown("Jump"))
        {
            jumpQueuedTimer = jumpQueuedTime;
        }

        if (jumpQueuedTimer > 0)
        {
            if (OnGround() | WallSide() != 0)
            {
                if (!OnGround())
                {
                    rb.velocity = new Vector2(kickOff * WallSide(), rb.velocity.y);
                    wallBurst.Play();
                }

                jumpQueuedTimer = 0;
                jumping = true;
                jumpTimer = jumpTime;
                em.rateOverTime = 30;
            }
            else
            {
                jumpQueuedTimer -= Time.deltaTime;
            }
        }

        if (Input.GetButton("Jump") && jumping && jumpTimer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTimer -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpQueuedTimer = 0;
            jumping = false;
            em.rateOverTime = 0;
        }

        if (jumpTimer <= 0)
        {
            jumping = false;
            em.rateOverTime = 0;
        }

        var wallEmission = wallParticles.emission;
        wallEmission.rateOverTime = 0;

        if (WallSide() != 0 & !Input.GetKey(KeyCode.S))
        {
            if (!OnGround())
            {
                wallEmission.rateOverTime = 8;
            }

            if (rb.velocity.y < wallFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, wallFallSpeed);
            }
        } 
        else if (rb.velocity.y < fallSpeed)
        {
            breakingFallSpeed = true;
            rb.velocity = new Vector2(rb.velocity.x, fallSpeed);
        }
    }

    private int WallSide()
    {
        if (TouchingWallLeft())
        {
            return -1;
        }

        if (TouchingWallRight())
        {
            return 1;
        }

        return 0;
    }

    private bool TouchingWallLeft()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, .1f, ground);
    }

    private bool TouchingWallRight()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, .1f, ground);
    }

    private bool OnGround()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ground);
    }

    private void OptionalExit()
    {
        var transitionData = optionalExit.GetComponent<TransitionData>();

        transition = true;
        scene = transitionData.targetScene;
        SaveDataStatic.saveData.playerX = transitionData.playerX;
        SaveDataStatic.saveData.playerY = transitionData.playerY;
        SaveDataStatic.saveData.scene = transitionData.targetScene;
        StopCoroutine("ScreenFade");
        StartCoroutine("ScreenFade", 1);
        Invoke("MoveScene", 1.5f);
    }

    private void PlayerDieAndRespawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = respawnPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TurnOffLight"))
        {
            playerLight.SetActive(false);
        }

        if (collision.gameObject.CompareTag("TurnOnLight"))
        {
            playerLight.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Hazard"))
        {
            PlayerDieAndRespawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cutscene"))
        {
            transition = true;
            currentLine = 0;
            dialogueData = collision.gameObject.GetComponent<DialogueData>();
            cameraTarget = collision.transform.GetChild(0);
            cameraFOVTarget = dialogueData.cameraFOV;
            dialogueBox.SetActive(true);
            dialogueBoxText.text = "";
            collision.enabled = false;
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            respawnPosition = collision.transform.position;
        }

        if (collision.gameObject.CompareTag("Save") && !working)
        {
            working = true;
            SaveGame();
        }

        if (collision.gameObject.CompareTag("Exit"))
        {
            var em = dust.emission;
            em.rateOverTime = 0;

            var transitionData = collision.GetComponent<TransitionData>();

            transition = true;
            scene = transitionData.targetScene;
            SaveDataStatic.saveData.playerX = transitionData.playerX;
            SaveDataStatic.saveData.playerY = transitionData.playerY;
            SaveDataStatic.saveData.scene = transitionData.targetScene;
            
            SaveGame();

            StopCoroutine("ScreenFade");
            StartCoroutine("ScreenFade", 1);
            Invoke("MoveScene", 1.5f);
        }

        if (collision.gameObject.CompareTag("Entrance"))
        {
            /*
            AllCamsOff();
            GameObject transitionCam = collision.gameObject.transform.GetChild(0).gameObject;
            transitionCam.SetActive(true);
            */
            //Vector2 transitionCam = collision.gameObject.transform.GetChild(0).position;
            //cam.position = new Vector3(transitionCam.x, transitionCam.y, -10);
            cameraTransform.position = new Vector3(collision.gameObject.transform.GetChild(0).position.x , transform.position.y + 1, -10);
            transition = true;
        }

        if (collision.gameObject.CompareTag("Exit (Optional)"))
        {
            optionalExit = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Save"))
        {
            transition = false;
        }

        if (collision.gameObject.CompareTag("Exit") || collision.gameObject.CompareTag("Entrance"))
        {
            var transitionData = collision.GetComponent<TransitionData>();
            rb.velocity = new Vector2(maxSpeed * transitionData.direction, rb.velocity.y);
        }

        if (collision.gameObject.CompareTag("Exit (Optional)"))
        {
            if (Input.GetKey(KeyCode.W) & !transition)
            {
                var transitionData = collision.GetComponent<TransitionData>();

                transition = true;
                scene = transitionData.targetScene;
                SaveDataStatic.saveData.playerX = transitionData.playerX;
                SaveDataStatic.saveData.playerY = transitionData.playerY;
                SaveDataStatic.saveData.scene = transitionData.targetScene;
                StopCoroutine("ScreenFade");
                StartCoroutine("ScreenFade", 1);
                Invoke("MoveScene", 2f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Entrance"))
        {
            /*
            AllCamsOff();
            cam.gameObject.SetActive(true);
            */
            transition = false;
            entered = true;
        }

        if (collision.gameObject.CompareTag("Exit (Optional)"))
        {
            optionalExit = null;
        }
    }

    private void MoveScene()
    {
        SceneManager.LoadScene(scene);
    }

    public void SaveGame()
    {
        /*SaveData saveData = new SaveData();
        saveData.playerX = gameObject.transform.position.x;
        saveData.playerY = gameObject.transform.position.y;
        saveData.scene = SceneManager.GetActiveScene().name;
        saveData.playTime = SaveDataStatic.saveData.playTime;*/
        SerializationManager.Save(MenuDataStatic.targetSaveName, SaveDataStatic.saveData);
        working = false;
    }

    private void RecordTime()
    {
        SaveDataStatic.saveData.playTime += Time.deltaTime;
    }

    private void AllCamsOff()
    {
        foreach (var item in Camera.allCameras)
        {
            item.gameObject.SetActive(false);
        }
    }

    IEnumerator ScreenFade(int darken)
    {
        if (darken == -1)
        {
            while (opacity > 0)
            {
                opacity += fadeSpeed * darken * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (opacity < 1)
            {
                opacity += fadeSpeed * darken * Time.deltaTime;
                yield return null;
            }
        }

        yield break;
    }
}