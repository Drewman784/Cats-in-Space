using UnityEngine;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Cells.Highlighters;

public class CellHighlighterHighlight : CellHighlighter
{
    public Color HighlightColor = Color.yellow; // Default highlight color
    private Color originalColor;
    private Renderer cellRenderer;

    public override void Apply(Cell cell)
    {
        if (cellRenderer == null)
        {
            cellRenderer = cell.GetComponent<Renderer>();
            if (cellRenderer != null)
            {
                originalColor = cellRenderer.material.color;
            }
        }

        if (cellRenderer != null)
        {
            cellRenderer.material.color = HighlightColor;
        }
    }

    public void ResetHighlight(Cell cell)
    {
        if (cellRenderer != null)
        {
            cellRenderer.material.color = originalColor;
        }
    }
}
