using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float veloc;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float entradaHorizontal;
    public float entradaVertical;
    public GameObject pfMissel;
    public GameObject DisparoTriplo;
    public bool possoDarDisparoTriplo = false;
    public float tempoDeDisparo = 0.3f;
    public float podeDisparar = 0.0f;
    private bool isPaused = false;
    private bool methodActivated = false;
    public int vidas = 5;
    public float rotationSpeed = 0.5f;
    private Quaternion initialRotation;
    private GerenciadorIU _iuGerenciador;
    private bool isDashing = false;
    private Rigidbody2D rb;
    public GameObject _dashingPlayer;
    public bool possoUsarDash = false;

    public void DanoAoPlayer()
    {
        vidas--;
        _iuGerenciador.AtualizaVidas(vidas);

        if (vidas < 1) 
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Método Start de" + this.name);
        veloc = 3.75f;
        transform.position = new Vector3(-7.6f,-0.36f,0);
        initialRotation = transform.rotation;
        _iuGerenciador = GameObject.Find("Canvas").GetComponent<GerenciadorIU>();
        if (_iuGerenciador != null ) 
        {
            _iuGerenciador.AtualizaVidas(vidas);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (isDashing == true)
        {
            // Ative o GameObject do sprite do Dash
            _dashingPlayer.SetActive(true);
        }
        else if (isDashing == false) 
        {
            // Desative o GameObject do sprite do Dash
            _dashingPlayer.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.L) && !isDashing)
        {
            
            StartCoroutine(Dash1());
        }

        if (Input.GetKeyDown(KeyCode.J) && !isDashing)
        {
            StartCoroutine(Dash2());
        }

        if (Input.GetKeyDown(KeyCode.K) && !isDashing)
        {
            StartCoroutine(Dash3());
        }

        if (Input.GetKeyDown(KeyCode.I) && !isDashing)
        {
            StartCoroutine(Dash4());
        }

        this.Movimento(); 
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
        Disparo();
            // Lógica para o jogador enquanto o jogo não estiver pausado
            // Por exemplo, movimento, ataque, etc.


            if (Input.GetKeyDown(KeyCode.Escape))
            { // Lógica para pausar o jogo
                if (isPaused == true) // Você pode escolher uma tecla para pausar
                {
                    PauseGame();

                }
                else
                {
                    ResumeGame();
                    // Coloque aqui a lógica para despausar o jogo, como restaurar o tempo   
                }
                isPaused = !isPaused; // Inverte o estado de pausa
            }

if (methodActivated)
        {
            // Coloque aqui o código que deseja executar quando o método estiver ativado.
            Debug.Log("Método ativado!");
        }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // Rotaciona o jogador para cima
                transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
                
            }
            // Verifica se o jogador pressionou a tecla "S" para girar para baixo
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                // Rotaciona o jogador para baixo
                transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Se nenhuma tecla estiver sendo pressionada, retorna à rotação inicial
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * 2.0f);
            }

        }
 }
    void PauseGame()
    {
        // Pausa o jogo definindo o Time.timeScale como 0 (tempo em pausa).
        Time.timeScale = 0;
        isPaused = true;

        // Execute aqui qualquer outra ação de pausa, como exibir um menu de pausa.
        Debug.Log("Jogo pausado");
    }

    void ResumeGame()
    {
        // Despausa o jogo, restaurando o Time.timeScale para 1 (tempo normal).
        Time.timeScale = 1;
        isPaused = false;

        // Execute aqui qualquer outra ação de despausa, como fechar o menu de pausa.
        Debug.Log("Jogo retomado");
    }
    private void Disparo()
    {
        if ( Input.GetKeyDown(KeyCode.Space)){

        if( Time.time > podeDisparar )
        {
            if(possoDarDisparoTriplo == true)
            {
            Instantiate(DisparoTriplo,transform.position + new Vector3(-0.03f,0,0),Quaternion.identity);
            }
            else
            {
                Instantiate(pfMissel,transform.position + new Vector3 (-0.03f,0,0),Quaternion.identity);
            }
            podeDisparar = Time.time + tempoDeDisparo ;
            }
        }
    }
    private void Movimento(){
        float entradaHorizontal = Input.GetAxis("Horizontal");
         transform.Translate(Vector3.right * entradaHorizontal * Time.deltaTime*veloc);
        if (transform.position.y > 5.45f){
            transform.position = new Vector3(transform.position.x,5.45f,0);
        } else if(transform.position.x < -8.60f){
            transform.position = new Vector3(-8.60f,transform.position.y,0);
        }     
        float entradaVertical = Input.GetAxis("Vertical");
         transform.Translate(Vector3.up * entradaVertical * Time.deltaTime*veloc);
        if (transform.position.x > 10.35f){
            transform.position = new Vector3(10.35f,transform.position.y,0);
        } else if(transform.position.y < -5.5f){
            transform.position = new Vector3(transform.position.x,-5.5f,0);
        }

        float rotationAmount = entradaVertical * rotationSpeed * Time.deltaTime;
        transform.Rotate( 0, 0, rotationAmount);
    }

    public void LigarPUDisparoTriplo(){
        possoDarDisparoTriplo = true;
        StartCoroutine(DisparoTriploRotina());
    }

    public IEnumerator DisparoTriploRotina (){ 
        yield return new WaitForSeconds(7.0f);
        possoDarDisparoTriplo = false;
    }

    IEnumerator Dash1()
    {
        isDashing = true;
        
        Vector2 dashDirection = Vector3.right * Mathf.Sign(rb.velocity.x); // Dash na direção em que o jogador está se movendo
        rb.velocity = dashDirection * dashSpeed;


        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero; // Parar o jogador após o dash
        isDashing = false;
    }

    IEnumerator Dash2()
    {
        isDashing = true;

        Vector2 dashDirection = Vector3.left * Mathf.Sign(rb.velocity.x); // Dash na direção em que o jogador está se movendo
        rb.velocity = dashDirection * dashSpeed;


        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero; // Parar o jogador após o dash
        isDashing = false;
    }

    IEnumerator Dash3()
    {
        isDashing = true;

        Vector2 dashDirection = Vector3.down * Mathf.Sign(rb.velocity.y); // Dash na direção em que o jogador está se movendo
        rb.velocity = dashDirection * dashSpeed;


        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero; // Parar o jogador após o dash
        isDashing = false;
    }

    IEnumerator Dash4()
    {
        isDashing = true;

        Vector2 dashDirection = Vector3.up * Mathf.Sign(rb.velocity.y); // Dash na direção em que o jogador está se movendo
        rb.velocity = dashDirection * dashSpeed;


        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero; // Parar o jogador após o dash
        isDashing = false;
    }
} 
