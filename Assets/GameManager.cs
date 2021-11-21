using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Board;
    public GameObject Tile;

    public float zoomSpeed = 0.0f;

    private bool first_press = true;
    private Vector3 pivot;
    private Vector3 start_pos;

    private Vector3 start_drag_pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = Board.transform.localScale + Vector3.one * Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
        if (scale.magnitude > Vector3.one.magnitude / 10 && scale.magnitude < Vector3.one.magnitude * 5)
        {
            Board.transform.localScale = scale;
        }

        if (Input.GetMouseButton(2))
        {
            if (first_press)
            {
                first_press = false;
                pivot = Input.mousePosition;
                start_pos = Board.transform.position;
            }


            Board.transform.position = start_pos + Input.mousePosition - pivot;
        } else
        {
            first_press = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            print("drag");
            start_drag_pos = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonUp(0))
        {
            print("make area");

            Vector3 end_drag_pos = Input.mousePosition;

            float scale_test = Board.transform.localScale.x;

            print((start_drag_pos.y - end_drag_pos.y) / (100 * scale_test));
            print((start_drag_pos.x - end_drag_pos.x) / (100 * scale_test));

            for (int y = 0; y < Mathf.Round((start_drag_pos.y - end_drag_pos.y) / (100 * scale_test)) + 1; y++)
            {
                for (int x = 0; x < Mathf.Round((start_drag_pos.x - end_drag_pos.x) / (100 * scale_test)) + 1; x++)
                {
                    GameObject currentTile = Instantiate(Tile, Board.transform);
                    currentTile.transform.position = new Vector3(Mathf.Round((start_drag_pos.x) / (100 * scale_test) - x) * 100 * scale_test, Mathf.Round((start_drag_pos.y) / (100 * scale_test) - y) * 100 * scale_test);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            GameObject currentTile = Instantiate(Tile, Board.transform);

            float scale_test = Board.transform.localScale.x;
            currentTile.transform.position = new Vector3(Mathf.Round(Input.mousePosition.x/(100* scale_test)) * 100 * scale_test, Mathf.Round(Input.mousePosition.y / (100* scale_test)) * 100 * scale_test);
            //currentTile.transform.parent = Board.transform;
        }
    }
}
