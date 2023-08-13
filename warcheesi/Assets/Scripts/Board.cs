using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TeamRecord
{
	public int Team;
	public Home Home;
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
        Debug.Log("home " + record.Home.name);
        return record.Home;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
