using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private List<RectTransform> imgRectTransforms = new List<RectTransform>();
    private Grid<PathNode> Grid => GridManager.MainGrid;

    private void OnEnable()
    {
        EventManager.OnGridMove.AddListener(CheckIfPlaceable);
        EventManager.OnBuildingPlace.AddListener(PlaceOnGrid);
    }
    private void OnDisable()
    {
        EventManager.OnGridMove.RemoveListener(CheckIfPlaceable);
        EventManager.OnBuildingPlace.RemoveListener(PlaceOnGrid);
    }

    private void CheckIfPlaceable(BuildingMovement building, Vector2 size)
    {
        if (CheckIfOnCanvas())
        {
            EventManager.OnUnavailableEvent.Invoke();
            return;
        }

        Corner corner = new Corner(building.unitCollider, building.unitTransform);
        int gridX, gridY;
        Grid.GetXY(corner.CornerPos + Vector3.one * 0.1f + Vector3.up * 0.5f, out gridX, out gridY);

        for (int x = gridX; x < gridX + size.x; x++)
        {
            for (int y = gridY; y < gridY + size.y; y++)
            {
                // Checks if Path Node is occupied 
                if (Grid.GetGridObject(x, y).occupied)
                {
                    EventManager.OnUnavailableEvent.Invoke();
                    return;
                }
            }
        }

        EventManager.OnPlaceableEvent.Invoke();

    }

    private void PlaceOnGrid(BuildingMovement building, Vector2 size, int i)
    {
        if (!building) return;

        Corner corner = new Corner(building.unitCollider, building.unitTransform);
        int gridX, gridY;
        Grid.GetXY(corner.CornerPos + Vector3.one * 0.1f + Vector3.up * 0.5f, out gridX, out gridY);

        // Sets nodes to occupied for building size
        for (int x = gridX; x < gridX + size.x; x++)
        {
            for (int y = gridY; y < gridY + size.y; y++)
            {
                Grid.GetGridObject(x, y).occupied = true;
            }
        }
    }

    private bool CheckIfOnCanvas()
    {
        // If mouse is on top a canvas building is not able to be placed
        foreach (var imgRectTransform in imgRectTransforms)
        {
            Vector2 localMousePosition = imgRectTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (imgRectTransform.rect.Contains(localMousePosition))
            {
                return true;
            }
        }

        return false;
    }
}
