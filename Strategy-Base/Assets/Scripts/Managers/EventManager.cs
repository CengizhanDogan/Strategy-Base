using System;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static BuildingBoughtEvent OnBuildingBought = new BuildingBoughtEvent();

    public static GridMoveEvent OnGridMove = new GridMoveEvent();
    public static BuildingPlaceEvent OnBuildingPlace = new BuildingPlaceEvent();
    public static CancelEvent OnCancel = new CancelEvent();

    public static PlaceableEvent OnPlaceableEvent = new PlaceableEvent();
    public static UnavailableEvent OnUnavailableEvent = new UnavailableEvent();

    public static SelectionEvent OnSelectionEvent = new SelectionEvent();
    public static DeselectEvent OnDeselectEvent = new DeselectEvent();

    public static PowerGenerationEvent OnPowerGeneration = new PowerGenerationEvent();
}

public class BuildingBoughtEvent : UnityEvent<SpriteRenderer, int> { }
public class GridMoveEvent : UnityEvent<BuildingMovement, Vector2> { }
public class BuildingPlaceEvent : UnityEvent<BuildingMovement, Vector2, int> { }
public class CancelEvent : UnityEvent { }
public class PlaceableEvent : UnityEvent { }
public class UnavailableEvent : UnityEvent { }
public class SelectionEvent : UnityEvent<UnitBehaviour, Transform> { }
public class DeselectEvent : UnityEvent<bool> { }
public class PowerGenerationEvent : UnityEvent<int> { }
