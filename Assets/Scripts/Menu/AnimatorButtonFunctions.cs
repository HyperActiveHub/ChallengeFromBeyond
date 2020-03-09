using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorButtonFunctions : MonoBehaviour
{
	[Tooltip("Most likely the canvas")] [SerializeField] GameObject menuButtonController;

	public bool disableOnce;
	void PlayButtonSound(string filePath)
	{
		if (!disableOnce)
		{
			FMODUnity.RuntimeManager.PlayOneShotAttached(filePath, menuButtonController);
		}
		else
		{
			disableOnce = false;
		}
	}
}
