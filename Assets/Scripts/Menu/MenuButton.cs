using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    MenuButtonController menuButtonController;
    ButtonFunction buttonFunction;
    SliderFunction sliderFunction;
    Animator animator;
    bool isSlider = false;
    AnimatorButtonFunctions animatorButtonFunctions;
    int thisIndex;
	private int sinceDroppedDown = 4;
	public bool countUp = false;
	private bool releasedButton = true;
	private int countDown = 2;
	private bool hovering = false;

	public void ExitDropDown()
	{
		sinceDroppedDown = 0;
		countUp = true;
		if (Input.GetAxis("Submit") != 0)
		{
			releasedButton = false;
		}
	}

	public void EnterDropDown()
	{
		countUp = false;
		sinceDroppedDown = 0;
		menuButtonController.inDropDown = true;
	}

    public void ChangeScene(string name)
    {
        GameManagerScript.Instance.ChangeScene(name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Update is called once per frame
 //   void Update()
	//{
	//	if (sinceDroppedDown >= countDown && countUp && releasedButton)
	//	{
	//		menuButtonController.inDropDown = false;
	//		countUp = false;
	//	}
	//	if (!menuButtonController.inDropDown)
	//	{
	//		//if (animator)
	//		//{
	//		//	if (menuButtonController.index == thisIndex)
	//		//	{
	//		//		selected();
	//		//		if (!isSlider)
	//		//		{
	//		//			if (Input.GetAxis("Submit") == 1)
	//		//			{
	//		//				pressed();
	//		//			}
	//		//			else if (animator.GetBool("pressed"))
	//		//			{
	//		//				animator.SetBool("pressed", false);
	//		//				animatorButtonFunctions.disableOnce = true;
	//		//			}
	//		//		}
	//		//		else
	//		//		{
	//		//			float input = Input.GetAxis("Horizontal");
	//		//			if (input != 0)
	//		//			{
	//		//				pressed();
	//		//				if (sliderFunction)
	//		//				{
	//		//					sliderFunction.SetValue(input);
	//		//				}
	//		//			}
	//		//			else if (animator.GetBool("pressed"))
	//		//			{
	//		//				animator.SetBool("pressed", false);
	//		//				animatorButtonFunctions.disableOnce = true;
	//		//			}
	//		//		}
	//		//	}
	//		//	else
	//		//	{
	//		//		notSelected();
	//		//	}
	//		//}
	//		//else
	//		//{
	//		//	if (menuButtonController.index == thisIndex)
	//		//	{
	//		//		if (!isSlider)
	//		//		{
	//		//			if (Input.GetAxis("Submit") == 1)
	//		//			{
	//		//				pressed();
	//		//			}
	//		//		}
	//		//		else
	//		//		{
	//		//			float input = Input.GetAxis("Horizontal");
	//		//			if (input != 0)
	//		//			{
	//		//				if (sliderFunction)
	//		//				{
	//		//					sliderFunction.SetValue(input);
	//		//				}
	//		//			}
	//		//		}
	//		//	}
	//		//}
		
	//	}
	//	if (countUp)
	//	{
	//		sinceDroppedDown++;
	//		if (Input.GetAxis("Submit") == 0)
	//		{
	//			releasedButton = true;
	//		}
	//	}
	//}

	private void selected()
	{
		if (animator)
		{
			animator.SetBool("selected", true);
		}
	}

	private void notSelected()
	{
		if (animator && !hovering)
		{
			animator.SetBool("selected", false);
		}
	}

	private void pressed()
	{
		if (animator)
		{
			animator.SetBool("pressed", true);
		}
		if (buttonFunction)
		{
			buttonFunction.Press();
		}
	}

	public void mouseEnter()
	{
		hovering = true;
		selected();
	}

	public void mouseExit()
	{
		hovering = false;
		notSelected();
	}

	public void mouseDown()
	{
		pressed();
	}
}