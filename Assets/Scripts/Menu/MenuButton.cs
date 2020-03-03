using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[Tooltip("Most likely the canvas")] [SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorButtonFunctions animatorButtonFunctions;
	[SerializeField] int thisIndex;

	// Update is called once per frame
	void Update()
	{
		if (menuButtonController.index == thisIndex)
		{
			animator.SetBool("selected", true);
			if (Input.GetAxis("Submit") == 1)
			{
				animator.SetBool("pressed", true);
			}
			else if (animator.GetBool("pressed"))
			{
				animator.SetBool("pressed", false);
				animatorButtonFunctions.disableOnce = true;
			}
		}
		else
		{
			animator.SetBool("selected", false);
		}
	}
}