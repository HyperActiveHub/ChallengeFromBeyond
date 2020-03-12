using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InsightGlobal
{
    private static float m_InsightValue = 0;
    public static float InsightValue
    {
        get { return m_InsightValue; }
    }

    private static bool initialized = false;
    private static Camera camera;

    static void Awake()
    {

    }

    public static void Initialize()
    {

    }


    public static void AddInsight(float amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("added insight amount must be more than 0.");
        }
        else if (amount >= 1)
        {
            Debug.LogError("Can't add more than 100%, add a value between 0 and 1.");
        }
        else
            m_InsightValue += amount;

    }
}
