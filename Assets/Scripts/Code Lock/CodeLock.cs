using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CodeLock : MonoBehaviour
{
    [SerializeField] private List<CodeLockDigit> digits;
    [SerializeField] private int[] solution;
    [HideInInspector] public UnityEvent onUpdate;

    private void OnEnable()
    {
        onUpdate.AddListener(CheckWinCondition);
    }

    private void OnDisable()
    {
        onUpdate.RemoveListener(CheckWinCondition);
    }

    private void CheckWinCondition()
    {
        if (ValidateSolution()) {
            Debug.Log("Win");
        }
    }

    private bool ValidateSolution()
    {
        if(digits.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < digits.Count; i++)
        {
            if(digits[i].currentDigit != solution[i])
            {
                return false;
            }
        }
        return true;
    }
}
