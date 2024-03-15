using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    #region variaves
    [Header("UI")]
    [SerializeField] GameObject[] Intaface;

    [Header("movimentação")]
    [SerializeField] private float velocidade;

    private SpriteRenderer _renderer;
    private Rigidbody2D _rigidbody;
    Animator _animator;

    [Header("Tempo")]
    [SerializeField] float _timeInteraction;

    [Header("se esta interagindo")]
    public bool interection;

    [Header("Debug")]
    [SerializeField] private State PlayerState; 
   
    float stateTIme; //tempo

   
    
    #endregion

    #region State
    enum State
    {
        parado,
        anadando,
        interecao
    }

    #endregion

    #region start & uptat
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float delta = Time.deltaTime;
        homdleenemyFSM(delta);
        _animator.SetInteger("State", (int)PlayerState);

        direction(moveButton());
    }
    #endregion

    #region FSM
    void homdleenemyFSM(float deltaTIme)
    {
        stateTIme += deltaTIme;

        var newState = TryChangeCurrentState(PlayerState, stateTIme);

        //saber se trocando de estado
        if (newState != PlayerState)
        {
            //troque de estado
            OnStateExit(PlayerState);

            //Trocar pora um novo estado
            PlayerState = newState;
            stateTIme = 0;

            //Entra no novo estado
            OnStateEnter(PlayerState);
        }
        //dar uipdaite para um estado atual
        OnStateUpdete(PlayerState, deltaTIme);
    }

    State TryChangeCurrentState(State State, float time)
    {

        switch (State)
        {
            case State.anadando:
                //tentando ir para o parado
                if (moveButton() == Vector2.zero)
                {
                    return State.parado;
                }
                //tentando ir para a interação
                if (interection || isInterface())
                {
                    return State.interecao;
                }
                break;
            case State.interecao:
                //tentando ir para parado
                if (time >= _timeInteraction && !isInterface())
                {
                    return State.parado;
                }
                break;
            case State.parado:
                if (moveButton() != Vector2.zero)
                {
                    return State.anadando;
                }
                //tentando ir para a interação
                if (interection || isInterface())
                {
                    return State.interecao;
                }
                break;
            default:
                break;
        }
        return State;
    }

    void OnStateExit(State State)
    {

    }

    void OnStateEnter(State State)
    {
     
    }
    void OnStateUpdete(State State, float deltaTIme)
    {
        switch (State)
        {
            case State.interecao:
                _rigidbody.velocity = Vector2.zero;
                break;
            case State.anadando:
                _rigidbody.velocity = moveButton() * velocidade;

                
                if (moveButton().x > 0 && _renderer.flipX || moveButton().x < 0 && !_renderer.flipX)
                    _renderer.flipX = !_renderer.flipX;

                break;
            default:
                break;
        }
    }

    #endregion

    #region sistemas
    Vector2 moveButton()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    bool isInterface()
    {
        int currant = 0;
        for (int i = 0; i < Intaface.Length;i++)
        {
            if (Intaface[i].activeInHierarchy) 
                currant ++;
        }
        return currant > 0 ? true : false;
    }
    void direction(Vector2 velocity)
    {
        if (velocity.x != 0 || velocity.y != 0)
        {
            if (velocity.x != 0 && velocity.y == 0)
            {
                _animator.SetInteger("direction", 2);
                return;
            }
            if (velocity.y != 0 && velocity.x == 0)
            {
                if (velocity.y > 0)
                {
                    _animator.SetInteger("direction", 0);
                    return;
                }
                else
                {
                    _animator.SetInteger("direction", 1);
                    return;
                }
            }

            if (velocity.x > 0 && velocity.y > 0)
            {
                if (velocity.x > velocity.y)
                {
                    _animator.SetInteger("direction", 2);
                    return;
                }
                else if (velocity.x < velocity.y)
                {
                    _animator.SetInteger("direction", 0);
                    return;
                }
                else
                {
                    if (_animator.GetInteger("direction") != 1) return;
                    int ram = Random.Range(0, 1);
                    if (ram == 1) ram = 2;
                    _animator.SetInteger("direction", ram);
                    return;
                }
            }
            else if (velocity.x < 0 && velocity.y < 0)
            {
                if (velocity.x > velocity.y)
                {
                    _animator.SetInteger("direction", 2);
                    return;
                }
                else if (velocity.x < velocity.y)
                {
                    _animator.SetInteger("direction", 1);
                    return;
                }
                else
                {
                    if (_animator.GetInteger("direction") != 0) return;
                    
                    _animator.SetInteger("direction", Random.Range(1, 2));
                    return;
                }
            }
            else if (velocity.x < 0 && velocity.y > 0)
            {
                if (velocity.x *-1 > velocity.y)
                {
                    _animator.SetInteger("direction", 2);
                    return;
                }
                else if (velocity.x *-1 < velocity.y)
                {
                    _animator.SetInteger("direction", 0);
                    return;
                }
                else
                {
                    if (_animator.GetInteger("direction") != 0) return;
                    int ram = Random.Range(0, 1);
                    if (ram == 1) ram = 2;
                    _animator.SetInteger("direction", ram);
                    return;
                }
            }
            else if (velocity.x > 0 && velocity.y < 0)
            {

                if (velocity.x *-1> velocity.y)
                {
                    _animator.SetInteger("direction", 2);
                    return;
                }
                else if (velocity.x * -1 < velocity.y)
                {
                    _animator.SetInteger("direction", 1);
                    return;
                }
                else
                {
                    if (_animator.GetInteger("direction") != 0) return;
                    _animator.SetInteger("direction", Random.Range(1, 2));
                    return;
                }
            }
        }
    }

    #endregion
}
