using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class PartList : MonoBehaviour
{
    public Transform[] parentModel;
    [SerializeField] GameObject partNamePrefab;
    [SerializeField] GameObject partNameParent;
    [SerializeField] Dictionary<string, GameObject> partDict = new Dictionary<string, GameObject>();
    [SerializeField] string excludeTag;

    public bool useExclude;

    void Awake()
    {
        ResetNameList(parentModel);
        InitializeDictionary(parentModel);
    }

    private void Start()
    {
        if (parentModel == null)
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Model").Length; i++)
            {
                parentModel[i] = GameObject.FindGameObjectsWithTag("Model")[i].transform;
            }
        }
    }

    public void SearchForPart(TMP_InputField input)
    {
        if (input.text != "")
        {
            var parts = partDict.Where(kvp => kvp.Key.Contains(input.text.ToString().ToLower()));
            List<string> partCached = new List<string>();

            float spacing = 0;
            float counter = 0;

            foreach (Transform child in partNameParent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (KeyValuePair<string, GameObject> part in parts)
            {
                if (part.Value.layer != LayerMask.NameToLayer("Selectable"))
                {
                    continue;
                }

                if (useExclude)
                {
                    if (!part.Value.CompareTag(excludeTag))
                    {
                        continue;
                    }
                    else
                    {
                        partCached.Add(part.Key);

                        spacing += -50f;
                        counter++;

                        GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

                        spawnName.GetComponent<TMP_Text>().text = part.Value.name;
                        spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
                        spawnName.GetComponent<JumpToPart>().assignedPart = part.Value;
                        spawnName.name = part.Key;

                        partNameParent.GetComponent<RectTransform>().sizeDelta = new Vector2(partNameParent.GetComponent<RectTransform>().sizeDelta.x, -partNameParent.transform.GetChild(partNameParent.transform.childCount - 1).GetComponent<RectTransform>().localPosition.y + 30);

                    }
                }
                else
                {
                    partCached.Add(part.Key);

                    spacing += -50f;
                    counter++;

                    GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

                    spawnName.GetComponent<TMP_Text>().text = part.Value.name;
                    spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
                    spawnName.GetComponent<JumpToPart>().assignedPart = part.Value;
                    spawnName.name = part.Key;

                    partNameParent.GetComponent<RectTransform>().sizeDelta = new Vector2(partNameParent.GetComponent<RectTransform>().sizeDelta.x, -partNameParent.transform.GetChild(partNameParent.transform.childCount - 1).GetComponent<RectTransform>().localPosition.y + 30);

                }

            }

            
        }
        else
        {
            ResetNameList(parentModel);
        }
        
    }

    public void InitializeDictionary(Transform[] parentModel)
    {
        for (int i = 0; i < parentModel.Length; i++)
        {
            foreach (Transform child in parentModel[i])
            {
                if (partDict.ContainsKey(child.name))
                {
                    continue;
                }
                else
                {
                    if (child.childCount > 0)
                    {
                        for (int j = 0; j < child.childCount; j++)
                        {
                            Debug.Log(child.GetChild(j).name);
                            partDict.Add(child.GetChild(j).name.ToLower(), child.GetChild(j).gameObject);
                        }
                    }
                    else
                    {
                        partDict.Add(child.name.ToLower(), child.gameObject);
                    }
                    
                }
            }
        }
        //partDict = new Dictionary<string, GameObject>();
        
    }


    public void ResetNameList(Transform[] parentModel)
    {
        foreach (Transform child in partNameParent.transform)
        {
            Destroy(child.gameObject);
        }

        float spacing = 0;
        float counter = 0;

        for (int i = 0; i < parentModel.Length; i++) 
        {
            foreach (Transform child in parentModel[i])
            {
                if (child.gameObject.layer != LayerMask.NameToLayer("Selectable"))
                {
                    continue;
                }

                if (useExclude)
                {
                    if (!child.CompareTag(excludeTag) && child.gameObject.layer == LayerMask.NameToLayer("Selectable"))
                    {
                        continue;
                    }
                    else
                    {

                        if (child.childCount > 0)
                        {
                            for (int j = 0; j < child.childCount; j++)
                            {
                                spacing += -50f;
                                counter++;

                                GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

                                spawnName.GetComponent<TMP_Text>().text = child.GetChild(j).name;
                                spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
                                spawnName.GetComponent<JumpToPart>().assignedPart = child.GetChild(j).gameObject;

                                spawnName.name = child.GetChild(j).name;
                            }
                        }        
                        else
                        {
                            spacing += -50f;
                            counter++;

                            GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

                            spawnName.GetComponent<TMP_Text>().text = child.name;
                            spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
                            spawnName.GetComponent<JumpToPart>().assignedPart = child.gameObject;

                            spawnName.name = child.name;
                        }
                        
                        
                    }
                }
                else
                {
                    if (child.childCount > 0)
                    {
                        for (int j = 0; j < child.childCount; j++)
                        {
                            spacing += -50f;
                            counter++;

                            GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

                            spawnName.GetComponent<TMP_Text>().text = child.GetChild(j).name;
                            spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
                            spawnName.GetComponent<JumpToPart>().assignedPart = child.GetChild(j).gameObject;

                            spawnName.name = child.GetChild(j).name;
                        }
                    }
                    else
                    {
                        spacing += -50f;
                        counter++;

                        GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

                        spawnName.GetComponent<TMP_Text>().text = child.name;
                        spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
                        spawnName.GetComponent<JumpToPart>().assignedPart = child.gameObject;

                        spawnName.name = child.name;
                    }
                }
                
            }
        }

        partNameParent.GetComponent<RectTransform>().sizeDelta = new Vector2(partNameParent.GetComponent<RectTransform>().sizeDelta.x, -partNameParent.transform.GetChild(partNameParent.transform.childCount - 1).GetComponent<RectTransform>().localPosition.y + 30);

    }

    public void ResetNameListOnButton()
    {
        ResetNameList(parentModel);
    }

}
