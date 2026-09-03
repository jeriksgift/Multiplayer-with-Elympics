using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridChildCellSizeSetter : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup;

    private void Awake()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void OnValidate()
    {
        SetCellSize();
    }

    private void OnRectTransformDimensionsChange()
    {
        SetCellSize();
    }

    private void SetCellSize()
    {
        if (gridLayoutGroup == null)
            gridLayoutGroup = GetComponent<GridLayoutGroup>();

        int childCount = transform.childCount;

        if (childCount == 0)
            return;

        int columnsCount;
        int rowsCount;

        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            columnsCount = gridLayoutGroup.constraintCount;
            rowsCount = Mathf.CeilToInt((float)childCount / columnsCount);
        }
        else if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            rowsCount = gridLayoutGroup.constraintCount;
            columnsCount = Mathf.CeilToInt((float)childCount / rowsCount);
        }
        else
        {
            return;
        }

        RectTransform rect = GetComponent<RectTransform>();

        float availableWidth =
            rect.rect.width
            - gridLayoutGroup.padding.left
            - gridLayoutGroup.padding.right
            - gridLayoutGroup.spacing.x * (columnsCount - 1);

        float availableHeight =
            rect.rect.height
            - gridLayoutGroup.padding.top
            - gridLayoutGroup.padding.bottom
            - gridLayoutGroup.spacing.y * (rowsCount - 1);

        float cellWidth = availableWidth / columnsCount;
        float cellHeight = availableHeight / rowsCount;

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
}