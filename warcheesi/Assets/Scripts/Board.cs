using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TeamRecord
{
	public readonly int Team;
	public readonly Home Home;
}

public class Board : MonoBehaviour
{
    public static Board Instance = null;


    [SerializeField]
    private List<TeamRecord> Teams;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
		{
            Debug.LogError("There is another board");
            return;
		}

        Instance = this;
    }

    public Home GetHome(int team)
	{
        var record = Teams.Find(record => record.Team == team);

        return record.Home;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
