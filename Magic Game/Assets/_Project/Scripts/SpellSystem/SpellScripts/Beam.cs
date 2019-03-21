﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{

    [Header("-- Beam --")]
    [SerializeField] private float baseDamage       = 1.0f;
    [SerializeField] private float baseRange        = 150.0f;
    [SerializeField] private float baseRadius       = 1f;

    private Vector3 direction                       = Vector3.zero;

    public override void CastSpell(Spellbook spellbook, int spellIndex)
    {
        // get the look direction from spellbook and spawn new beam according to that // also child it to player to follow pos and rot
        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Beam beam = Instantiate(this, spellbook.spellPos.position, rot);
        beam.transform.parent = spellbook.transform;

        // apply all spellmodifiers to the beam
        ApplyModifiers(beam.gameObject, spellIndex, spellbook);

        // keep casting beam as long as the beam button is held down TODO:: change this
        beam.StartCoroutine(CastBeam(beam.gameObject, spellbook, spellIndex));
    }

    IEnumerator CastBeam(GameObject self, Spellbook spellbook, int spellIndex)
    {

        print("Started beam cast");

        while (true)
        {
            // keep updating the direction the player is looking and check if our beam hits something
            Vector3 direction = spellbook.GetDirection();
            Ray ray = new Ray(spellbook.spellPos.position, direction * baseRange);
            RaycastHit hit;

            // if beam hits something do this
            if (Physics.Raycast(ray, out hit, baseRange))
            {
                Debug.DrawRay(spellbook.spellPos.position, (hit.point - spellbook.spellPos.position), Color.red);

                // apply beam effects here to target we hit
                if (hit.collider.gameObject.CompareTag("Enemy") /* || hit.collider.gameObject.CompareTag("Player") */)
                {
                    // deal damage to the enemy and apply all collision modifiers ( knockback, burn, etc )
                    hit.collider.GetComponent<Health>().Hurt(baseDamage);
                    OnCollision[] collisionMods = GetComponents<OnCollision>();
                    foreach (OnCollision mod in collisionMods)
                    {
                        mod.Hit(hit.collider.gameObject, spellbook);
                    }
                }
            }
            else
            {
                // do max range beam if nothing is hit
                Debug.DrawRay(spellbook.spellPos.position, ray.direction * baseRange, Color.green);
            }

            // if player is not pressing or releases the beam key stop the cast
            if(Input.GetKeyUp((spellIndex + 1).ToString()) || !Input.GetKey((spellIndex + 1).ToString()))
            {
                print("Beam cast ended");
                break;
            }

            yield return null;
        }

        // stop the spellcast and set the cooldown for the spell
        spellbook.StopCasting();
        Destroy(self);

    }

}
