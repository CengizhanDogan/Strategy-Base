using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Factory
{
    public static class UnitAbilityFactory
    {
        private static Dictionary<string, Type> unitAbilitiesByName;
        private static bool IsInitialized => unitAbilitiesByName != null;

        private static void InitializeFactory()
        {
            if (IsInitialized) return;

            // Gets all Unit Abilities and adds to dictionary
            var unitTypes = Assembly.GetAssembly(typeof(UnitAbility)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(UnitAbility)));

            unitAbilitiesByName = new Dictionary<string, Type>();

            foreach(var type in unitTypes)
            {
                var tempUnit = Activator.CreateInstance(type) as UnitAbility;
                unitAbilitiesByName.Add(tempUnit.Name, type);
            }
            //
        }

        public static UnitAbility GetUnit(string unitType)
        {
            InitializeFactory();

            if (unitAbilitiesByName.ContainsKey(unitType))
            {
                Type type = unitAbilitiesByName[unitType];
                var unit = Activator.CreateInstance(type) as UnitAbility;
                return unit;
            }

            return null;
        }
    }
}
