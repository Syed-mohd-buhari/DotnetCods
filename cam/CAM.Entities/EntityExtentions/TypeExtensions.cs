using System;

namespace CAM.Entities.EntityExtentions
{
    public static class TypeExtensions
    {
        public static bool IsBaseEntity<TToCompare>(this Type type)
        {
            for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
            {
                if (baseType == typeof(TToCompare))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
