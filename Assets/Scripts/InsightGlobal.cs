using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static void ChangeInsight(int amount)
    {
        Mathf.Clamp(amount, -m_InsightLayerValue, m_InsightMaximum - m_InsightLayerValue);
        for (int i = 0; i < Mathf.Abs(amount); i++)
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
                Camera.main.cullingMask |= (toggle << LayerMask.NameToLayer(string.Format("{0} {1}", "Insight level ", amount + i)));
            }
        }
        
    }
}
