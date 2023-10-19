using UnityEngine;

public class JumpToPart : MonoBehaviour
{
    [SerializeField] CameraControls camControl;
    [SerializeField] BasicInteractions basicInteractions;
    [SerializeField] public GameObject assignedPart;

    private void Start()
    {
        camControl = Camera.main.GetComponent<CameraControls>();
        basicInteractions = GameObject.FindGameObjectWithTag("GameController").GetComponent<BasicInteractions>();
    }

    public void GoToPart()
    {
        if (!basicInteractions.isolateCheck)
        {
            camControl.target = assignedPart.transform;
            camControl.stopRecentering = false;
            basicInteractions.SelectPart(assignedPart.transform);
        }
        

    }
}
