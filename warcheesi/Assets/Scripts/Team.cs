using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField]
    private Color Color;
    [SerializeField]
    private int TeamNumber;

    private List<Square> squares;
    private Home home;

    private void Initialize()
	{
        squares = new (GetComponentsInChildren<Square>());
        home = GetComponentInChildren<Home>();

        home.Initialize(TeamNumber,Color);

        squares.ForEach(square => square.UpdateMaterial(Color));
	}

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
