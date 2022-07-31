using HashidsNet;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Binder;

public class HashidsModelBinder : IModelBinder
{
    private readonly IHashids hashids;

    public HashidsModelBinder(IHashids hashids)
    {
        this.hashids = hashids ?? throw new ArgumentNullException(nameof(hashids));
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new System.ArgumentNullException(nameof(bindingContext));

        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        var ids = hashids.DecodeLong(value);

        if (ids.Length == 0)
            return Task.CompletedTask;

        bindingContext.Result = ModelBindingResult.Success(ids.First());

        return Task.CompletedTask;
    }
}
