using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahtzeeTest : MonoBehaviour
{

    public Shahtzee s;

    void Start()
    {
        s = new Shahtzee();

        int a = s.Aces(new List<int>(){1,1,1,1,1});
        Debug.Log($"Aces: Expected: 5 Actual: {a}");

        if(s.FullHouse(new List<int>(){1,1,1,6,6}) > 0)
            Debug.Log("Full House: Pass");

        if(!(s.FullHouse(new List<int>(){1,2,1,6,6}) > 0))
            Debug.Log("Not Full House: Pass");

        if((s.SmallStraight(new List<int>(){1,2,3,4,2}) > 0))
            Debug.Log("Small Straight With Duplicates: Pass");

        if(!(s.SmallStraight(new List<int>(){1,2,3,5,6}) > 0))
            Debug.Log("No Small Straight: Pass");

        if((s.LargeStraight(new List<int>(){1,2,3,4,5}) > 0))
            Debug.Log("Large Straight: Pass");

        if(!(s.LargeStraight(new List<int>(){6,2,3,5,6}) > 0))
            Debug.Log("No Large Straight: Pass");
    }
}
