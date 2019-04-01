﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour
{

    public GameObject firstSelected;

    protected EventSystem evenSystem
    {
        get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); }
    }

    public virtual void OnFocus()
    {
        evenSystem.SetSelectedGameObject(firstSelected);
    }

    protected virtual void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()
    {
        Display(false);
    }

    protected virtual void Awake()
    {
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
