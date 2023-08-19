using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum TurnState { Move, CountKill, Six1, Six2 }
    public static GameManager Instance = null;

    [SerializeField]
    List<Team> Teams;

    [SerializeField]
    private int CurrentTeam;
    public event EventHandler<int> OnTurnChange;
    public event EventHandler<int> OnDiceThrown;

    private Token selectedToken;
    private Token lastSelectedToken;
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

        StartCoroutine(InitializeTeamCoroutine());
    }

    private IEnumerator InitializeTeamCoroutine()
	{
        selectedToken = null;
        lastSelectedToken = null;
        
        yield return new WaitForSeconds(0.5f);
        
        currentState = TurnState.Move;

        Debug.Log("readying team");
        CurrentTeam = UnityEngine.Random.Range(1, 5);
        ActivateNextTeam();

        ProcessState(0);
        
        yield return null;
    }

    public void SelectSquare(Square square)
	{
        if(square.IsReachable() && square.HasRoom())
		{
            Debug.Log("move to reachable square " + square.name);
            this.selectedToken.MoveTo(square);

            DeselectCurrentToken();
            if (CheckForKill(square))
            {
                GoToNextState(TurnState.CountKill, SQUARES_TO_MOVE_KILL);
            }
            else
            {
                if (currentDiceValue != 6)
                {
                    Debug.Log("next turn");
                    StartNewTurn();
                }
                else
				{
                    Debug.Log("next dice throw");
                    ProcessState(0);
				}
            }
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
            lastSelectedToken = selectedToken;
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

            reachableSquares = GetSquareDestinations(token, currentDiceValue);
            Debug.Log("squares: " + reachableSquares.Count);
            reachableSquares.ForEach(square => Debug.Log(square.name));
            reachableSquares.ForEach(square => square.SetReachable());
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

    public void GoToNextState(TurnState state, int numSquares)
    {
        currentState = state;
        ProcessState(numSquares);
    }

    private void KillLastToken()
	{
        if (lastSelectedToken != null)
        {
            lastSelectedToken.TeleportTo(GetTeamHome(lastSelectedToken).GetFreePosition());
            Debug.Log("out!!");
        }
        else
		{
            Debug.LogError("There was no last moved token. This should not happen.");
		}
    }

    public void GoToNextState()
	{
        StartNewTurn();
	}

    public void StartNewTurn()
	{
        currentState = TurnState.Move;
        ActivateNextTeam();
        ProcessState(0);
    }

    public const int SQUARES_TO_MOVE_KILL = 20;

    private void SetSquaresToMove(int squaresToMove)
	{
        currentDiceValue = squaresToMove;
        Debug.LogWarning($"throw dice game manager: {currentDiceValue}");
        OnDiceThrown?.Invoke(this, currentDiceValue);

    }

    private void ProcessState(int diceValue)
	{
        SetSquaresToMove((diceValue == 0) ? Dice.Instance.ThrowDice() : diceValue);
        
        Debug.Log("processstate current " + currentState);
        if (currentDiceValue == 6)
        {
            Debug.Log("6 in state " + currentState);
            if (currentState == TurnState.Move)
            {
                GoToNextState(TurnState.Six1, 0);
            }
            else if (currentState == TurnState.Six1)
            {
                GoToNextState(TurnState.Six2,0);
            }
            else if (currentState == TurnState.Six2)
            {
                KillLastToken();
                Debug.Log("SIX3 from processstate gotonextstate");
                StartNewTurn();
            }

            Debug.Log("in new state " + currentState);
        }
	}

    // Update is called once per frame
    void Update()
    {
    }
}
