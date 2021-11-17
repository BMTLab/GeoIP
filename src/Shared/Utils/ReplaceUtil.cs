using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace Shared.Utils;


[SuppressMessage("Performance", "CA1810", MessageId = "Initialize reference type static fields inline")]
public static class ReplaceUtil<T> where T : notnull
{
    #region Fields
    [PublicAPI]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static readonly PropertyInfo[] Properties;

    [PublicAPI]
    public static readonly Dictionary<PropertyInfo, Action<T, T>> ReplacePropertyHandlers;

    private static readonly Action<T, T> ReplaceFunc;
    private static readonly Func<T, T, List<PropertyInfo>> DetectChangedFunc;
    #endregion _Fields


    static ReplaceUtil()
    {
        Properties = GetPropertiesForReplacement();
        ReplacePropertyHandlers = new Dictionary<PropertyInfo, Action<T, T>>(Properties.Length);
        ReplaceFunc = BuildAssignPropertiesExpression().Compile();
        DetectChangedFunc = BuildDetectChangedPropertiesExpression().Compile();
        
        foreach (var property in Properties)
        {
            ReplacePropertyHandlers.Add
            (
                property,
                BuildAssignPropertyExpression(property).Compile()
            );
        }
    }
    
    

    [PublicAPI]
    public static void Replace(T old, T other) =>
        ReplaceFunc(old, other);
    
    
    [PublicAPI]
    public static IReadOnlyList<PropertyInfo> DetectChanged(T old, T other) =>
        DetectChangedFunc(old, other);



    private static PropertyInfo[] GetPropertiesForReplacement() =>
        typeof(T)
           .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
           .Where(p => p.CanWrite && p.CanRead)
           .ToArray();
    

    private static Expression<Action<T, T>> BuildAssignPropertiesExpression()
    {
        var oldParam = Expression.Parameter(typeof(T), "old");
        var newParam = Expression.Parameter(typeof(T), "other");
        
        var block = Expression.Block
        (
            Properties.Select(property => BuildAssignPropertyExpression(oldParam, newParam, property))
        );
        
        return Expression.Lambda<Action<T, T>>(block, oldParam, newParam);
    }
    
    
    private static Expression<Action<T, T>> BuildAssignPropertyExpression(PropertyInfo propertyInfo)
    {
        var oldParam = Expression.Parameter(typeof(T), "old");
        var newParam = Expression.Parameter(typeof(T), "other");
        
        var body = BuildAssignPropertyExpression(oldParam, newParam, propertyInfo);
        
        return Expression.Lambda<Action<T, T>>(body, oldParam, newParam);
    }

    
    private static Expression BuildAssignPropertyExpression(Expression first, Expression second, PropertyInfo property)
    {
        var oldProp = Expression.Property(first, property);
        var newProp = Expression.Property(second, property);
        
        var assignExpr = Expression.Assign(oldProp, newProp).Reduce();
        
        return assignExpr;
    } 
    
    
    private static Expression<Func<T, T, List<PropertyInfo>>> BuildDetectChangedPropertiesExpression()
    {
        var oldParam = Expression.Parameter(typeof(T), "old");
        var newParam = Expression.Parameter(typeof(T), "other");

        var list = Expression.Variable(typeof(List<PropertyInfo>), "list");
        var newListExpr = Expression.New(typeof(List<PropertyInfo>));
        var assignNewListExpr = Expression.Assign(list, newListExpr);
        
        var body = new List<Expression>(Properties.Length + 1) { assignNewListExpr };

        foreach (var property in Properties)
        {
            var oldProp = Expression.Property(oldParam, property);
            var newProp = Expression.Property(newParam, property);
            var isEqualExpr = Expression.NotEqual(oldProp, newProp);
            var propConst = Expression.Constant(property, typeof(PropertyInfo));
            var appendExpr = Expression.Call(list, typeof(List<PropertyInfo>).GetMethod(nameof(List<PropertyInfo>.Add))!, propConst);
            var conditionExpr = Expression.IfThen(isEqualExpr, appendExpr);
            
            body.Add(conditionExpr);
        }
        
        body.Add(list);
        
        var block = Expression.Block
        (
            new[] { list },
            body
        );
        
        return Expression.Lambda<Func<T, T, List<PropertyInfo>>>(block, oldParam, newParam);
    }
    
}