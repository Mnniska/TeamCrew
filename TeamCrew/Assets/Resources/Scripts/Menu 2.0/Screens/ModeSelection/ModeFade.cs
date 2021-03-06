﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeCollection
{
    public SpriteRenderer[] renderers;
    public TextMesh[] meshes;

    public void FadeIn()
    {
        foreach(SpriteRenderer r in renderers)
        {
            Color c = r.color;

            if (c.a < 1)
            {
                c.a = Mathf.MoveTowards(c.a, 1, Time.deltaTime);
                r.color = c;
            }
        }
        foreach (TextMesh t in meshes)
        {
            Color c = t.color;

            if (c.a < 1)
            {
                c.a = Mathf.MoveTowards(c.a, 1, Time.deltaTime);
                t.color = c;
            }
        }
    }
    public void FadeOut()
    {
        foreach (SpriteRenderer r in renderers)
        {
            Color c = r.color;

            if (c.a > 0)
            {
                c.a = Mathf.MoveTowards(c.a, 0, Time.deltaTime);
                r.color = c;
            }
        }
        foreach (TextMesh t in meshes)
        {
            Color c = t.color;

            if (c.a > 0)
            {
                c.a = Mathf.MoveTowards(c.a, 0, Time.deltaTime);
                t.color = c;
            }
        }
    }
}
public class ModeFade : MonoBehaviour 
{
    public Transform modifierParent;
    public Transform descriptionParent;
    public TextMesh informationText;

    private FadeCollection modCollection;
    private FadeCollection descCollection;
    public M_FadeOnScreenSwitch fadeModifier;
    public M_Button kingOfTheHillButtonOn;
    public M_Button kingOfTheHillButtonOff;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        modCollection = new FadeCollection();
        descCollection = new FadeCollection();

        modCollection.renderers = modifierParent.GetComponentsInChildren<SpriteRenderer>();
        modCollection.meshes = modifierParent.GetComponentsInChildren<TextMesh>();

        descCollection.renderers = descriptionParent.GetComponentsInChildren<SpriteRenderer>();
        descCollection.meshes = descriptionParent.GetComponentsInChildren<TextMesh>();

        modifierParent.gameObject.SetActive(false);
    }
    public void FadeToMod()
    {
        informationText.text = "Modifiers";
        modifierParent.gameObject.SetActive(true);
        descriptionParent.gameObject.SetActive(false);

        kingOfTheHillButtonOn.Disabled = (gameManager.GetFrogReadyCount() <= 1);
        kingOfTheHillButtonOn.gameObject.SetActive(!kingOfTheHillButtonOn.Disabled);

        kingOfTheHillButtonOff.Disabled = !kingOfTheHillButtonOn.Disabled;
        kingOfTheHillButtonOff.gameObject.SetActive(!kingOfTheHillButtonOff.Disabled);

        if (kingOfTheHillButtonOn.Disabled)
        {
            GameObject.FindObjectOfType<GameModifiers>().SetModifierState(Modifier.KOTH, false);
            kingOfTheHillButtonOn.gameObject.GetComponent<ModifierAnimatorCaller>().SetState(false);
        }

        ModifierAnimatorCaller[] animCallers = modifierParent.GetComponentsInChildren<ModifierAnimatorCaller>();
        foreach (ModifierAnimatorCaller anim in animCallers)
        {
            anim.Refresh();
        }

    }
    public void FadeToDesc()
    {
        informationText.text = "Mountain type";
        modifierParent.gameObject.SetActive(false);
        descriptionParent.gameObject.SetActive(true);
    }
}
