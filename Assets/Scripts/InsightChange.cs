using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InsightChange : MonoBehaviour
{
    //copy a bit from interactableObject and its editorScript to make this stuff look nicer in inspector
    [Range(0.05f, 1f)]
    [SerializeField] List<float> insightThresholds;
    [SerializeField] List<UnityEngine.Events.UnityEvent> changes = new List<UnityEngine.Events.UnityEvent>();

    private void OnValidate()
    {
        Debug.Assert(insightThresholds.Count == changes.Count, "Object does not have equal amount of insight levels and insight changes.", this);

        float minValue = 0;
        for (int i = 0; i < insightThresholds.Count; i++)
        {
            if (insightThresholds[i] < 0.05f)
                insightThresholds[i] = 0.05f;

            if (insightThresholds[i] > minValue)
            {
                minValue = insightThresholds[i];
            }
            else
                Debug.LogError("Insight-thresholds be of lower values than the next threshold, and higher than the previous, " +
                    "and must be above or eqaul to 0.1f.", this);
        }
    }

    void Start()
    {
        //InsightGlobal.AddInsight(0.5f);
    }

    void Update()
    {
        for (int i = 0; i < insightThresholds.Count; i++)
        {

            if (InsightGlobal.InsightValue >= insightThresholds[i])
            {
                changes[i].Invoke();
                changes.Remove(changes[i]);
                insightThresholds.Remove(insightThresholds[i]);
                break;
            }
        }

    }
}
