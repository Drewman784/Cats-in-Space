using TbsFramework.Cells;
using UnityEngine;

    public class Square_Script : Square
    {
        // How big is the tile (16x16 w/Pixel per unit set to 10)
        Vector3 dimensions = new Vector3(1.6f, 1.6f, 0f);

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

