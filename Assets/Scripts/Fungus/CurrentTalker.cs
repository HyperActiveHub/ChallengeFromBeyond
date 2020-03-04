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
        SayDialog = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Fungus/Resources/Prefabs/SayDialog.prefab", typeof(GameObject));
        SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
        PlayerPanel = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sprites/UI/Dialog/Panels/PlayerPanel.png", typeof(Sprite));
        JasperPanel = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sprites/UI/Dialog/Panels/JasperPanel.png", typeof(Sprite));
        MaybellePanel = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sprites/UI/Dialog/Panels/MaybellePanel.png", typeof(Sprite));
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SayDialog = GameObject.Find("SayDialog");
        SayDialogPanel = SayDialog.transform.GetChild(0).gameObject;
    }

    public override void OnEnter()
    {
        if (talker == Talker.Player)
        {
            Debug.Log("Player");
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
