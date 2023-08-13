using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTurnControlWidget : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TeamText;
    [SerializeField]
    private TextMeshProUGUI DiceText;
    [SerializeField]
    private Button PassButton;

	// Start is called before the first frame update
	void Start()
    {
        Debug.Log("subscribe to onturnchange");
        GameManager.Instance.OnTurnChange += OnTurnChange;
        GameManager.Instance.OnDiceThrown += OnDiceThrown;

        PassButton.onClick.AddListener(PassButtonClick);
    }

    private void OnTurnChange(object sender, int currentTeam)
	{
        Debug.Log($"update team to {currentTeam}");
        TeamText.text = currentTeam.ToString();
	}
    private void OnDiceThrown(object sender, int diceValue)
    {
        Debug.Log($"new dice value {diceValue}");
        DiceText.text = diceValue.ToString();
    }

    private void PassButtonClick()
	{
        GameManager.Instance.GoToNextState();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
