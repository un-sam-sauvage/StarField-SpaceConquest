using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float _horizontal, _vertical;

    public float speed;
    public float scale = .1f;
    private Rigidbody2D _rb;
    float _orthographicSize;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _orthographicSize = GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _horizontal = -Input.GetAxis("Mouse X");
            _vertical = -Input.GetAxis("Mouse Y");
            _rb.AddForce(new Vector3(_horizontal * Time.deltaTime * speed, _vertical * Time.deltaTime * speed, 0));
        }

        
        _orthographicSize -= Input.mouseScrollDelta.y * scale;
        _orthographicSize = Mathf.Clamp(_orthographicSize, 0.5f, 10);
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, _orthographicSize, Time.deltaTime*5);

    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -10, 10);
        pos.y = Mathf.Clamp(pos.y, -10, 10);
        transform.position = pos;
    }
}