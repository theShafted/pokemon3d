using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities
{
    public static class PreDefinedAssembliesUtils
    {
        enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass
        }

        static AssemblyType? GetAssemblyType(string assembly)
        {
            return assembly switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp=Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null
            };
        }

        public static List<Type> GetTypes(Type interfaceType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<AssemblyType, Type[]> assemblyTypes = new();
            List<Type> types = new();
            foreach (Assembly assembly in assemblies)
            {
                AssemblyType? assemblyType = GetAssemblyType(assembly.GetName().Name);
                if (assemblyType != null)
                    assemblyTypes.Add((AssemblyType) assemblyType, assembly.GetTypes());
            }

            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharp], interfaceType, types);
            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharpFirstPass], interfaceType, types);

            return types;
        }

        private static void AddTypesFromAssembly(Type[] assembly, Type interfaceType, ICollection<Type> types)
        {
            if (assembly == null) return;
            foreach (Type type in assembly)
            {
                if (type != interfaceType && interfaceType.IsAssignableFrom(type)) types.Add(type);
            }
        }
    }
}