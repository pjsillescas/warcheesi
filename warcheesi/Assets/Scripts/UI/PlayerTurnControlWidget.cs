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
    private Button PassButton;

	// Start is called before the first frame update
	void Start()
    {
        Debug.Log("subscribe to onturnchange");
        GameManager.Instance.OnTurnChange += OnTurnChange;

        PassButton.onClick.AddListener(PassButtonClick);
    }

    private void OnTurnChange(object sender, int currentTeam)
	{
        Debug.Log($"update team to {currentTeam}");
        TeamText.text = currentTeam.ToString();
	}

    private void PassButtonClick()
	{
        GameManager.Instance.ActivateNextTeam();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
