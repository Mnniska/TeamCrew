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
    public ModeSelectionScreen modeSelectionScreen;
    public Transform modifierParent;
    public Transform descriptionParent;
    public TextMesh informationText;

    private FadeCollection modCollection;
    private FadeCollection descCollection;
    public bool fadeToMod = false;
    public M_FadeOnScreenSwitch fadeModifier;

    void Start()
    {
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
        modeSelectionScreen.SetPoffState(false);
        informationText.text = "Modifiers";
        modifierParent.gameObject.SetActive(true);
        descriptionParent.gameObject.SetActive(false);
        //fadeToMod = true;
    }
    public void FadeToDesc()
    {
        modeSelectionScreen.SetPoffState(true);
        informationText.text = "Game modes";
        modifierParent.gameObject.SetActive(false);
        descriptionParent.gameObject.SetActive(true);
        //fadeToMod = false;
    }
}
