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

    private Color color;
    private bool useColor;
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

        if (useColor)
        {
            Mesh.material.color = color;
        }
        
        isReachable = false;
    }

    public void UpdateMaterial(Color color)
	{
        useColor = true;
        this.color = color;
        UpdateMaterial();
	}

    public bool HasRoom()
	{
        return TokenPositions[1].Token != null || TokenPositions[0].Token == null && TokenPositions[2].Token == null;

    }

    public Transform OccupyFreePosition(Token token)
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

    private void RepositionToken(int indexFrom, int indexTo)
	{
        TokenPositions[indexFrom].Token.TeleportTo(TokenPositions[indexTo].Transform);
        SetPositionToken(indexTo, TokenPositions[indexFrom].Token);

        SetPositionToken(indexFrom, null);

    }

    private void FreeSpaces()
	{
        for (var k = 0; k < 3; k++)
        {
            if (TokenPositions[k].Token != null && !TokenPositions[k].Token.GetSquare().Equals(this))
            {
                SetPositionToken(k, null);
            }
        }
    }

    public void Reorganize()
	{
        FreeSpaces();
        if (TokenPositions[0].Token != null && TokenPositions[1].Token == null && TokenPositions[2].Token == null)
		{
            RepositionToken(0,1);
        }
        else if (TokenPositions[0].Token == null && TokenPositions[1].Token == null && TokenPositions[2].Token != null)
        {
            RepositionToken(2, 1);
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
        useColor = false;
        isReachable = false;

        Mesh = GetComponentInChildren<MeshRenderer>();
        UpdateMaterial();
    }
}
