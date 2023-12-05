using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private Camera cameraa;

    [SerializeField]
    private float minColor = 0.315f;
    [SerializeField]
    private float maxColor = 0.645f;

    private float rColor;
    private float gColor;
    private float pColor;

    private bool increaseR;
    private bool decreaseR;
    private bool increaseG;
    private bool decreaseG;
    private bool increaseP;
    private bool decreaseP;

    private void Awake()
    {
        cameraa = GetComponent<Camera>();
    }

    private void Start()
    {
        rColor = minColor; // max max max min min min max
        gColor = minColor; // min min max max max min min
        pColor = maxColor; // max min min min max max max
    }

    private void Update()
    {
        if (!increaseR && !increaseG && !increaseP
            && !decreaseR && !decreaseG && !decreaseP)
        {
            Check();
        }

        HandleColor();
    }

    private void HandleColor()
    {
        if (increaseR)
        {
            rColor += Time.deltaTime;
            if (rColor >= maxColor)
            {
                increaseR = false;
            }
        }
        else if (increaseG)
        {
            gColor += Time.deltaTime;
            if (gColor >= maxColor)
            {
                increaseG = false;
            }
        }
        else if(increaseP)
        {
            pColor += Time.deltaTime;
            if (pColor >= maxColor)
            {
                increaseP = false;
            }
        }
        else if (decreaseR)
        {
            rColor -= Time.deltaTime;
            if (rColor <= minColor)
            {
                decreaseR = false;
            }
        }
        else if (decreaseG)
        {
            gColor -= Time.deltaTime;
            if (gColor <= minColor)
            {
                decreaseG = false;
            }
        }
        else if (decreaseP)
        {
            pColor -= Time.deltaTime;
            if (pColor <= minColor)
            {
                decreaseP = false;
            }
        }

        Color color = new Color(rColor, gColor, pColor);
        cameraa.backgroundColor = color;
    }

    private void Check()
    {
        if (rColor <= minColor && gColor <= minColor)
        {
            // +r
            increaseR = true;
        }
        else if (gColor <= minColor && pColor <= minColor)
        {
            // +g
            increaseG = true;
        }
        else if (pColor <= minColor && rColor <= minColor)
        {
            // +p
            increaseP = true;
        }
        else if (rColor >= maxColor && gColor >= maxColor)
        {
            // -r
            decreaseR = true;
        }
        else if (gColor >= maxColor && pColor >= maxColor)
        {
            // -g
            decreaseG = true;
        }
        else if (pColor >= maxColor && rColor >= maxColor)
        {
            // -p
            decreaseP = true;
        }
    }
}
