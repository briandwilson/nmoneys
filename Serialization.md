# Serialization Considerations #


## Objectives ##
As much as possible, we want both `Currency` and `Money` to be serialized. I definitely see scenarios where I serialize those objects, therefore I add support for serialization.

`Currency` needs to become as close to a singleton as possible. We do so for a number of reasons:
  * Equality is evaluated only over the iso code, assuming all instances are created though the provided creation methods
  * We want ot avoid having two instances of the same currency with different data just because one of them comes from a serialized instance of a previous version of the library


Another goal is store a little information needed in the serialized data. Just enough to be able to rehydrate a `Currency` and `Money` instance with the data and behavior of the current version of the deserializer library.
For a `Currency` object that is the ISO code and for a `Money` object is the currency and the amount.

## Options ##
Serialization is a delicate and complex subject.
On one hand there are several (multiple) serialization models to be considered:
  * Binary serialization
  * SOAP serialization
  * XML serialization
  * XAML serialization
  * Data Contract serialization
  * Data Contract JSON serialization
  * Simple JSON serialization
  * Third party serialization models (Json.Net, ...)

On the other hand each serialization model comes with a set of requirements in terms of code security. For example, to fully control binary (and other models) serialization, _ISerializable_ is the way to go. But it is not supported in _Partial Trust_ environments.

I do not have enough user base with sufficient different usage scenarios to make a more informed choices, so I will take some default decisions that can be changed if we found they collide with the usage of most people.

## Decisions ##
In this version I am commited to support fully customized:
  * binary
  * xml
  * data contract (XML and Json) serialization

SOAP serialization is left out as it's deprecated.
XAML serialization is not supported as, either I do not have enough kwnoledge to meet my requirements or they are not possible to meet with the current version of the language.
"Simple" json serialization (`JavaScriptSerializer`) is left out because there are better (and faster) alternatives for serialzing to JSON.
Third party serialization models are left out of the library as I do not wish to take any non-framework dependency on the library. See [Third-Party Serialization](ThirdPartySerialization.md) for supported libraries.

`ISerializable` model is supported to handle the requirements. If that means that most people can't use the library, we can reconsider that support.