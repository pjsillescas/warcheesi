using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameManager : MonoBehaviour
{
    public enum TurnState { ThrowDice, Move }
    public static GameManager Instance = null;

    [SerializeField]
    private int CurrentTeam;
    public event EventHandler<int> OnTurnChange;
    public event EventHandler<int> OnDiceThrown;

    private InputActions inputActions;
    private Token selectedToken;
    private TurnState currentState;
    private int currentDiceValue;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
		{
            Debug.LogError("There is another game manager");
            return;
		}
        
        Instance = this;

        selectedToken = null;
        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Click.performed += OnPerformedClick;
        currentState = TurnState.ThrowDice;

        StartCoroutine(InitializeTeamCoroutine());
    }

    private IEnumerator InitializeTeamCoroutine()
	{
        yield return new WaitForSeconds(1f);

        Debug.Log("readying team");
        CurrentTeam = UnityEngine.Random.Range(1, 5);
        OnTurnChange?.Invoke(this, CurrentTeam);
        ProcessState(currentState);
        yield return null;
    }
    private void OnPerformedClick(CallbackContext obj)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
		{
            if (hitInfo.collider.TryGetComponent(out Square square))
            {
                Debug.Log($"square {square.name}" );
			}
            else if (hitInfo.collider.GetComponentInParent<Token>() != null)
			{
                var token = hitInfo.collider.GetComponentInParent<Token>();
                Debug.Log($"token {token.name}");
                SelectToken(token);
			}
		}
    }

    private void SelectToken(Token token)
	{
        
        if (token.GetTeam() == CurrentTeam)
        {
            if (selectedToken != null)
            {
                selectedToken.Deselect();
            }

            selectedToken = token;

            selectedToken.Select();

            if(currentState == TurnState.Move)
			{
                var squares = GetSquareDestinations(token, currentDiceValue);
			}
        }
	}

    private List<Square> GetSquareDestinations(Token token, int diceValue)
	{
        var destinations = new List<Square>();

        if(token.IsInPlay())
		{
            ;
		}
        else
		{
            if(diceValue == 5)
			{

			}
		}

        return destinations;
	}

    private void ActivateNextTeam()
	{
        CurrentTeam = CurrentTeam % 4 + 1;
        OnTurnChange?.Invoke(this, CurrentTeam);
    }

    public void GoToNextState()
	{
        switch (currentState)
        {
            case TurnState.Move:
                currentState = TurnState.ThrowDice;
                ActivateNextTeam();
                break;
            case TurnState.ThrowDice:
            default:
                currentState = TurnState.Move;
                break;
        }

        ProcessState(currentState);
    }

    private void ProcessState(TurnState state)
	{
        switch(state)
		{
            case TurnState.Move:
                break;
            case TurnState.ThrowDice:
            default:
                currentDiceValue = Dice.Instance.ThrowDice();
                Debug.Log($"throw dice game manager: {currentDiceValue}");
                OnDiceThrown?.Invoke(this, currentDiceValue);
                break;
		}
	}

    // Update is called once per frame
    void Update()
    {
    }
}
