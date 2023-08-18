using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum TurnState { ThrowDice, Move, CountKill, Six1, Six2, Six3, Move1, Move2 }
    public static GameManager Instance = null;

    [SerializeField]
    List<Team> Teams;

    [SerializeField]
    private int CurrentTeam;
    public event EventHandler<int> OnTurnChange;
    public event EventHandler<int> OnDiceThrown;

    private Token selectedToken;
    private TurnState currentState;
    private int currentDiceValue;
    List<Square> reachableSquares;

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
        currentState = TurnState.ThrowDice;

        StartCoroutine(InitializeTeamCoroutine());
    }

    private IEnumerator InitializeTeamCoroutine()
	{
        yield return new WaitForSeconds(0.5f);

        Debug.Log("readying team");
        CurrentTeam = UnityEngine.Random.Range(1, 5);
        OnTurnChange?.Invoke(this, CurrentTeam);
        ProcessState();
        yield return null;
    }

    public void SelectSquare(Square square)
	{
        if(square.IsReachable() && square.HasRoom())
		{
            Debug.Log("move to reachable square " + square.name);
            this.selectedToken.MoveTo(square);

            DeselectCurrentToken();
            if(CheckForKill(square))
			{
                GoToNextState(TurnState.CountKill);
			}
            GoToNextState();
        }
        else
		{
            Debug.Log("square not reachable " + square.name);
            DeselectCurrentToken();
		}
    }

    private Home GetTeamHome(Token token)
	{
        return Board.Instance.GetHome(token.GetTeam());
    }

    private bool CheckForKill(Square square)
	{
        var tokens = square.GetTokens();
        var isKill = tokens.Count > 1 && tokens[0].GetTeam() != tokens[1].GetTeam() && !square.IsSafeSquare();
        if (isKill)
		{
            Debug.Log("token killed");
            tokens[0].MoveTo(GetTeamHome(tokens[0]));
		}

        return isKill;
	}

    public void DeselectCurrentToken()
	{
        if (selectedToken != null)
        {
            reachableSquares?.ForEach(square => square.UpdateMaterial());
            selectedToken.Deselect();
        }
    }

    public void SelectToken(Token token)
	{
        if (token.GetTeam() == CurrentTeam)
        {
            DeselectCurrentToken();

            selectedToken = token;

            selectedToken.Select();

            if(currentState == TurnState.Move || currentState == TurnState.Move1 || currentState == TurnState.Move2)
			{
                reachableSquares = GetSquareDestinations(token, currentDiceValue);
                Debug.Log("squares: " + reachableSquares.Count);
                reachableSquares.ForEach(square => Debug.Log(square.name));
                reachableSquares.ForEach(square => square.SetReachable());
			}
        }
	}

    private void GetSquareDestinations(Square startSquare, int numSquares, ref List<Square> destinations)
	{
        if (numSquares > 0)
		{
            foreach(var square in startSquare.GetNextSquares())
			{
                GetSquareDestinations(square, numSquares - 1, ref destinations);
			}
		}
        else
		{
            destinations.Add(startSquare);
		}
	}

    private List<Square> GetSquareDestinations(Token token, int diceValue)
	{
        var destinations = new List<Square>();

        if(token.IsInPlay())
		{
            var startSquare = token.GetSquare();
            GetSquareDestinations(startSquare, diceValue, ref destinations);
		}
        else
		{
            //if(diceValue == 5)
			{
                var home = Board.Instance.GetHome(CurrentTeam);
                var square = home.GetStartSquare();
                destinations.Add(square);
			}
		}

        return destinations;
	}

    private void ActivateNextTeam()
	{
        CurrentTeam = CurrentTeam % 4 + 1;
        Debug.Log("activating team " + CurrentTeam);
        OnTurnChange?.Invoke(this, CurrentTeam);
    }

    public void GoToNextState(TurnState state)
	{
        currentState = state;
        //GoToNextState();
	}

    private void KillLastToken()
	{
        Debug.Log("out!!");
    }

    public void GoToNextState()
	{
        switch (currentState)
        {
            case TurnState.Move1:
            case TurnState.Move2:
                break;
            case TurnState.Move:
                StartNewTurn();
                break;
            case TurnState.CountKill:
                SetSquaresToMove(SQUARES_TO_MOVE_KILL);
                currentState = TurnState.Move;
                break;
            case TurnState.Six1:
                currentState = TurnState.Move1;
                break;
            case TurnState.Six2:
                currentState = TurnState.Move2;
                break;
            case TurnState.Six3:
                KillLastToken();
                StartNewTurn();
                break;
            case TurnState.ThrowDice:
            default:
                currentState = TurnState.Move;
                break;
        }

        ProcessState();
    }

    public void StartNewTurn()
	{
        currentState = TurnState.ThrowDice;
        ActivateNextTeam();
    }

    public const int SQUARES_TO_MOVE_KILL = 20;

    private void SetSquaresToMove(int squaresToMove)
	{
        currentDiceValue = squaresToMove;
        Debug.Log($"throw dice game manager: {currentDiceValue}");
        OnDiceThrown?.Invoke(this, currentDiceValue);

    }

    private void ProcessState()
	{
        Debug.Log("processstate current " + currentState);
        switch(currentState)
		{
            case TurnState.Six3:
                selectedToken.TeleportTo(GetTeamHome(selectedToken).GetFreePosition());
                Debug.Log("SIX3 from processstate gotonextstate");
                GoToNextState();
                break;
            case TurnState.Move:
            case TurnState.CountKill:
                Debug.Log("in moving state");
                break;
            case TurnState.ThrowDice:
            default:
                SetSquaresToMove(Dice.Instance.ThrowDice());
                Debug.Log("THROW from processstate gotonextstate " + currentState);
                if (currentDiceValue == 6)
                {
                    Debug.Log("6 in state " + currentState);
                    if (currentState == TurnState.Move)
                    {
                        GoToNextState(TurnState.Six1);
                    }
                    else if (currentState == TurnState.Move1)
                    {
                        GoToNextState(TurnState.Six2);
                    }
                    else if(currentState == TurnState.Move2)
                    {
                        GoToNextState(TurnState.Six3);
                    }
                    else
                    {
                        GoToNextState();
                    }
                    
                    Debug.Log("in new state " + currentState);
                }
                else
                {
                    Debug.Log("not a 6 " + currentDiceValue + " in state " + currentState);
                    GoToNextState();
                }
                break;
		}
	}

    // Update is called once per frame
    void Update()
    {
    }
}
