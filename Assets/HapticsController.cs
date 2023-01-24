using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsController : MonoBehaviour
{
    public Camera _camera;
    public Transform _cursorControl;
    public Transform _cursorDisplay;
    public Transform _prevCursorControl;
    public float cd_factor_slope = 1.7f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.visible = false;
        _prevCursorControl = _cursorControl;
    }

    float GetGrayScale(Vector3 pos) {
        // from https://docs.unity3d.com/ScriptReference/RaycastHit-textureCoord.html
        RaycastHit hit;
        if (!Physics.Raycast(_camera.ScreenPointToRay(pos), out hit))
            return 0;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return 0;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        Color color = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        float grayscale = color.grayscale;
        return grayscale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0.85f;
        Vector3 target = _camera.ScreenToWorldPoint(pos);

        Vector3 display_pos = _camera.WorldToScreenPoint(new Vector3(_cursorDisplay.position.x, _cursorDisplay.position.y, 0.85f));
        float grayscale = GetGrayScale(display_pos);
        Vector3 displacement = target - _prevCursorControl.position;
        _cursorControl.transform.position = target;

        // assuming sigmoidal C/D curve
        float cd_factor = 2f / (1f + Mathf.Exp(cd_factor_slope * (grayscale - 0.5f)));
        _cursorDisplay.transform.position += displacement * cd_factor;

        _prevCursorControl = _cursorControl;

        Debug.Log("display: " + display_pos + ", control: " + pos + ", grayscale: " + grayscale + ", cd_factor: " + cd_factor);

    }
}
