# Kiota Json Serialization Library for dotnet

The Json Serialization Library for dotnet is the dotnet JSON serialization library implementation with System.Text.Json

A [Kiota](https://github.com/microsoft/kiota) generated project will need a reference to a json serialization package to handle json payloads from an API endpoint.

Read more about Kiota [here](https://github.com/microsoft/kiota/blob/main/README.md).

## Using the Kiota Json Serialization Library

```shell
dotnet add package Microsoft.Kiota.Serialization.Json
```

## Using Date and Time Converters with System.Text.Json

The library provides `DateJsonConverter` and `TimeJsonConverter` to enable serialization and deserialization of Kiota's `Date` and `Time` types when using `System.Text.Json.JsonSerializer` directly (outside of Kiota's serialization infrastructure).

### Option 1: Using DefaultOptionsWithConverters

The simplest approach is to use the pre-configured options with converters:

```csharp
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Serialization.Json;
using System.Text.Json;

var date = new Date(2025, 10, 24);
var json = JsonSerializer.Serialize(date, KiotaJsonSerializationContext.DefaultOptionsWithConverters);
// Output: "2025-10-24"

var deserialized = JsonSerializer.Deserialize<Date>(json, KiotaJsonSerializationContext.DefaultOptionsWithConverters);
// deserialized.Year == 2025, deserialized.Month == 10, deserialized.Day == 24
```

### Option 2: Manually Registering Converters

You can also manually register the converters with your own `JsonSerializerOptions`:

```csharp
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Serialization.Json;
using System.Text.Json;

var options = new JsonSerializerOptions();
options.Converters.Add(new DateJsonConverter());
options.Converters.Add(new TimeJsonConverter());

var time = new Time(10, 18, 54);
var json = JsonSerializer.Serialize(time, options);
// Output: "10:18:54"

var deserialized = JsonSerializer.Deserialize<Time>(json, options);
// deserialized.Hour == 10, deserialized.Minute == 18, deserialized.Second == 54
```

This is particularly useful when integrating with third-party libraries that serialize/deserialize Kiota-generated models but cannot use Kiota's serialization infrastructure.

## Debugging

If you are using Visual Studio Code as your IDE, the **launch.json** file already contains the configuration to build and test the library. Otherwise, you can open the **Microsoft.Kiota.Serialization.Json.sln** with Visual Studio.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit [https://cla.opensource.microsoft.com](https://cla.opensource.microsoft.com).

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
