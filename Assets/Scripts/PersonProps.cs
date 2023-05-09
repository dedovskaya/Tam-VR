using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersonProps : MonoBehaviour
{
    public string id = "";
    public string givenname = "";
    public string surname = "";
    public DateTime bdate;
    public string motherId;
    public string fatherId;
    public int sex; //0 fem 1 male
    public string bplace = "";
    public DateTime ddate;

    public GameObject mother;
    public GameObject father;
    public List<GameObject> children;
    public List<GameObject> links;
    public List<GameObject> families;

    public string GetFullName()
    {
        if (!string.IsNullOrEmpty(givenname) && !string.IsNullOrEmpty(surname))
        {
            return givenname + " " + surname;
        }
        else
        {
            return "";
        }
    }

    public GameObject GetMother()
    {
        foreach (GameObject f in families)
        {
            FamilyProps fam_props = f.GetComponent<FamilyProps>();
            if (fam_props.children.Contains(gameObject))
            {
                return fam_props.wife;
            }
        }
        return null;
    }

    public GameObject GetFather()
    {
        foreach (GameObject f in families)
        {
            FamilyProps fam_props = f.GetComponent<FamilyProps>();
            if (fam_props.children.Contains(gameObject))
            {
                return fam_props.husband;
            }
        }
        return null;
    }

    public List<GameObject> GetChildren()
    {
        List<GameObject> ch = new List<GameObject>();

        foreach (GameObject f in families)
        {
            FamilyProps fam_props = f.GetComponent<FamilyProps>();
            if (gameObject == fam_props.husband || gameObject == fam_props.wife)
            {
                ch.AddRange(fam_props.children);
            }
        }
        return ch;
    }

    public List<GameObject> GetSpouses()
    {
        List<GameObject> sp = new List<GameObject>();

        foreach (GameObject f in families)
        {
            FamilyProps fam_props = f.GetComponent<FamilyProps>();
            if (gameObject == fam_props.husband && fam_props.wife)
            {
                sp.Add(fam_props.wife);
            }
            else if (gameObject == fam_props.wife && fam_props.husband)
            {
                sp.Add(fam_props.husband);
            }
        }
        return sp;
    }
}
