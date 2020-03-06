using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] ButtonFunction buttonFunction;
	[SerializeField] SliderFunction sliderFunction;
	[SerializeField] Animator animator;
	[SerializeField] bool isSlider = false;
	[SerializeField] AnimatorButtonFunctions animatorButtonFunctions;
	[SerializeField] int thisIndex;
	public int sinceDroppedDown = 4;
	public bool countUp = false;
	private int countDown = 4;

	// Update is called once per frame
	void Update()
	{
		if (sinceDroppedDown >= countDown)
		{
			menuButtonController.inDropDown = false;
		}
		if (!menuButtonController.inDropDown)
		{
			if (animator)
			{
				if (menuButtonController.index == thisIndex)
				{
					animator.SetBool("selected", true);
					if (!isSlider)
					{
						if (Input.GetAxis("Submit") == 1)
						{
							animator.SetBool("pressed", true);
							if (buttonFunction)
							{
								buttonFunction.Press();
							}
						}
						else if (animator.GetBool("pressed"))
						{
							animator.SetBool("pressed", false);
							animatorButtonFunctions.disableOnce = true;
						}
					}
					else
					{
						float input = Input.GetAxis("Horizontal");
						if (input != 0)
						{
							animator.SetBool("pressed", true);
							if (sliderFunction)
							{
								sliderFunction.ChangeValue(input);
							}
						}
						else if (animator.GetBool("pressed"))
						{
							animator.SetBool("pressed", false);
							animatorButtonFunctions.disableOnce = true;
						}
					}
				}
				else
				{
					animator.SetBool("selected", false);
				}
			}
			else
			{
				if (menuButtonController.index == thisIndex)
				{
					if (!isSlider)
					{
						if (Input.GetAxis("Submit") == 1)
						{
							if (buttonFunction)
							{
								buttonFunction.Press();
							}
						}
					}
					else
					{
						float input = Input.GetAxis("Horizontal");
						if (input != 0)
						{
							if (sliderFunction)
							{
								sliderFunction.ChangeValue(input);
							}
						}
					}
				}
			}
		
		}
		if (countUp)
		{
			sinceDroppedDown++;
		}
	}
}