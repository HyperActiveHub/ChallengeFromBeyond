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
			FMOD.Studio.Bus masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
			masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
			FMODUnity.RuntimeManager.PlayOneShotAttached(filePath, menuButtonController);
		}
		else
		{
			disableOnce = false;
		}
	}
}
