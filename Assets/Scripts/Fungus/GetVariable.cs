using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVariable : MonoBehaviour
{
    public VariablePortObject variablePort = null;

    public void OnEnable()
    {
        if (variablePort == null)
        {
            variablePort = Resources.Load<VariablePortObject>("Fungus/VariablePort");
        }
        variablePort.onVariable += OnVariable;
    }

    public void OnDisable()
    {
        variablePort.onVariable -= OnVariable;
    }

    public void OnVariable(VariablePortObject variablePort, string variableName, string variable)
    {
        float outputFloat;
        bool success = float.TryParse(variable, out outputFloat);
        if (success)
        {
            int outputInt;
            success = int.TryParse(variable, out outputInt);
            if (success)
            {
                gameObject.SendMessage(variableName, outputInt);
            }
            else
            {
                gameObject.SendMessage(variableName, outputFloat);
            }
        }
        else
        {
            gameObject.SendMessage(variableName, variable);
        }
    }
}