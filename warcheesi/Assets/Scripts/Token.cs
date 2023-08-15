using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField]
    private int Team;

    [SerializeField]
    private Material SelectedMaterial;
    [SerializeField]
    private Material NormalMaterial;

    private MeshRenderer mesh;
    private Color color;
    private bool inPlay;
    private Square CurrentSquare;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        Deselect();
        inPlay = false;
        CurrentSquare = null;
    }

    public bool IsInPlay() => inPlay;
    public int GetTeam() => Team;

    public Square GetSquare() => CurrentSquare;

    public void SetColor(Color color, int team)
	{
        this.color = color;
        this.Team = team;
    }

    public bool MoveTo(Square square)
	{
        bool success = false;
        var targetTransform = square.OccupyFreePosition(this);
        if (targetTransform != null)
        {
            TeleportTo(targetTransform);
            var previousSquare = CurrentSquare;
            SetSquare(square);
            previousSquare?.Reorganize();
            if (!inPlay)
			{
                inPlay = true;
			}
            success = true;
        }

        return success;
	}

    public bool MoveTo(Home home)
    {
        bool success = false;
        var targetTransform = home.OccupyFreePosition();
        if (targetTransform != null)
        {
            TeleportTo(targetTransform);
            var previousSquare = CurrentSquare;
            SetSquare(null);
            previousSquare?.Reorganize();
            if (!inPlay)
            {
                inPlay = true;
            }
            success = true;
        }

        return success;
    }

    public void SetSquare(Square square)
	{
        if (square != null)
        {
            Debug.Log($"token {name} in square {square.name}");
        }
        else
		{
            Debug.Log($"token {name} in square null");
        }
        CurrentSquare = square;
	}

    public void TeleportTo(Transform transform)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMaterial(Material material)
	{
        var materials = mesh.materials;
        materials[0] = material;
        mesh.materials = materials;

        mesh.material.color = color;
    }
    public void Select()
	{
        SetMaterial(SelectedMaterial);
    }

    public void Deselect()
    {
        SetMaterial(NormalMaterial);
    }
}
