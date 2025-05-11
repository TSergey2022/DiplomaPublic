using System;
using System.Collections.Generic;
using Craft.CraftLib;
using Newtonsoft.Json.Linq;

namespace Craft.Processors {
  public static class ProcessorResolver {
    private static readonly Dictionary<string, Func<JObject, IItemProcessor>> Resolvers = new();

    static ProcessorResolver() {
      Register("simple", SimpleProcessor.FromJObject);
    }

    private static void Register(string type, Func<JObject, IItemProcessor> resolver) {
      if (string.IsNullOrEmpty(type))
        throw new ArgumentException("Тип не может быть пустым", nameof(type));
      if (Resolvers.ContainsKey(type))
        throw new InvalidOperationException($"Тип '{type}' уже зарегистрирован.");
      Resolvers[type] = resolver ?? throw new ArgumentNullException(nameof(resolver));
    }

    public static IItemProcessor Resolve(JObject json) {
      var type = json[IItemProcessor.TypeMetadataKey]?.ToString();
      if (string.IsNullOrEmpty(type))
        throw new InvalidOperationException("Не указан тип процессора.");
      if (!Resolvers.TryGetValue(type, out var resolver))
        throw new InvalidOperationException($"Неизвестный тип процессора: {type}");
      return resolver(json);
    }
  }
}
