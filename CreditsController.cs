using UnityEngine;
using System.Collections;

public class CreditsController : MonoBehaviour
{
    public static int Credits;

    public int startingCredits;

    void Start()
    {
        Credits = startingCredits;
    }

    public static bool SubtractCredits(int amount)
    {
        if (CanAfford(amount))
        {
            Credits -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool AddCredits(int amount)
    {
        Credits += amount;
        return true;
    }

    public static bool CanAfford(int amount)
    {
        return Credits >= amount;
    }
}

