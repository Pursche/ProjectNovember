﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOnButtonClick : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	    GetComponent<Button>().onClick.AddListener(OnClick);
	}

    public virtual void OnClick()
    {
        
    }
}
