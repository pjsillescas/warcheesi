using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Positions;
    [SerializeField]
    private Square GameStartSquare;
    [SerializeField]
    private GameObject TokenPrefab;
    [SerializeField]
    private Color Color;
    [SerializeField]
    private int Team;

    private List<Token> tokens;

	public void Initialize(int team, Color color)
	{
        Team = team;
        Color = color;

        tokens = new();
        if (TokenPrefab != null && Positions != null && Positions.Count > 0)
        {
            foreach (var position in Positions)
            {
                var tokenObj = Instantiate(TokenPrefab, position.position, Quaternion.identity);
                tokenObj.GetComponentInChildren<MeshRenderer>().material.color = Color;
                tokens.Add(tokenObj.GetComponent<Token>());
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
