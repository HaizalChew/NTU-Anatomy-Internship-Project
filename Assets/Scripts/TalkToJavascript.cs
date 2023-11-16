using UnityEngine;
using System.Runtime.InteropServices;


public class TalkToJavascript : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void GetInt(int x);

    public Vector2 screenSize;
    // Start is called before the first frame update
    void Start()
    {
        screenSize = new Vector2 (Screen.width, Screen.height); 
    }

    public void detectResChange()
    {
        if (screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            screenSize.x = Screen.width;
            screenSize.y = Screen.height;
            GetInt(1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        detectResChange();
    }
}
