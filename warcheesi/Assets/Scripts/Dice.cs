using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public static int DICE_MAX_VALUE = 6;
    public static Dice Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
		{
            Debug.LogError("There is another Dice instance");
            
            return;
		}

        Instance = this;
    }

    public int ThrowDice(int max)
	{
        return Random.Range(1, max + 1);
	}

    public int ThrowDice()
	{
        return ThrowDice(DICE_MAX_VALUE);
	}
}
