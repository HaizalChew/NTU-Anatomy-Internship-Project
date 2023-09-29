using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PartList : MonoBehaviour
{
    public Transform parentModel;
    [SerializeField] GameObject partNamePrefab;
    [SerializeField] GameObject partNameParent;
    [SerializeField] Dictionary<string, GameObject> partDict = new Dictionary<string, GameObject>();

    void Awake()
    {
        ResetNameList();
        InitializeDictionary();
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
            ResetNameList();
        }
        
    }

    public void InitializeDictionary()
    {
        partDict = new Dictionary<string, GameObject>();
        foreach (Transform child in parentModel)
        {
            partDict.Add(child.name.ToLower(), child.gameObject);
        }
    }

    public void ResetNameList()
    {
        foreach (Transform child in partNameParent.transform)
        {
            Destroy(child.gameObject);
        }

        float spacing = 0;
        float counter = 0;

        foreach (Transform child in parentModel)
        {
            spacing += -50f;
            counter++;

            GameObject spawnName = Instantiate(partNamePrefab, partNameParent.transform);

            spawnName.GetComponent<TMP_Text>().text = child.name;
            spawnName.GetComponent<RectTransform>().localPosition = new Vector3(0, spacing, 0);
            spawnName.GetComponent<JumpToPart>().assignedPart = child.gameObject;
            spawnName.name = child.name;
        }

        partNameParent.GetComponent<RectTransform>().sizeDelta = new Vector2(partNameParent.GetComponent<RectTransform>().sizeDelta.x, -partNameParent.transform.GetChild(partNameParent.transform.childCount - 1).GetComponent<RectTransform>().localPosition.y + 30);

    }

}
