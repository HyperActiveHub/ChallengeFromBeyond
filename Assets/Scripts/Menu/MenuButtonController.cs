using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{

	// Use this for initialization
	public int index;
	[SerializeField] bool keyDown;
	public bool inDropDown = false;
	[SerializeField] int maxIndex;
	public GameObject[] sliders;

	private void OnDisable()
	{
		foreach (GameObject slider in sliders)
		{
			slider.SetActive(false);
		}
	}

	private void OnEnable()
	{
		foreach (GameObject slider in sliders)
		{
			slider.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update()
	{

		if (!inDropDown)
		{
			if (Input.GetAxis("Vertical") != 0)
			{
				if (!keyDown)
				{
					if (Input.GetAxis("Vertical") > 0)
					{
						if (index < maxIndex)
						{
							index++;
						}
						else
						{
							index = 0;
						}
					}
					else if (Input.GetAxis("Vertical") < 0)
					{
						if (index > 0)
						{
							index--;
						}
						else
						{
							index = maxIndex;
						}
					}
					keyDown = true;
				}
			}
			else
			{
				keyDown = false;
			}
		}
	}

}
