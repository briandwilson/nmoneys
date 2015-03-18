# Third Party Serialization #
Serialization for third party libraries (those not included in the .NET framework) is supported via separate NuGet packages (one per library) that provide the source code needed to customize the serialization process.
> NOTE: the process and artifacts needed is different for each library. A different section will be provided for each library.

## Json.NET ##

![https://nmoneys.googlecode.com/svn/wiki/NMoneys_JsonNET_Nuget.png](https://nmoneys.googlecode.com/svn/wiki/NMoneys_JsonNET_Nuget.png)

Once the package is installed, there are available three custom converters
  * `CanonicalMoneyConverter`
  * `DefaultMoneyConverter`
  * `NumericalMoneyConverter`
Each converter can be configured as per _Json.NET_ configuration instructions:
  * pass the converter as an optional parameter to either`.SerializeObject()`or `.DeserializeObject()` static methods of `JsonConvert`
  * Use the [.Converters](http://james.newtonking.com/json/help/index.html?topic=html/P_Newtonsoft_Json_JsonSerializerSettings_Converters.htm) collection of a `JsonSerializerSettings` instance that will be passed to any of the aforementioned methods

If _Json-NET_ is used by a framework/library, refer to that library documentation on how to customize serialization/deserialization. Or you can drop a line in the discussion group in case someone can give a hand.

### CanonicalMoneyConverter ###
This converter processes the same input/output as the built-in JSON serialization of the library when using the built-in `DataContractJsonSerializer`: an _Amount_ numeric property and a _Currency_ object with a three-letter code (by default) _IsoCode_ property.

Property casing must be configured outside this converter, for example, using a different implementation of `DefaultContractResolver`.

```
{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}
```

### DefaultMoneyConverter ###
This converter produces a standard representation of a monetary quantity: an _Amount_ numeric property and a _Currency_ code that can be serialized either as a string (the default or providing `CurrencyStyle.Alphabetic` to the converter constructor) or as a number (providing `CurrencyStyle.Numeric` to the converter constructor).

Property casing must be configured outside this converter, for example, using a different implementation of `DefaultContractResolver`.

```
{"Amount" : 123.4, "Currency" : "XXX"}
{"amount" : 123.4, "currency" : 999}
```

### CurrencyLessMoneyConverter ###
This converter is a deserializer designed to provide some limited support to legacy integration processes in which a numeric property is included, but no currency is specified in the input, but a default currency (`XXX` by default) can be provided at the time of instantiating the converter.

## ServiceStack ##
![https://nmoneys.googlecode.com/svn/wiki/NMoneys_ServiceStack_Nuget.png](https://nmoneys.googlecode.com/svn/wiki/NMoneys_ServiceStack_Nuget.png)

_ServiceStack_ follows a completely different configuration model than _Json.NET_. In order to customize how a type needs to be serialized/deserialized a pair of functions is provided to the configurator, via `JsConfig<Money>.RawSerializationFx` and `JsConfig<Money>.RawDeserializationFx`.

The functions that can be provided are the ones defined in the static `.Serialize()` and `.Deserialize()` methods in the following classes:
  * `CanonicalMoneySerializer`
  * `DefaultMoneySerializer`
  * `CurrencyLessMoneySerializer`

### CanonicalMoneySerializer ###
Provides functions to serialize and deserialize the same input/output as the built-in JSON serialization of the library when using the built-in `DataContractJsonSerializer`: an _Amount_ numeric property and a _Currency_ object with a three-letter code (by default) _IsoCode_ property.

Property casing must be configured outside this serializer using, for instance, `JsConfig.EmitCamelCaseNames`.

```
{"Amount" : 123.4, "Currency" : {"IsoCode" : "XXX"}}
```

### DefaultMoneySerializer ###

Provides functions to serialize and deserialize the same input/output as a standard serializer would do with a monetary quantity: an _Amount_ numeric property and an alphabetical _Currency_ code.

The currency code can be processed either as a three-letter code (using `DefaultMoneySerializer` methods) or as a numeric code (using `DefaultMoneySerializer.Numeric` methods).

Property casing must be configured outside this serializer using, for instance, `JsConfig.EmitCamelCaseNames`.

```
{"Amount" : 123.4, "Currency" : "XXX"}
{"amount" : 123.4, "currency" : 999}
```

### CurrencyLessMoneySerializer ###

Defines a function designed to provide some limited support to legacy integration processes in which a numeric property is included, but no currency is specified in the input, but a default currency (`XXX`) can be used.

Due to the functional style of configuration, no other default currency can be specified. But source code is easy enough to modify.

## RavenDB ##

![https://nmoneys.googlecode.com/svn/wiki/NMoneys_RavenDB_Nuget.png](https://nmoneys.googlecode.com/svn/wiki/NMoneys_RavenDB_Nuget.png)

_RavenDB_ uses its own "internalized" version of _Json.NET_ under its own namespace. The very same converters (`CanonicalMoneyConverter`, `DefaultMoneyConverter` and `NumericalMoneyConverter`) are included in the package and their usage is exactly the same.

The only difference occurs when configuring the converters, as serialization happens not by invoking a method on `JsonConvert`. In order to use any of the provided converters, they can be added to the `.Converters` collection from the `JsonSerializer` instance configurable via the `docStoreInstance.Conventions.CustomizeJsonSerializer` delegate. For instance:

```
var store = new DocumentStore() { /* initialize the store */ };
store.Conventions.CustomizeJsonSerializer = serializer => serializer.Converters.Add(new DefaultMoneyConverter());
```