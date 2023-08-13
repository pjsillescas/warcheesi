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

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        Deselect();
        inPlay = false;
    }

    public bool IsInPlay() => inPlay;
    public int GetTeam() => Team;


    public void SetColor(Color color, int team)
	{
        this.color = color;
        this.Team = team;
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
