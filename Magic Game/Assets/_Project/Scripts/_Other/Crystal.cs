﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float rotationSpeed = 5.0f;

    [SerializeField] private ScoreSystem scoreSystem = null;

    private void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerCore core = other.GetComponent<PlayerCore>();
            GlobalVariables.crystalsCollected++;

            if (scoreSystem != null)
            {
                scoreSystem.crystalFound = true;
            }

            if (core != null)
            {
                core.GetHUD().spellEditingController.crystalsLeft++;
                Debug.Log("Got a crystal!");
                //core.ToggleSpellEditingUI();
                core.cHealth.UpdateMaxHealth();
            }
            
            Destroy(gameObject);
        }
    }
}
