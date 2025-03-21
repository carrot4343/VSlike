using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : BaseController
{
    public int GemValue
    {
        get
        {
            return gemValue;
        }
        set
        {
            gemValue = value;
        }
    }
    int gemValue = 1;
    public string GemSpriteName
    {
        get;set;
    }
    public override bool Init()
    {
        base.Init();

        if (gemValue < 20)
        {
            GemSpriteName = "GreenGem.sprite";
        }
        else if(gemValue >= 30)
        {
            GemSpriteName = "YellowGem.sprite";
        }
        else
        {
            GemSpriteName = "BlueGem.sprite";
        }

        return true;
    }
}
