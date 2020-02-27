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
        SayDialog = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Fungus/Resources/Prefabs/SayDialog.prefab", typeof(GameObject));
        SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SayDialog = GameObject.Find("SayDialog");
        SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
    }

    public void ChangeSayDialog(string character)
    {
        if(character == "Player")
        {
            Debug.Log("Player");
            SayDialogPanel.GetComponent<Image>().sprite = PlayerPanel;
        }
        if (character == "Jasper")
        {
            SayDialogPanel.GetComponent<Image>().sprite = JasperPanel;
        }
        if (character == "Maybelle")
        {
            SayDialogPanel.GetComponent<Image>().sprite = MaybellePanel;
        }
    }

    public void ChangeInsight(int amount)
    {
        InsightGlobal.ChangeInsight(amount);
        Debug.Log("raised insight");
    }
}
