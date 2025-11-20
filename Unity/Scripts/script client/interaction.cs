using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayCard.GravityInverted = !PlayCard.GravityInverted;
        }
    }
}
