using System.Reflection;

using FluentValidation;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

using Shared.Utils.TypeExtensions;


namespace Shared.ViewModels.Validators.Extensions;

public static class FluentValidationExtensions
{
    [PublicAPI]
    public static IServiceCollection AddLocalizedValidators
        (this IServiceCollection services, string validatorNamespace, string modelNamespace)
    {
        if (!validatorNamespace.IsValid())
            throw new ArgumentNullException(nameof(validatorNamespace));

        if (!modelNamespace.IsValid())
            throw new ArgumentNullException(nameof(modelNamespace));

        var currentAssembly = Assembly.GetExecutingAssembly();
        var classes = currentAssembly.GetTypes()
                                     .Where
                                      (
                                          p =>
                                              !(
                                                  (p.Attributes & TypeAttributes.Abstract) != 0 &&
                                                  (p.Attributes & TypeAttributes.BeforeFieldInit) != 0
                                              ) &&
                                              p.Namespace == validatorNamespace &&
                                              p.Name.Contains(@"Validator", StringComparison.Ordinal)
                                      )
                                     .ToList();


        Assembly? modelAssembly = null;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var value = assembly.GetName().Name;

            if (value != null && value.Contains(modelNamespace, StringComparison.CurrentCultureIgnoreCase))
                modelAssembly = assembly;
        }

        if (modelAssembly is null)
            return services;

        foreach (var t in classes)
        {
            var modelClassName = t.Name.Replace(@"Validator", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            var modelClass = modelAssembly.GetType($"{modelNamespace}.{modelClassName}");

            if (modelClass is null)
                continue;

            var genericType = typeof(IValidator<>).MakeGenericType(modelClass);
            services.AddScoped(genericType, t);
        }

        ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

        return services;
    }
}