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

    [Header("Tempo")]
    [SerializeField] float _timeInteraction;

    [Header("se esta interagindo")]
    public bool interection;

    [Header("Debug")]
    [SerializeField] private State PlayerState; 
   
    float stateTIme; //tempo

    private void Update()
    {
        float delta = Time.deltaTime;
        homdleenemyFSM(delta);
    }
    
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
                if (moveButton().x > 0 && !_renderer.flipX || moveButton().x < 0 && _renderer.flipX)
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

    #endregion
}
