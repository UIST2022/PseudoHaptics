using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsController : MonoBehaviour
{
    public Camera _camera;
    public Transform _cursorControl;
    public Transform _cursorDisplay;
    public Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0.85f;
        Vector3 target = _camera.ScreenToWorldPoint(pos);

        int x = Mathf.FloorToInt(transform.position.x / size.x * heightmap.width);
        int z = Mathf.FloorToInt(transform.position.z / size.z * heightmap.height);
        Vector3 pos = transform.position;
        pos.y = heightmap.GetPixel(x, z).grayscale * size.y;
        transform.position = pos;

        _cursorControl.transform.position = _camera.ScreenToWorldPoint(pos);
        _cursorDisplay.transform.position = _cursorControl.transform.position;
    }
}
