﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    List<Square> parentSquares = new List<Square>();

    Image image;

    Vector3 initScale;

    public List<Square> ParentSquares
    {
        get { return parentSquares; }
    }

    bool pressed;
    
    public bool IsPressed
    {
        get { return pressed; }
        set { pressed = value; }
    }
    
    private void Start()
    {
        image = GetComponent<Image>();
        initScale = transform.localScale;
    }

    public void AddSquare(Square square)
    {
        parentSquares.Add(square);
    }
    
    public void SetColor(Color color)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Normal");
        transform.localScale = initScale;
        animator.enabled = false;
        image.color = color;
    }
    
    public void On_Pressed()
    {
        if (!pressed)
        {
            pressed = true;
            GameController.Instance.End_Turn(this); // Ver si ha cerrado cuadrado
        }
    }

}
