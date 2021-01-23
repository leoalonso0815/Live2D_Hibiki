using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour
{

    public void ZoomInFunc()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        //transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f, transform.localScale.z + 0.1f);
        //Debug.Log("downZoomIN");
    }

    public void ZoomOutFunc()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        //transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
        //Debug.Log("downZoomIN");
    }

}
