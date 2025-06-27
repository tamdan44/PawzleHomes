using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    public Image hooverImage;
    public Image visibleImage;
    public Image normalImage;
    public Image sampleImage;
    // public List<Sprite> normalImages;
    public bool isHoover { get; set; }
    public bool isVisible { get; set; }
    public bool SquareOccupied { get; set; }
    public Vector3Int TileIndex { get; set; }

    public bool isInSample { get; set; }

    void Awake()
    {

    }

    void Start()
    {
        isVisible = false;
        isInSample = false;
    }

    public bool CanUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnBoard()
    {
        hooverImage.gameObject.SetActive(false);
        if (isVisible)
        {
            isVisible = false;
            visibleImage.gameObject.SetActive(false);
        }
        else
        {
            isVisible = true;
            visibleImage.gameObject.SetActive(true);
        }
    }


    public void SetThisTileAsSample()
    {
        if (isInSample)
        {
            normalImage.gameObject.SetActive(false);
            sampleImage.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter");
        float collided_shape_tile_rot_z = collision.GetComponent<ShapeTile>().GetComponent<RectTransform>().rotation.z;
        if (this.GetComponent<RectTransform>().rotation.z == collided_shape_tile_rot_z)
        {
            isHoover = true;
            hooverImage.gameObject.SetActive(true);
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        float collided_shape_tile_rot_z = collision.GetComponent<ShapeTile>().GetComponent<RectTransform>().rotation.z;
        if (this.GetComponent<RectTransform>().rotation.z == collided_shape_tile_rot_z)
        {
            isHoover = true;
            hooverImage.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        hooverImage.gameObject.SetActive(false);
        isHoover = false;
    }

}
