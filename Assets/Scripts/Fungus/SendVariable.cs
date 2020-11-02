using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Variable", "Send variable", "Sends the variable and its name")]
public class SendVariable : Command
{
    [Tooltip("If it isn't the default")] public VariablePortObject variablePort = null;
    public string variableName;
    private string variable;
    public enum VariableType { Int, Float, String };

    public VariableType variableType;
    // Start is called before the first frame update
    void Start()
    {
        if (variablePort == null)
        {
            variablePort = Resources.Load<VariablePortObject>("Fungus/VariablePort");
        }
    }

    public override void OnEnter()
    {
        if (variableType == VariableType.Int)
        {
            int v = GetFlowchart().GetIntegerVariable(variableName);
            variable = v.ToString();
        }
        if (variableType == VariableType.Float)
        {
            float v = GetFlowchart().GetFloatVariable(variableName);
            variable = v.ToString();
        }
        if (variableType == VariableType.String)
        {
            string v = GetFlowchart().GetStringVariable(variableName);
            variable = v;
        }
        variablePort.Variable(variableName, variable);
        Continue();
    }
}
