using UnityEngine;

public class MovimentoEmPlataforma : MonoBehaviour
{
    public float velocidade = 10;

    [Header("Vari�veis do Pulo")]
    public float puloMin = 1;
    public float puloMax = 50;
    public float tempoSegurandoPuloMin = 0;
    public float tempoSegurandoPuloMax = 2;
    float tempoSegurandoPulo;

    [Header("Detec��o do Ch�o")]
    public Transform posicaoDoPe;
    public float raioDoPe;
    public LayerMask layerDoChao;

    [Header("Detec��o da Parede")]
    public Transform posEsquerda;
    public Transform posDireita;
    public float raioDeParede;

    [Header("Pulo na Parede")]
    public Vector2 puloParedeSemApertar;
    public Vector2 puloParedeApertando;
    public float cooldownParede;
    float contagemCooldownParede;

    [Header("Dash")]
    public Vector2 forcaDoDash;


    bool estaNaParede = false;


    Rigidbody2D fisica;

    bool noChao = false;

    Animator anim;
    SpriteRenderer spriteRenderer;

    //S� para termos uma vari�vel com o nome mais bonito
    //Usamos Lambda Expression (=>) que nos permite escrever
    //L�gica enquanto declaramos uma vari�vel.
    //Basicamente disse que estamos olhando para a direita
    //Se o sprite est� flipado.
    bool olhandoParaDireita => spriteRenderer.flipX;

    // Start is called before the first frame update
    void Start()
    {
        fisica = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Descobrir se est� na parede
        Collider2D paredeAEsquerda =
            Physics2D.OverlapCircle(
                posEsquerda.position,
                raioDeParede,
                layerDoChao
                );
        Collider2D paredeADireita =
            Physics2D.OverlapCircle(
                posDireita.position,
                raioDeParede,
                layerDoChao
                );

        //Essa declara��o de vari�vel vai l� em cima, l� na
        //Classe!
        //bool olhandoParaDireita => spriteRenderer.flipX;

        if ( !noChao && (
            (paredeAEsquerda != null && !olhandoParaDireita)
            || 
            (paredeADireita != null && olhandoParaDireita))
          )
        {
            estaNaParede = true;
        }
        else
        {
            estaNaParede = false;
        }

        anim.SetBool("Parade", estaNaParede);

        //Wall slide. Se estamos caindo mas estamos com uma parede
        //Perto, vamos diminuir a gravidade.
        //Se n�o, gravidade � normal
        if(fisica.velocity.y < 0 && estaNaParede)
        {
            fisica.gravityScale = 0.2f;
        }
        else
        {
            fisica.gravityScale = 1;
        }



        //Descobrir se est� no ch�o
        Collider2D circuloBatemEm = 
            Physics2D.OverlapCircle(
                posicaoDoPe.position,
                raioDoPe,
                layerDoChao
                );

        if(circuloBatemEm == null)
        {
            noChao = false;
        }
        else
        {
            noChao = true;
        }



        //Pega o movimento Horizontal do player (tecla A, D ,
        //seta esquerda ou direita). S�o valores de -1 at� 1.
        //-1 esquerda, 1 direita, e se n�o apertei nada � 0
        float MovimentoHorizontal = Input.GetAxisRaw("Horizontal");

        if (MovimentoHorizontal > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (MovimentoHorizontal < 0)
        {
            spriteRenderer.flipX = false;
        }

        if (MovimentoHorizontal != 0)
        {
            anim.SetBool("Correndo", true);
        }
        else
        {
            anim.SetBool("Correndo", false);
        }

        //! � o contr�rio do valor. Se no ch�o for true, vai botar false
        anim.SetBool("Pulando", !noChao);



        if(contagemCooldownParede > 0)
        {
            contagemCooldownParede -= Time.deltaTime;
            return;
        }

        //Pular se est� na parede
        if (estaNaParede && Input.GetKeyDown(KeyCode.Space))
        {
            contagemCooldownParede = cooldownParede;
            fisica.velocity = Vector2.zero;

            Vector2 forcaAplicada = puloParedeSemApertar;
            if(MovimentoHorizontal != 0)
            {
                forcaAplicada = puloParedeApertando;
            }

            if (olhandoParaDireita)
            {
                forcaAplicada.x *= -1;
            }

            fisica.AddForce(forcaAplicada,
                ForceMode2D.Impulse);
            spriteRenderer.flipX = !spriteRenderer.flipX;

            return;
        }

        //Fazer o dash!
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            contagemCooldownParede = cooldownParede;
            fisica.velocity = Vector2.zero;
            Vector2 forcaAplicada = forcaDoDash;
            if (!olhandoParaDireita)
            {
                forcaAplicada.x *= -1;
            }

            float MovimentoVertical = 
                Input.GetAxisRaw("Vertical");
            forcaAplicada.y *= MovimentoVertical;

            fisica.AddForce(forcaAplicada,
                ForceMode2D.Impulse);
            return;
        }



        //Aplicamos o movimento na velocidade da f�sica. Como
        //o movimento acima � no m�ximo 1, pode ser lento.
        //Ent�o multiplicamos pela velocidade
        //A velocidade em Y a gente n�o quer mudar. Quer manter
        //A do rigidbody. Ent�o repitimos fisica.velocity.y

        fisica.velocity = new Vector2(
            MovimentoHorizontal * velocidade,
            fisica.velocity.y);

        CalculosDePulo();

    }

    void CalculosDePulo()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Quantidade de tempo que demora entre um Update
            //E outro. O tempo em cada frame.
            tempoSegurandoPulo += Time.deltaTime;
        }

        //Ao soltarmos espa�o, se estamos no ch�o...
        if (Input.GetKeyUp(KeyCode.Space) && noChao)
        {
            Debug.Log(tempoSegurandoPulo);

            //Mathf � uma classe do C# que tem v�rias fun��es
            //Matem�ticas j� programadas
            //InverseLerpe a gente passa um n�mero inicial
            //e um n�mero final, e um n�mero no meio do caminho.
            //Ele nos devolve a porcentagem que o n�mero no meio do caminho est�
            //Exemplo: Inicial: 0, Final: 2 ---Meio: 1.-- 
            //Me retornaria: 0.5, que � 50% entre 0 e 2.
            float porcentagemDeTempo =
            Mathf.InverseLerp(tempoSegurandoPuloMin,
                tempoSegurandoPuloMax, tempoSegurandoPulo);

            Debug.Log("Porcentagem �: " + porcentagemDeTempo);

            //Lerp � parecido, mas eu passo um n�mero inicial,
            //Um final, e a porcentagem entre os dois que eu quero.
            //Ent�o, se o inicial � 0, o final � 50
            //e minha porcentagem � 0.5 (50%)...
            //Daria 25, ou seja, metade de 50.
            float forcaDoPulo =
                Mathf.Lerp(puloMin, puloMax, porcentagemDeTempo);

            Debug.Log("For�a do Pulo ser�: " + forcaDoPulo);

            tempoSegurandoPulo = 0;

            Vector2 novaVelocidadeDePulo =
                new Vector2(fisica.velocity.x, forcaDoPulo);

            fisica.velocity = novaVelocidadeDePulo;


            /*
             * Comentamos essa parte para fazer um pulo mais complexo
             * baseado em Celeste
             * 
            //Vamos criar um vetor, que tem X e Y para representar
            //a for�a. Em X, nada. N�o queremos empurrar pra frente
            //Em Y, a for�a do pulo!
            Vector2 vetorDoPulo = new Vector2(0, puloMax);
            fisica.AddForce(vetorDoPulo);
            */
        }
    }

    //DrawGizmos � uma fun��o que desenha algo para nos auxiliar
    //Apenas na janela Scene. N�o vai pro jogo. S� dentro da Unity.
    //Nesse caso, escolhemos a cor vermelha
    //e desenhamos a borda de uma esfera na posi��o
    //e raio que quisermos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoDoPe.position, raioDoPe);

        Gizmos.DrawWireSphere(posEsquerda.position, raioDeParede);
        Gizmos.DrawWireSphere(posDireita.position, raioDeParede);
    }

}
