using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CodeLock : MonoBehaviour
{
    [SerializeField] private List<CodeLockDigit> digits;
    [SerializeField] private int[] solution;
    [HideInInspector] public UnityEvent onUpdate;
    [SerializeField] private UnityEvent onWin;

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
            onWin.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }

    private bool ValidateSolution()
    {
        if(digits.Count == 0)
        {
            return false;
        }

        if(digits.Count != solution.Length)
        {
            Debug.LogError(string.Format("Number of digits: {0}, no. digits in the solution {1}. They need to match!", digits.Count, solution.Length));
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
