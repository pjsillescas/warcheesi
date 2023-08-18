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
    [SerializeField]
    private LayerMask TokenLayer;

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
                var token = tokenObj.GetComponent<Token>();
                token.SetColor(Color, team);
                tokens.Add(token);
            }
        }
    }

    private bool ThereIsToken(Transform transform)
	{
        return Physics.Raycast(transform.position, Vector3.up, TokenLayer);
	}

    public Transform GetFreePosition()
    {
        for(var k = 0; k < Positions.Count; k++)
		{
            if (!ThereIsToken(Positions[k]))
            {
                return Positions[k];
            }
        }

        return null;
    }
    public Square GetStartSquare() => GameStartSquare;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
