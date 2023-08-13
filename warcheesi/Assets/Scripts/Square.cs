using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
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
    private List<Transform> TokenPositions;

    [SerializeField]
    private List<Square> NextSquares;

    public void SetReachable()
	{
        Mesh.material = ReachableMaterial;
	}

    public void UpdateMaterial()
	{
        Mesh.material = (IsSafe) ? SafeMaterial : NormalMaterial;
	}

    public void UpdateMaterial(Color color)
	{
        Mesh.material.color = color;
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
