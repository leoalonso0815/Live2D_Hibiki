using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour
{

    IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        //Debug.Log(offset);

        while (Input.GetMouseButton(0))
        {
            //Debug.Log("down");
            Vector3 cuScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 cuPosition = Camera.main.ScreenToWorldPoint(cuScreenSpace) + offset;
            transform.position = cuPosition;
            yield return new WaitForFixedUpdate();
        }
    }
}
