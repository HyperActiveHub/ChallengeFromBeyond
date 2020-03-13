using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FungusConversationFunctions : MonoBehaviour
{
    private GameObject SayDialog;
    private GameObject SayDialogPanel;
    public Sprite PlayerPanel;
    public Sprite JasperPanel;
    public Sprite MaybellePanel;

    // Start is called before the first frame update
    void Start()
    {
        SayDialog = GameObject.Find("SayDialog");
        if (!SayDialog)
        {
            SayDialog = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Fungus/Resources/Prefabs/SayDialog.prefab", typeof(GameObject));
            SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
            ChangeSayDialog("Player");
            StartCoroutine(LateStart(0.1f));
        }
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SayDialog = GameObject.Find("SayDialog");
        if (SayDialog)
        {
            SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
        }
        else
        {
            StartCoroutine(LateStart(0.1f));
        }
    }

    public void ChangeSayDialog(string talker)
    {
        if(talker == "Player")
        {
            SayDialogPanel.GetComponent<Image>().sprite = PlayerPanel;
        }
        if (talker == "Jasper")
        {
            SayDialogPanel.GetComponent<Image>().sprite = JasperPanel;
        }
        if (talker == "Maybelle")
        {
            SayDialogPanel.GetComponent<Image>().sprite = MaybellePanel;
        }
    }

    public void ChangeInsight(int amount)
    {
        InsightGlobal.AddInsight(amount, this);
        Debug.Log("raised insight");
    }
}
