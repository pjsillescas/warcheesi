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
    private Square CurrentSquare;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        Deselect();
        CurrentSquare = null;
    }

    public bool IsInPlay() => CurrentSquare != null;
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
        var targetTransform = square.GetFreePosition(this);
        if (targetTransform != null)
        {
            TeleportTo(targetTransform);
            var previousSquare = CurrentSquare;
            SetSquare(square);
            previousSquare?.Reorganize();
            success = true;
        }

        return success;
	}

    public bool MoveTo(Home home)
    {
        bool success = false;
        var targetTransform = home.GetFreePosition();
        if (targetTransform != null)
        {
            TeleportTo(targetTransform);
            var previousSquare = CurrentSquare;
            previousSquare.ReleaseFromToken(this);
            SetSquare(null);
            previousSquare?.Reorganize();
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
