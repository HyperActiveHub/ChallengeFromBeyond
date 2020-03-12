using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public static class GetVariable
{
    public static int getInt(string variableName , Flowchart flowchart)
    {
        int variable = flowchart.GetIntegerVariable(variableName);
        return variable;
    }
    public static float getFloat(string variableName, Flowchart flowchart)
    {
        float variable = flowchart.GetFloatVariable(variableName);
        return variable;
    }
    public static string getString(string variableName, Flowchart flowchart)
    {
        string variable = flowchart.GetStringVariable(variableName);
        return variable;
    }
}
