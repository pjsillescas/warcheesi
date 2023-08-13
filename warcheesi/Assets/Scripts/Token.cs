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
        var targetTransform = square.GetFreePosition(this);
        if (targetTransform != null)
        {
            Debug.Log("teleport");
            TeleportTo(targetTransform);
            Debug.Log("setting square");
            CurrentSquare?.Reorganize();
            SetSquare(square);
            if(!inPlay)
			{
                inPlay = true;
			}
            success = true;
        }

        Debug.Log("moveto " + (success ? "pozi" : "pono"));

        return success;
	}

    public void SetSquare(Square square)
	{
        CurrentSquare?.FreeSpace(this);
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
