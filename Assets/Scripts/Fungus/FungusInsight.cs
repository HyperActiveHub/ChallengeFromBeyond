using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusInsight : MonoBehaviour
{
    public float insight;

    public void ChangeInsight(float amount)
    {
        InsightGlobal.AddInsight(amount);
        insight = InsightGlobal.InsightValue;
        Debug.Log("raised insight");
    }
}