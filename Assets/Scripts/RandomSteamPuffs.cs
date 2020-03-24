using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSteamPuffs : MonoBehaviour
{
    [Range(3, 30)]
    float minInterval = 3;
    [Range(6, 40)]
    float maxInterval = 6;


    float timer;
    float timeToPoof;

    Transform[] poofChildren;

    void Start()
    {
        poofChildren = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            poofChildren[i] = transform.GetChild(i);
        }
        timeToPoof = 2f;
    }

    void Update()
    {
        if (timer < timeToPoof)
        {
            timer += Time.deltaTime;
            timeToPoof = Random.Range(minInterval, maxInterval);
        }
        else
        {
            Transform poofChild = poofChildren[Random.Range(0, poofChildren.Length)];
            //poofChild.SetActive(true);
            poofChild.GetComponent<Animator>().SetTrigger("Puff");
            string path = "event:/Ambience/Boiler/Steam Puff";//GetComponent<FMOD_StudioEventEmitter>().path;
            FMODUnity.RuntimeManager.PlayOneShot(path, poofChild.position);
            timer = 0;

        }
    }
}
