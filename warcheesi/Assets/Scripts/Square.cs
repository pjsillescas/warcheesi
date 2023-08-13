using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [Serializable]
    public struct TokenPosition
	{
        public Transform Transform;
        public Token Token;
	}

    [SerializeField]
    private Material NormalMaterial;
    [SerializeField]
    private Material SafeMaterial;
    [SerializeField]
    private Material ReachableMaterial;

    [SerializeField]
    private bool IsSafe;

    [SerializeField]
    private MeshRenderer Mesh;

    [SerializeField]
    private List<TokenPosition> TokenPositions;

    [SerializeField]
    private List<Square> NextSquares;

    private bool isReachable;

    public void SetReachable()
	{
        Mesh.material = ReachableMaterial;
        isReachable = true;
	}

    public bool IsReachable() => isReachable;

    public List<Square> GetNextSquares() => NextSquares;

    public void UpdateMaterial()
	{
        Mesh.material = (IsSafe) ? SafeMaterial : NormalMaterial;
        isReachable = false;
    }

    public void UpdateMaterial(Color color)
	{
        Mesh.material.color = color;
	}

    public Transform GetFreePosition(Token token)
	{
        if(TokenPositions[0].Token == null && TokenPositions[1].Token == null && TokenPositions[2].Token == null)
		{
            SetPositionToken(1, token);
            return TokenPositions[1].Transform;
        }
        else if (TokenPositions[0].Token == null && TokenPositions[1].Token != null && TokenPositions[2].Token == null)
		{
            TokenPositions[1].Token.TeleportTo(TokenPositions[0].Transform);
            SetPositionToken(0, TokenPositions[1].Token);

            SetPositionToken(1, null);

            token.SetSquare(this);
            SetPositionToken(2,token);
            return TokenPositions[2].Transform;
        }
        else
		{
            return null;
		}
    }

    public void FreeSpace(Token token)
    {
        for (int k = 0; k <= 2; k++)
        {
            if (TokenPositions[k].Token == token)
            {
                SetPositionToken(k, null);
            }
        }
    }

    public void Reorganize()
	{
        var x1 = (TokenPositions[0].Token != null) ? "X" : "-";
        var x2 = (TokenPositions[1].Token != null) ? "X" : "-";
        var x3 = (TokenPositions[2].Token != null) ? "X" : "-";
        Debug.Log("reorganizing " + x1 + x2 + x3);
        if (TokenPositions[0].Token != null && TokenPositions[1].Token == null && TokenPositions[2].Token == null)
		{
            TokenPositions[0].Token.TeleportTo(TokenPositions[1].Transform);
            SetPositionToken(1, TokenPositions[0].Token);

            TokenPositions[1].Token.SetSquare(this);
            SetPositionToken(0, null);
        }
        else if (TokenPositions[0].Token == null && TokenPositions[1].Token == null && TokenPositions[2].Token != null)
        {
            TokenPositions[2].Token.TeleportTo(TokenPositions[1].Transform);
            SetPositionToken(1, TokenPositions[2].Token);

            TokenPositions[1].Token.SetSquare(this);
            SetPositionToken(2, null);
        }

    }

    private void SetPositionToken(int index, Token token)
	{
        var position = TokenPositions[index];
        position.Token = token;
        TokenPositions[index] = position;
    }

    private void Awake()
	{
        Mesh = GetComponentInChildren<MeshRenderer>();
        UpdateMaterial();
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
