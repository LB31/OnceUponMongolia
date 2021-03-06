using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    public int NeededAmountToFinish;

    private HashSet<string> foodInPot = new HashSet<string>();

    public void PutFoodInPot(string foodName)
    {
        if (!foodName.Contains("Cut")) return;

        foodInPot.Add(foodName.Substring(0, 3));
        print("pot amount " + foodInPot.Count);

        if(foodInPot.Count >= NeededAmountToFinish)
        {
            Debug.LogError("FINITO");
        }
    }
}
