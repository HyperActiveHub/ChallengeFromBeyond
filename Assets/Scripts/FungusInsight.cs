using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusInsight : MonoBehaviour
{
    public int insight;

    public void ChangeInsight(int amount)
    {
        InsightGlobal.ChangeInsight(amount);
        insight = InsightGlobal.InsightValue;
        Debug.Log("raised insight");
    }
}