using UnityEngine;

public class RandomiseCharacters : MonoBehaviour
{
    [SerializeField] GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        int randInt = Random.Range(0, objects.Length);
        
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        objects[randInt].SetActive(true);
    }

}
