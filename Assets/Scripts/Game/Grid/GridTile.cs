using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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

    private ShapeTile _collidedShapeTile;

    void Awake()
    {
        isVisible = false;
        isInSample = false;
    }

    void OnEnable()
    {
    }
    void OnDisable()
    {
    }
    public bool CanUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }

    public void SwitchShapeVisibility()
    {
        hooverImage.gameObject.SetActive(false);
        if (isVisible)
        {
            isVisible = false;
            _collidedShapeTile.gameObject.SetActive(false);
            // visibleImage.gameObject.SetActive(false);
        }
        else
        {
            isVisible = true;
            _collidedShapeTile.gameObject.SetActive(true);
            // visibleImage.gameObject.SetActive(true);
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
        _collidedShapeTile = collision.GetComponent<ShapeTile>();
        float collided_shape_tile_rot_z = collision.GetComponent<ShapeTile>().GetComponent<RectTransform>().rotation.z;
        if (this.GetComponent<RectTransform>().rotation.z == _collidedShapeTile.GetComponent<RectTransform>().rotation.z)
        {
            isHoover = true;
            hooverImage.gameObject.SetActive(true);
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        _collidedShapeTile = collision.GetComponent<ShapeTile>();
        if (this.GetComponent<RectTransform>().rotation.z == _collidedShapeTile.GetComponent<RectTransform>().rotation.z)
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
