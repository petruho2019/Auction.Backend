using AutoMapper;
using System.Reflection;

namespace Auction.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly)
            => ApplyMethodFromAssembly(assembly);

        private void ApplyMethodFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType is true && i.GetGenericTypeDefinition() == typeof(IMapWith<>)));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, [this]);
            }
        }
        
    }
}
