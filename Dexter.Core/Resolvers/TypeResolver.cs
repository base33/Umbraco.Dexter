using System;

namespace Dexter.Core.Resolvers
{
    public class TypeResolver
    {
        public static T ResolveTypeAndConstruct<T>(string assembly, string type, params object[] constructorParams)
        {
            var realType = Type.GetType($"{type},{assembly}");

            if (!typeof(T).IsAssignableFrom(realType))
                return default(T);

            return (T)Activator.CreateInstance(realType, constructorParams);
        }

        public static Type ResolveType<T>(string assembly, string type)
        {
            var realType = Type.GetType($"{type},{assembly}");

            if (!typeof(T).IsAssignableFrom(realType))
                return null;

            return realType;
        }
    }
}
