using TbsFramework.Cells;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_Script : Square
    {
    // How big is the tile (16x16 w/Pixel per unit set to 16)
    Vector3 dimensions = new Vector3(2.6f, 2.6f, 6f);

    public string TileType;
    public int DefenseBoost;

    public override Vector3 GetCellDimensions()
        {
            return GetComponent<Renderer>().bounds.size;
        }

        public override void MarkAsHighlighted()
        {
            GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
        }

        public override void MarkAsPath()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }

        public override void MarkAsReachable()
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }

        public override void UnMark()
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

