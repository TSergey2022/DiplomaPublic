using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Craft.CraftLib;

namespace Craft.Requirements {
  public static class RequirementResolver {
    // Словарь, хранящий связи между типом и функцией, создающей требование
    private static readonly Dictionary<string, Func<JObject, IItemRequirement>> Resolvers = new();

    // Статический конструктор для регистрации стандартных типов требований
    static RequirementResolver() {
      // Регистрация стандартных типов требований
      Register(AndRequirement.TypeValue, (json) => AndRequirement.FromJObject(json, Resolve));
      Register(OrRequirement.TypeValue, (json) => OrRequirement.FromJObject(json, Resolve));
      Register(IdRequirement.TypeValue, IdRequirement.FromJObject);
      Register(TagRequirement.TypeValue, TagRequirement.FromJObject);
      Register(QuantityRequirement.TypeValue, (json) => QuantityRequirement.FromJObject(json, Resolve));
    }

    // Метод для регистрации новых типов требований
    private static void Register(string type, Func<JObject, IItemRequirement> resolver) {
      if (string.IsNullOrEmpty(type))
        throw new ArgumentException("Тип не может быть пустым", nameof(type));

      // Если тип уже зарегистрирован, выбрасываем исключение
      if (Resolvers.ContainsKey(type))
        throw new InvalidOperationException($"Тип '{type}' уже зарегистрирован.");

      // Регистрируем новый обработчик для типа
      Resolvers[type] = resolver ?? throw new ArgumentNullException(nameof(resolver));
    }

    // Метод для разрешения требования по типу
    public static IItemRequirement Resolve(JObject json) {
      var type = json[IItemRequirement.TypeMetadataKey]?.ToString();

      if (string.IsNullOrEmpty(type)) {
        throw new InvalidOperationException("Не указан тип требования.");
      }

      // Проверяем, существует ли регистратор для указанного типа
      if (!Resolvers.TryGetValue(type, out var resolver)) {
        throw new InvalidOperationException($"Неизвестный тип требования: {type}");
      }

      // Вызываем зарегистрированный обработчик
      return resolver(json);
    }
  }
}
