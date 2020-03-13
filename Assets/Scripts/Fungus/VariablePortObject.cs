using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VariablePort", menuName = "SP1/Variable Port", order = 2)]
public class VariablePortObject : ScriptableObject
{
    public UnityAction<VariablePortObject, string, string> onVariable = delegate { };

    public void Variable(string variableName, string variable)
    {
        onVariable(this, variableName, variable);
    }
}