using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;

public class UnitMover : MonoBehaviour
{
    private Pathfinding pathfinding;

    private Vector2 objectGrid;
    private GameObject moveObject;

    #region Singleton
    public static UnitMover Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    void Start()
    {
        pathfinding = new Pathfinding();
    }

    void Update()
    {
        MovementInput();
    }

    public void StartMovement(GameObject moveObject, Transform flag)
    {
        if (!moveObject) return;
        this.moveObject = moveObject;

        GridManager.MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
        Vector2 objectGrid = new Vector2(x, y);

        GridManager.MainGrid.GetXY(flag.position, out var z, out var t);
        List<PathNode> path = pathfinding.FindPath((int)objectGrid.x, (int)objectGrid.y, z, t, false);

        Move(path, 1);
    }

    private void MovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastManager ray = new RaycastManager();

            moveObject = ray.SelectedObject;
            Debug.Log(moveObject);
            if (!moveObject) return;
            GridManager.MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
            objectGrid = new Vector2(x, y);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!moveObject) return;
            if (moveObject.TryGetComponent(out BuildingBehaviour b)) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetXY(new Vector2(mousePos.x, mousePos.y) + Vector2.up * 0.5f, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath((int)objectGrid.x, (int)objectGrid.y, x, y, true);
            Move(path, 1);
        }
    }

    void Move(List<PathNode> path, int i)
    {
        if (path != null)
        {
            Vector2 gridPos = GridManager.MainGrid.GetWorldPosition(path[i].x, path[i].y);
            moveObject.transform.DOMove(gridPos, 7).SetSpeedBased().SetEase(Ease.Linear).OnUpdate(() =>
            {
                GridManager.MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
                objectGrid = new Vector2(x, y);
            }).OnComplete(() =>
            {
                if (path.Count > i + 2) Move(path, i + 1);
            });
        }
    }
}