using UnityEngine;

public class ImportCustomModel : MonoBehaviour
{
    public string finalPath;

    public void LoadFile()
    {
        string fileType = NativeFilePicker.ConvertExtensionToFileType(".fbx");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("Operation Cancelled");
            }
            else
            {
                finalPath = path;
                Debug.Log("Picked File: " + finalPath);
            }
        }, new string[] { fileType });
    }


    //IEnumerator LoadImport()
    //{
    //    WWW www = new WWW(finalPath);

    //    while (!www.isDone)
    //    {
    //        yield return null;
    //    }
        
    //    //Scene newScene = SceneManager.CreateScene(finalPath);
    //    //SceneManager.LoadSceneAsync(newScene.name);
    //}
}
