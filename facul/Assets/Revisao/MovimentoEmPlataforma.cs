using UnityEngine;

public class MovimentoEmPlataforma : MonoBehaviour
{
    public float velocidade = 10;

    [Header("Variáveis do Pulo")]
    public float puloMin = 1;
    public float puloMax = 50;
    public float tempoSegurandoPuloMin = 0;
    public float tempoSegurandoPuloMax = 2;
    float tempoSegurandoPulo;

    [Header("Detecção do Chão")]
    public Transform posicaoDoPe;
    public float raioDoPe;
    public LayerMask layerDoChao;

    [Header("Detecção da Parede")]
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

    //Só para termos uma variável com o nome mais bonito
    //Usamos Lambda Expression (=>) que nos permite escrever
    //Lógica enquanto declaramos uma variável.
    //Basicamente disse que estamos olhando para a direita
    //Se o sprite está flipado.
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
        //Descobrir se está na parede
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

        //Essa declaração de variável vai lá em cima, lá na
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
        //Se não, gravidade é normal
        if(fisica.velocity.y < 0 && estaNaParede)
        {
            fisica.gravityScale = 0.2f;
        }
        else
        {
            fisica.gravityScale = 1;
        }



        //Descobrir se está no chão
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
        //seta esquerda ou direita). São valores de -1 até 1.
        //-1 esquerda, 1 direita, e se não apertei nada é 0
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

        //! é o contrário do valor. Se no chão for true, vai botar false
        anim.SetBool("Pulando", !noChao);



        if(contagemCooldownParede > 0)
        {
            contagemCooldownParede -= Time.deltaTime;
            return;
        }

        //Pular se está na parede
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



        //Aplicamos o movimento na velocidade da física. Como
        //o movimento acima é no máximo 1, pode ser lento.
        //Então multiplicamos pela velocidade
        //A velocidade em Y a gente não quer mudar. Quer manter
        //A do rigidbody. Então repitimos fisica.velocity.y

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

        //Ao soltarmos espaço, se estamos no chão...
        if (Input.GetKeyUp(KeyCode.Space) && noChao)
        {
            Debug.Log(tempoSegurandoPulo);

            //Mathf é uma classe do C# que tem várias funções
            //Matemáticas já programadas
            //InverseLerpe a gente passa um número inicial
            //e um número final, e um número no meio do caminho.
            //Ele nos devolve a porcentagem que o número no meio do caminho está
            //Exemplo: Inicial: 0, Final: 2 ---Meio: 1.-- 
            //Me retornaria: 0.5, que é 50% entre 0 e 2.
            float porcentagemDeTempo =
            Mathf.InverseLerp(tempoSegurandoPuloMin,
                tempoSegurandoPuloMax, tempoSegurandoPulo);

            Debug.Log("Porcentagem é: " + porcentagemDeTempo);

            //Lerp é parecido, mas eu passo um número inicial,
            //Um final, e a porcentagem entre os dois que eu quero.
            //Então, se o inicial é 0, o final é 50
            //e minha porcentagem é 0.5 (50%)...
            //Daria 25, ou seja, metade de 50.
            float forcaDoPulo =
                Mathf.Lerp(puloMin, puloMax, porcentagemDeTempo);

            Debug.Log("Força do Pulo será: " + forcaDoPulo);

            tempoSegurandoPulo = 0;

            Vector2 novaVelocidadeDePulo =
                new Vector2(fisica.velocity.x, forcaDoPulo);

            fisica.velocity = novaVelocidadeDePulo;


            /*
             * Comentamos essa parte para fazer um pulo mais complexo
             * baseado em Celeste
             * 
            //Vamos criar um vetor, que tem X e Y para representar
            //a força. Em X, nada. Não queremos empurrar pra frente
            //Em Y, a força do pulo!
            Vector2 vetorDoPulo = new Vector2(0, puloMax);
            fisica.AddForce(vetorDoPulo);
            */
        }
    }

    //DrawGizmos é uma função que desenha algo para nos auxiliar
    //Apenas na janela Scene. Não vai pro jogo. Só dentro da Unity.
    //Nesse caso, escolhemos a cor vermelha
    //e desenhamos a borda de uma esfera na posição
    //e raio que quisermos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posicaoDoPe.position, raioDoPe);

        Gizmos.DrawWireSphere(posEsquerda.position, raioDeParede);
        Gizmos.DrawWireSphere(posDireita.position, raioDeParede);
    }

}
