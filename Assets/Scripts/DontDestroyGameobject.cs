using UnityEngine;

public class DontDestroyGameobject : MonoBehaviour
{
    public static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
