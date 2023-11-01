using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    public Rigidbody objeto;
    public float velocidadeHorizontal = 3f;
    public float velocidadeVertical = 3f;
    public float velocidadeCorrendo = 6f;
    public float forcaDePulo = 5f;
    private bool noChao = true;
    private int life = 100;
    private bool isTakingDamage = false;
    private Animator animator;
    private bool isAttacking = false;
    public Text lifeText;
    public Text bonusTimer;
    public Text timerText;
    private float timerGame;
    private int damageTaken = 5;

    public Vector3 scaleIncrease = new Vector3(2.0f, 2.0f, 2.0f);
    private Vector3 originalScale;
    private bool isScaled = false;
    private float timer = 0f;
    private bool isSlow = false;
    private float slowTimer = 0f;
    private int pontuacao;

    public AudioSource somDeDestruição; 

    private Collision collidingEnemy;

    public generateEnemy enemyGenerator;
    public Transform transf; public Transform camera_transf;
    Vector2 rotacaoMouse; public int sensibilidade = 100;

    void Start()
    {
        objeto = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
        if (PlayerPrefs.GetFloat("Timer") != null)
        {
            timerGame = PlayerPrefs.GetFloat("Timer");
        }
        else
        {
            timerGame = 0;
        }
        if (PlayerPrefs.GetFloat("Timer") != null)
        {
            timerGame = PlayerPrefs.GetFloat("Timer");
        }
        else
        {
            timerGame = 0;
        }
        if (PlayerPrefs.GetFloat("Life") != null)
        {
            life = PlayerPrefs.GetInt("Life");
        }
        else
        {
            life = 0;
        }
        if (PlayerPrefs.GetFloat("Score") != null)
        {
            pontuacao = PlayerPrefs.GetInt("Score");
        }
        else
        {
            pontuacao = 0;
        }
    }

    void Update()
    {
        timerGame += Time.deltaTime;
        timerText.text = timerGame.ToString("0.0") + "s";
        Camera();
        Movimento();
        if (Input.GetButtonDown("Jump") && noChao)
        {
            Pular();
        }

        if (Input.GetMouseButtonDown(0)  && noChao) //&& isAttacking
        {
            Atacar();
        }

        bool correndo = Input.GetKey("left shift") || Input.GetKey("right shift");
        objeto.GetComponent<Animator>().SetBool("run", correndo);

        if (this.collidingEnemy != null && !isTakingDamage)
        {
            isTakingDamage = true;
            Invoke("setDamage", 0.5f);
        }
        lifeText.text = life.ToString();

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            bonusTimer.text = timer.ToString("0.0");
            if (timer <= 0)
            {
                isScaled = false;
                transform.localScale = originalScale;
                timer = 0;
                damageTaken = 5;
                bonusTimer.text = "";
            }
        }
        if (slowTimer > 0)
        {
            slowTimer -= Time.deltaTime;
            bonusTimer.text = slowTimer.ToString("0.0");
            if (slowTimer <= 0)
            {
                isSlow = false;
                slowTimer = 0;
                bonusTimer.text = "";
                velocidadeHorizontal = 3f;
                velocidadeVertical = 3f;
                velocidadeCorrendo = 6f;
                if (enemyGenerator != null)
                {
                    enemyGenerator.stopGenerate = false;
                }
            }
        }
    }

    private void Camera()
    {
        Vector2 controleMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotacaoMouse = new Vector2(rotacaoMouse.x + controleMouse.x * sensibilidade *
        Time.deltaTime, rotacaoMouse.y + controleMouse.y * sensibilidade * Time.deltaTime);

        transf.eulerAngles = new Vector3(transf.eulerAngles.x, rotacaoMouse.x, transf.eulerAngles.z);
        rotacaoMouse.y = Mathf.Clamp(rotacaoMouse.y, -80, 80);
        
        camera_transf.localEulerAngles = new Vector3(-rotacaoMouse.y,
        camera_transf.localEulerAngles.y, camera_transf.localEulerAngles.z);

    }

    private void setDamage()
    {
            this.life -= damageTaken;
            isTakingDamage = false;
    }

    void Movimento()
    {
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        float movimentoVertical = Input.GetAxis("Vertical");
        float y = objeto.velocity.y;

        // Obtenha a direção da câmera
        Vector3 cameraForward = camera_transf.forward;
        cameraForward.y = 0;  // Certifique-se de que a direção não tenha componente vertical

        // Normalize a direção do movimento
        Vector3 movimento = (cameraForward * movimentoVertical + camera_transf.right * movimentoHorizontal).normalized;

        float velocidade = Input.GetKey("left shift") || Input.GetKey("right shift") ? velocidadeCorrendo : velocidadeHorizontal;
        objeto.velocity = new Vector3(movimento.x * velocidade, y, movimento.z * velocidadeVertical);

        if (movimento.magnitude > 0)
        {
            objeto.GetComponent<Animator>().SetBool("idle", false);
            objeto.GetComponent<Animator>().SetBool("walk", true);

            Quaternion novaRotacao = Quaternion.LookRotation(movimento);
            objeto.transform.rotation = Quaternion.Slerp(objeto.transform.rotation, novaRotacao, Time.deltaTime * 5f);
        }
        else
        {
            objeto.GetComponent<Animator>().SetBool("idle", true);
            objeto.GetComponent<Animator>().SetBool("walk", false);
        }
    }

    void Pular()
    {
        objeto.AddForce(Vector3.up * forcaDePulo, ForceMode.Impulse);
        noChao = false;
        objeto.GetComponent<Animator>().SetBool("jump", true);
    }

    void Atacar()
    {
        isAttacking = true;
        objeto.GetComponent<Animator>().SetBool("hit", true);
        if (this.collidingEnemy != null)
        {
            this.somDeDestruição.Play();
            Destroy(collidingEnemy.gameObject);
            this.collidingEnemy = null;            
        }
        Invoke("FinalizarAtaque", 1.0f);
    }

    public void FinalizarAtaque()
    {
        isAttacking = false;
        objeto.GetComponent<Animator>().SetBool("hit", false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bomba"))
        {
            collidingEnemy = collision;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("save1"))
        {
            PlayerPrefs.SetFloat("Timer", timerGame);
            PlayerPrefs.SetInt("Life", life);
            PlayerPrefs.SetInt("Score", pontuacao);

            PlayerPrefs.Save();

            // Use SceneManager para fazer a transição para a cena desejada
            SceneManager.LoadScene("inicio 1");
        }
        if (collision.gameObject.CompareTag("Bomba"))
        {
            collidingEnemy = collision;
        }
        if (collision.gameObject.CompareTag("tiro"))
        {
            this.life -= 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Bonus") && !isScaled)
        {
            isScaled = true;
            transform.localScale = scaleIncrease;

            Destroy(collision.gameObject);
            damageTaken = 1;

            timer = 10f;
        }
        if (collision.gameObject.CompareTag("slow") && !isSlow)
        {
            Destroy(collision.gameObject);
            isSlow = true;
            velocidadeHorizontal = 1f;
            velocidadeVertical = 1f;
            velocidadeCorrendo = 2f;
            slowTimer = 10f;
            if (enemyGenerator != null)
            {
                enemyGenerator.stopGenerate = true;
            }
        }
        else
        {
            noChao = true;
            objeto.GetComponent<Animator>().SetBool("jump", false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collidingEnemy = null;
    }
}
