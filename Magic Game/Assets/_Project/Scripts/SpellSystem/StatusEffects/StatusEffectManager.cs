﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    public enum EffectType
    {
        Freeze,
        Ignite,
        Moist,
        Confuse,
    };

    public List<StatusEffect> affectingEffects = new List<StatusEffect>();
    public Dictionary<EffectType, bool> AppliedEffects = new Dictionary<EffectType, bool>();


    private void Awake()
    {
        AppliedEffects.Add(EffectType.Freeze, false);
        AppliedEffects.Add(EffectType.Ignite, false);
        AppliedEffects.Add(EffectType.Moist, false);
        AppliedEffects.Add(EffectType.Confuse, false);
    }


    private void Update()
    {
        for (int i = 0; i < affectingEffects.Count; i++)
        {
            affectingEffects[i].OnTick();

            if (affectingEffects[i].IsFinished)
            {
                RemoveStatusEffect(affectingEffects[i]);
            }
        }
    }

    public void ApplyStatusEffect(StatusEffect effect, List<StatusEffect> allEffectsInSpell)
    {
        for (int i = 0; i < affectingEffects.Count; i++)
        {
            if (affectingEffects[i].GetType() == effect.GetType())
            {
                // same effect already exisit
                affectingEffects[i].ReApply(allEffectsInSpell);
                return;
            }
        }

        // if we get this far the effect is new       
        affectingEffects.Add(effect);
        effect.OnApply(gameObject, allEffectsInSpell);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        for (int i = 0; i < affectingEffects.Count; i++)
        {
            if (affectingEffects[i].GetType() == effect.GetType())
            {
                // found the effect we want to remove
                affectingEffects[i].OnLeave();
                affectingEffects.RemoveAt(i);
                return;
            }
        }
    }

}
