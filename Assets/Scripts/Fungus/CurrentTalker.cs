using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using UnityEditor;

[CommandInfo("Talker", "Current talker", "Changes the dialog according to the talker")]
public class CurrentTalker : Command
{
    public enum Talker { Player, Jasper, Maybelle};

    public Talker talker;

    private GameObject SayDialog;
    private GameObject SayDialogPanel;
    private Sprite PlayerPanel;
    private Sprite JasperPanel;
    private Sprite MaybellePanel;

    // Start is called before the first frame update
    void Start()
    {
        SayDialog = GameObject.Find("SayDialog");
        if (!SayDialog)
        {
            //SayDialog = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Fungus/Resources/Prefabs/SayDialog.prefab", typeof(GameObject));
            //SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
            //SayDialogPanel.GetComponent<Image>().sprite = PlayerPanel;
            StartCoroutine(LateStart(0.1f));
        }
        PlayerPanel = Resources.Load<Sprite>("Fungus/PlayerPanel.png");
        JasperPanel = Resources.Load<Sprite>("Fungus/JasperPanel.png");
        MaybellePanel = Resources.Load<Sprite>("Fungus/MaybellePanel.png");

        //cant use AssetDatabase ingame
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

    public override void OnEnter()
    {
        if (talker == Talker.Player)
        {
            SayDialogPanel.GetComponent<Image>().sprite = PlayerPanel;
        }
        if (talker == Talker.Jasper)
        {
            SayDialogPanel.GetComponent<Image>().sprite = JasperPanel;
        }
        if (talker == Talker.Maybelle)
        {
            SayDialogPanel.GetComponent<Image>().sprite = MaybellePanel;
        }
        Continue();
    }
}
