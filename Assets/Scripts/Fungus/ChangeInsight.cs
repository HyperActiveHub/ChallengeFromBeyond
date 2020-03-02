using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Insight","Change Insight","Change the Insight value by the inputted amount")]
public class ChangeInsight : Command
{
    public int insight;

    public override void OnEnter()
    {
        InsightGlobal.ChangeInsight(insight);
        Debug.Log("raised insight");
        Continue();
    }
}
