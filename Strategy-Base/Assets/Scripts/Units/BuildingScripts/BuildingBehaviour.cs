using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Factory;
using System;

public class BuildingBehaviour : UnitBehaviour
{
    public Transform flagTransform;
    public void ProcessAbility()
    {
        UnitAbility ability = UnitAbilityFactory.GetUnit(unit.unitName);

        if (ability == null) return;

        ability.Process();
    }

    public void CreateFlag()
    {
        BuildingObject building = unit as BuildingObject;
        // If building is able to produce units create a flag
        if (building.productionList.Count > 0)
        {
            var gameObject = ObjectPooler.Instance.SpawnFromPool("Flag", InputManager.MousePosition, Quaternion.identity);

            EventManager.OnBuildingBought.Invoke(gameObject.GetComponent<SpriteRenderer>(), 0);

            gameObject.GetComponentInChildren<IPlaceableBuilding>().Move();

            Vector2 myPos = transform.position;

            gameObject.GetComponentInChildren<IClampable>().CheckClamp = true;
            gameObject.GetComponentInChildren<IClampable>().DoClamp(myPos);
            flagTransform = gameObject.transform;
        }
    }
}
