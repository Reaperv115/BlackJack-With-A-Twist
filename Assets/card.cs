using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour 
{
    string[] suit = { "spades", "hearts", "diamonds", "clubs" };
    string[] number = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"  };
    public string[] name = new string[52];
    int i = 0;


    private void Awake()
    {
        for (int j = 0; j < 4; ++j)
        {
            for (int k = 0; k < 13; ++k)
            {
                name[i] = number[k] + suit[j];
                ++i;
            }
        }
    }

   
}
