using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	void PlaySound(string filePath, GameObject objectTheEmitterIsOn)
	{
		FMODUnity.RuntimeManager.PlayOneShotAttached(filePath, objectTheEmitterIsOn);
	}
	
}
