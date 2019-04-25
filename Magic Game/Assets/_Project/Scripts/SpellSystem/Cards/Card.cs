﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [Header("Spell editing variables")]
    public List<SpellType> types                        = new List<SpellType>();
    public string cardName                              = null;

    [TextArea]
    public string cardDescription                       = null;

    // Important 3
    public List<CastRequirement> castRequirements       = new List<CastRequirement>();
    public List<SpellBalance> balances                  = new List<SpellBalance>();
    public List<SpellScriptableModifier> modifiers      = new List<SpellScriptableModifier>();

    [Space(10)]
    public GameObject graphicsProjecile                 = null;
    public GameObject graphicsBeam                      = null;
    public GameObject graphicsAoe                       = null;

}
