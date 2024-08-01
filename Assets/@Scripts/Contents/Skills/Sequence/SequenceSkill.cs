using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceSkill : SkillBase
{
    public abstract void DoSkill(Action callback = null);

}
