using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ParseGedcom : MonoBehaviour
{
    public GameObject personPrefab;
    public GameObject familyPrefab;
    public GameObject linkPrefab;
    public GameObject emptyGameObjectPrefab;
    public TextAsset tfm_graph;

    public TextAsset gedcom_file;
    public List<GameObject> persons;
    public List<GameObject> families;
    public List<GameObject> links;

    private GameObject current_person;
    private GameObject current_family;
    private string current_parentType;
    private int procreationAge = 20;

    private void Start()
    {
        LoadGedcom(gedcom_file);
        EstimateMissingDates(persons, families, procreationAge);
        SetParents();
        GetCoordinates(tfm_graph);
        CreateLinks();
        Debug.Log("Loaded " + persons.Count + " persons in " + families.Count + " families");

    }

    //Rename .ged to .txt
    public void LoadGedcom(TextAsset gedcom_file)
    {
        string[] lines = gedcom_file.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        ////Person
        //GameObject current_person = Instantiate(personPrefab);
        //current_person.name = "person";
        //PersonProps person_props = current_person.GetComponent<PersonProps>();

        ////Family
        //GameObject current_family = Instantiate(familyPrefab);
        //current_family.name = "family";
        //FamilyProps family_props = current_family.GetComponent<FamilyProps>();
        
        GameObject fam = GameObject.Find("Families");
        fam.transform.position = new Vector3(0, 0, 0);
        GameObject per = GameObject.Find("Persons");
        per.transform.position = new Vector3(0, 0, 0);

        for (int i = 0; i < lines.Length; i++)
        {
            var tokens = lines[i].Split(' ');
            if (tokens[0] == "0" && tokens.Length > 2)
            {
                var nodeType = tokens[2].Trim();
                if (nodeType == "INDI")
                {
                    var id = tokens[1].ToString();
                    //Person
                    GameObject person = Instantiate(personPrefab, per.transform);
                    person.name = id;
                    PersonProps person_props = person.GetComponent<PersonProps>();
                    persons.Add(person);
                    current_person = person;
                }
                else if (nodeType == "FAM")
                {
                    var id = tokens[1];
                    //Family
                    GameObject family = Instantiate(familyPrefab, fam.transform);
                    family.name = id;
                    FamilyProps family_props = family.GetComponent<FamilyProps>();
                    families.Add(family);
                    current_family = family;
                }
                else
                {
                    current_person = emptyGameObjectPrefab;
                    current_family = emptyGameObjectPrefab;
                }
            }
            //-------------------------------------------------------------
            else if (tokens[0] == "1")
            {
                var nodeType = tokens[1].Trim();
                current_parentType = nodeType;

                //-------------------------- encounterd while parsing PERSONS
                if (current_parentType == "NAME" && current_person != emptyGameObjectPrefab && current_person.GetComponent<PersonProps>().GetFullName() == "")
                {
                    for (int j = 2; j < tokens.Length; j++)
                    {
                        if (current_person.GetComponent<PersonProps>().surname == "" && tokens[j].StartsWith("/")) // extract surname
                        {
                            current_person.GetComponent<PersonProps>().surname = tokens[j].Replace("/", " ").Trim();
                        }
                        else if (current_person.GetComponent<PersonProps>().givenname == "") // given name
                        {
                            current_person.GetComponent<PersonProps>().givenname = tokens[j].Trim();
                        }
                    }
                }
                else if (nodeType == "SEX" && current_person != emptyGameObjectPrefab)
                {
                    var sex = tokens[2].Trim();
                    current_person.GetComponent<PersonProps>().sex = sex == "M" ? 1 : 0;
                }
                //-------------------------- encounterd while parsing FAMILIES
                else if (nodeType == "HUSB")
                {
                    // important to trim trailing \r from id token!!
                    var id = tokens[2].Trim();
                    GameObject person = persons.Find(o => o.name == id);

                    if (person != null)
                    {
                        // create bidirectional link between family and person
                        person.GetComponent<PersonProps>().families.Add(current_family);
                        current_family.GetComponent<FamilyProps>().husband = person;
                    }
                }
                else if (nodeType == "WIFE")
                {
                    var id = tokens[2].Trim();
                    GameObject person = persons.Find(o => o.name == id);
                    if (person != null)
                    {
                        // create bidirectional link between family and person
                        person.GetComponent<PersonProps>().families.Add(current_family);
                        current_family.GetComponent<FamilyProps>().wife = person;
                    }
                }
                else if (nodeType == "CHIL")
                {
                    var id = tokens[2].Trim();
                    GameObject person = persons.Find(o => o.name == id);
                    if (person == null)
                    {
                        person = Instantiate(personPrefab);
                        person.name = id;
                        PersonProps person_props = person.GetComponent<PersonProps>();
                        persons.Add(person);
                    }

                    // create bidirectional link between family and person
                    person.GetComponent<PersonProps>().families.Add(current_family);
                    current_family.GetComponent<FamilyProps>().children.Add(person);
                }
            }
            //-------------------------------------------------------------
            else if (tokens[0] == "2")
            {
                if (tokens[1] == "DATE" && tokens.Length > 2)
                {
                    DateTime date = new DateTime(900, 1, 1); // Default date
                    string datestr = string.Join(" ", tokens.Skip(2));

                    string cleanstr = datestr
                        .ToLower()
                        // unwanted characters and words
                        .Replace(".", " ").Replace("?", " ").Replace(",", " ")
                        .Replace("abt", " ").Replace("before", " ").Replace("bef", " ")
                        .Replace("undefined ", " ").Replace("undef ", " ")
                        .Replace("um ", " ")
                        // common german wording replacements
                        .Replace("jänner", "jan").Replace("januar ", "jan ")
                        .Replace("feber", "feb").Replace("februar ", "feb ")
                        .Replace("märz", "mar").Replace("mai", "may")
                        .Replace("juni", "jun").Replace("juli", "jul")
                        .Replace("okt", "oct").Replace("dez", "dec")
                        .Replace("ä", "a")
                        .Trim();

                    // add day number in case only month and year is given
                    if (cleanstr.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == 2
                        && Regex.IsMatch(cleanstr, "^[jfmasond]"))
                    {
                        cleanstr = "1 " + cleanstr;
                    }

                    // convert to timestap in ms
                    if (DateTime.TryParse(cleanstr, out DateTime parsedDate))
                    {
                        date = parsedDate;
                    }
                    else
                    {
                        // parsing error -> parse ourselves
                        string[] a = cleanstr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (a.Length > 2)
                        {
                            string customstr = $"{a[2].Trim()}-{a[1].Trim()}-{a[0].Trim()}";
                            if (DateTime.TryParse(customstr, out parsedDate))
                            {
                                date = parsedDate;
                            }
                            else
                            {
                                Console.WriteLine($"Can't parse custom date string '{customstr}' ({cleanstr}) ({datestr})");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Can't parse date string '{datestr}' ({cleanstr})");
                            date = new DateTime(1000, 1, 1); // unknown date
                        }
                    }

                    // set date to event
                    if (current_parentType == "BIRT")
                    {
                        current_person.GetComponent<PersonProps>().bdate = date;
                    }
                    else if (current_parentType == "DEAT")
                    {
                        current_person.GetComponent<PersonProps>().ddate = date;
                    }
                }
            }
        }
    }

    public void EstimateMissingDates(List<GameObject> persons, List<GameObject> families, int procreationAge)
    {
        DateTime defaultDate = new DateTime();
        var updated = true;
        while (updated)
        {
            // continue estimation until nothing was updated anymore
            updated = false;
            foreach (GameObject person in persons)
            {
                PersonProps p = person.GetComponent<PersonProps>();

                if (p.bdate == defaultDate)    // missing date of birth
                {
                    GameObject mother = p.GetMother();
                    GameObject father = p.GetFather();

                    // birthday of youngest parent
                    var pbdate = defaultDate;
                    var mbdate = mother ? mother.GetComponent<PersonProps>().bdate : defaultDate;
                    var fbdate = father ? father.GetComponent<PersonProps>().bdate : defaultDate;
                    if (mbdate != defaultDate && fbdate == defaultDate) pbdate = mbdate;
                    else if (mbdate == defaultDate && fbdate != defaultDate) pbdate = fbdate;
                    else if (mbdate == defaultDate && fbdate == defaultDate) pbdate = (mbdate > fbdate) ? mbdate : fbdate;

                    // birthday of oldest child
                    var cbdate = defaultDate;
                    var children = p.GetChildren();
                    foreach (GameObject c in children)
                    {
                        if (cbdate == defaultDate)
                        {
                            cbdate = c.GetComponent<PersonProps>().bdate;
                        }
                        else if (c.GetComponent<PersonProps>().bdate < cbdate)
                        {
                            cbdate = c.GetComponent<PersonProps>().bdate;
                        }
                    }
                    // birthday of oldest spouse
                    var spbdate = defaultDate;
                    var spouses = p.GetSpouses();
                    foreach (GameObject sp in spouses)
                    {
                        if (spbdate == defaultDate)
                        {
                            spbdate = sp.GetComponent<PersonProps>().bdate;
                        }
                        else if (sp.GetComponent<PersonProps>().bdate < spbdate)
                        {
                            spbdate = sp.GetComponent<PersonProps>().bdate;
                        }
                    }
                    // estimate based on parent or child birthdates
                    if (pbdate != defaultDate || cbdate != defaultDate)
                    {
                        if (pbdate != defaultDate && cbdate == defaultDate)
                        {
                            p.bdate = pbdate;
                            p.bdate = p.bdate.AddYears(procreationAge);
                        }
                        else if (pbdate == defaultDate && cbdate != defaultDate)
                        {
                            p.bdate = cbdate;
                            p.bdate = p.bdate.AddYears(-procreationAge);
                        }
                        if (pbdate != defaultDate && cbdate != defaultDate)
                        {
                            //p.bdate = (pbdate + cbdate)/2;
                            p.bdate = pbdate;
                        }

                        Debug.Log("Missing birth date of " + p.GetFullName() + " was estimated " + p.bdate);
                        updated = true;
                    }
                    // neither parents nor childs are known - estimate based on spouse's bdate
                    else if (spbdate != null)
                    {
                        p.bdate = spbdate;  // assume person is of the same age as his oldest spouse
                        updated = true;
                    }
                }

            }
        }

    }

    //Update coordinates from the tam-web tfm graph (After loading GEDCOM and saving as a graph) 
    //Rename .tfm to .json
    public void GetCoordinates(TextAsset tfm_graph)
    {
        string jsonString = tfm_graph.text;

        JObject json = JObject.Parse(jsonString);

        // Get the "nodePositions" property as a JObject
        JObject nodePositions = (JObject)json["nodePositions"];
        
        // Loop through the node positions
        foreach (JProperty nodeProperty in nodePositions.Properties())
        {
            string nodeName = nodeProperty.Name;
            JObject nodeData = (JObject)nodeProperty.Value;

            // Get the x, y, and fixed values
            float x = (float)nodeData["x"] / 50;
            float y = (float)nodeData["y"] / 50;

            foreach (GameObject fam in families)
            {
                if (nodeName == fam.name)
                {
                    Vector3 tfm_pos = new Vector3(x, 0, y);
                    fam.transform.position = tfm_pos;
                }
            }

            foreach (GameObject per in persons)
            {
                if (nodeName == per.name)
                {
                    Vector3 tfm_pos = new Vector3(x, 0, y);
                    per.transform.position = tfm_pos;
                }
            }
        }
    }

    //Set peson parents for the persons
    public void SetParents()
    {
        foreach (GameObject fam in families)
        {
            FamilyProps fam_props = fam.GetComponent<FamilyProps>();
            foreach (GameObject child in fam_props.children)
            {
                PersonProps child_props = child.GetComponent<PersonProps>();
                child_props.mother = fam_props.wife;
                child_props.father = fam_props.husband;
            }
        }
    }

    //Create Links from persons to families
    public void CreateLinks()
    {
        GameObject lin = new GameObject("Links");

        foreach (GameObject fam in families)
        {

            FamilyProps fam_props = fam.GetComponent<FamilyProps>();

            GameObject wife = fam_props.wife;
            if (wife != null)
            {
                GameObject link = Instantiate(linkPrefab, lin.transform);
                link.name = fam.name + "to" + wife.name;
                LinkProps link_props = link.GetComponent<LinkProps>();
                link_props.source_object = wife;
                link_props.target_object = fam;
                LineRenderer lineRenderer = link.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, wife.transform.position);
                lineRenderer.SetPosition(1, fam.transform.position);
                links.Add(link);
            }
            GameObject husband = fam_props.husband;
            if (husband != null)
            {
                GameObject link = Instantiate(linkPrefab, lin.transform);
                link.name = fam.name + "to" + husband.name;
                LinkProps link_props = link.GetComponent<LinkProps>();
                link_props.source_object = husband;
                link_props.target_object = fam;
                LineRenderer lineRenderer = link.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, husband.transform.position);
                lineRenderer.SetPosition(1, fam.transform.position);
                links.Add(link);
            }
        }
    }
}
