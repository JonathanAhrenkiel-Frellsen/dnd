using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Board;
    public GameObject Tile;

    Vector3 board_start_pos;

    public float zoomSpeed = 0.0f;

    private bool first_press = true;
    private Vector3 pivot;
    private Vector3 start_pos;

    private Vector3 start_drag_pos = Vector3.zero;

    private float tile_size = 10.0f;


    private Vector3 prevCoordinate;

    // Start is called before the first frame update
    void Start()
    {
        board_start_pos = Board.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 zoom = Board.transform.position + Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
        if (zoom.magnitude > board_start_pos.magnitude / 5 && zoom.magnitude < board_start_pos.magnitude * 5)
        {
            Board.transform.position = zoom;
        }

        if (Input.GetMouseButton(2))
        {
            if (first_press)
            {
                first_press = false;

                RaycastHit hit1;
                var ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray1, out hit1))
                {
                    pivot = hit1.point;
                    start_pos = Board.transform.position;
                }
            }

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Board.transform.position = start_pos + hit.point - pivot;
            }
        } else
        {
            first_press = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            print("drag");
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                start_drag_pos = hit.point;
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonUp(0))
        {
            print("make area");

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 end_drag_pos = hit.point;

                float y_start = 0;
                float y_end = Mathf.Round(Mathf.Abs(start_drag_pos.y - end_drag_pos.y) / tile_size) + 1;

                float x_start = 0;
                float x_end = Mathf.Round(Mathf.Abs(start_drag_pos.x - end_drag_pos.x) / tile_size) + 1;

                if (start_drag_pos.y < end_drag_pos.y && start_drag_pos.x < end_drag_pos.x)
                {
                    y_start = -Mathf.Round(Mathf.Abs(start_drag_pos.y - end_drag_pos.y) / tile_size);
                    y_end = 1;

                    x_start = -Mathf.Round(Mathf.Abs(start_drag_pos.x - end_drag_pos.x) / tile_size);
                    x_end = 1;
                } else if (start_drag_pos.y > end_drag_pos.y && start_drag_pos.x < end_drag_pos.x)
                {
                    y_start = 0;
                    y_end = Mathf.Round(Mathf.Abs(start_drag_pos.y - end_drag_pos.y) / tile_size) + 1;

                    x_start = -Mathf.Round(Mathf.Abs(start_drag_pos.x - end_drag_pos.x) / tile_size);
                    x_end = 1;
                } else if (start_drag_pos.y < end_drag_pos.y && start_drag_pos.x > end_drag_pos.x)
                {
                    y_start = -Mathf.Round(Mathf.Abs(start_drag_pos.y - end_drag_pos.y) / tile_size);
                    y_end = 1;

                    x_start = 0;
                    x_end = Mathf.Round(Mathf.Abs(start_drag_pos.x - end_drag_pos.x) / tile_size) + 1;
                }

                for (float y = y_start; y < y_end; y++)
                {
                    for (float x = x_start; x < x_end; x++)
                    {
                        GameObject currentTile = Instantiate(Tile, Board.transform);
                        currentTile.transform.localPosition = new Vector3(Mathf.Round((start_drag_pos.x - Board.transform.position.x) / tile_size - x) * tile_size, Mathf.Round((start_drag_pos.y - Board.transform.position.y) / tile_size - y) * tile_size, 0);
                    }
                }
            }

            start_drag_pos = Vector3.zero;
        }
        else if (Input.GetMouseButton(0) && start_drag_pos == Vector3.zero)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Background")
                {
                    Vector3 currentCoordinate = new Vector3(Mathf.Round((hit.point.x - Board.transform.position.x) / tile_size) * tile_size, Mathf.Round((hit.point.y - Board.transform.position.y) / tile_size) * tile_size, 0);
                    if (currentCoordinate != prevCoordinate)
                    {
                        prevCoordinate = currentCoordinate;

                        GameObject currentTile = Instantiate(Tile, Board.transform);

                        currentTile.transform.localPosition = currentCoordinate;
                    }
                }
            }
        }
    }
}
