using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InsightGlobal
{
    private static int m_InsightLayerValue;
    public static int InsightLayerValue
    {
        get { return m_InsightLayerValue; }
        set { m_InsightLayerValue = value; }
    }

    private static int m_InsightValue;
    public static int InsightValue
    {
        get { return m_InsightValue; }
        set { m_InsightValue = value; }
    }

    private static int m_InsightMaximum;
    public static int InsightMaximum
    {
        get { return m_InsightMaximum; }
        set { m_InsightMaximum = value; }
    }

    private static bool initialized = false;
    private static Camera camera;

    static void Awake()
    {
        if (InsightMaximum<=0)
        {
            InsightMaximum = 10;
        }

        if (InsightMaximum <= 0)
        {
            InsightMaximum = 10;
        }
    }

    // ensures you call this method from a script in your first loaded scene
    public static void Initialize()
    {
        if (initialized == false)
        {
            initialized = true;
            // adds this to the 'activeSceneChanged' callbacks if not already initialized.
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneWasLoaded;
        }
    }

    // triggers when a new scene is loaded
    private static void OnSceneWasLoaded(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        // fixes main camera if not null
        if (camera)
        {
            Camera.main.cullingMask = camera.cullingMask;
        }
        camera = Camera.main;
    }

    public static void ChangeInsight(int amount)
    {
        Mathf.Clamp(amount, -m_InsightLayerValue, m_InsightMaximum - m_InsightLayerValue);
        InsightValue += amount;
        /*for (int i = 0; i < Mathf.Abs(amount); i++)
        {
            int toggle;
            if(amount > 0)
            {
                toggle = 1;
            }
            else
            {
                toggle = 0;
            }
            if(LayerMask.NameToLayer(string.Format("{0} {1}", "Insight level ", amount + i)) == -1){
                Debug.LogError(string.Format("{0} {1} {2}", "Insight level ", amount + i, "doesn't exist"));
            }
            else
            {
                camera.cullingMask |= (toggle << LayerMask.NameToLayer(string.Format("{0} {1}", "Insight level ", amount + i)));
            }
        }*/

    }
}
