# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.9.7] - 2024-06-28

- Add helper methods to parse enums from strings.

## [1.9.6] - 2024-06-12

- Add `IEnumerable<T>` extension methods to remove LINQ dependency from generated code.

### Added

## [1.9.5] - 2024-06-03

### Added

- Kiota's `Date` and `Time` types are interchangeable with the system `DateOnly` and `TimeOnly` types through implicit conversion operators.

## [1.9.4] - 2024-05-31

### Changed

- Fix MultipartBody serialization

## [1.9.3] - 2024-05-28

### Changed

- Fix time formatting for other cultures.

## [1.9.2] - 2024-05-24

### Changed

- Removes LINQ usages from product code.

## [1.9.1] - 2024-05-13

### Changed

- Removes GetAwaiter() call in KiotaSerializer.GetStringFromStream and makes it fully synchronous.

## [1.9.0] - 2024-05-06

Adds asynchronous deserialization support and marks synchronous as obsolete. https://github.com/microsoft/kiota-abstractions-dotnet/issues/223

### Added

- Added asynchronous deserialization methods (to KiotaJsonSerializer.Deserialization and KiotaSerializer.Deserialization).
- Added IAsyncParseNodeFactory interface to provide asynchronous version of GetRootParseNode: GetRootParseNodeAsync.
- Added ParseNodeFactoryRegistry.GetRootParseNodeAsync method.
- Added ParseNodeProxyFactory.GetRootParseNodeAsync method
- Adds async overloads for serialization helpers

### Changed

- Marked synchronous deserialization methods as obsolete.
- Marked IParseNodeFactory.GetRootParseNode as obsolete.
- Refactored ParseNodeFactoryRegistry.GetFactory to support both asynchronous (IAsyncParseNodeFactory) and synchronous (IParseNodeFactory) factories.


## [1.8.4] - 2024-04-19

- Bumps Std.UriTemplate to version 0.0.57

## [1.8.3] - 2024-04-18

- Have set the license expression on the nuget package rather than bundling in a file (<https://github.com/microsoft/kiota-abstractions-dotnet/issues/219>)

## [1.8.2] - 2024-04-18

- Have made System.Diagnostics.DiagnosticSource only be included on Net Standard's TFM & net 5 (<https://github.com/microsoft/kiota-abstractions-dotnet/issues/218>)

## [1.8.1] - 2024-03-26

### Changed

- `MultipartBody` now supports an optional `fileName` parameter to specify the file name of the part. (<https://github.com/microsoft/kiota-abstractions-dotnet/issues/212>)

## [1.8.0] - 2024-03-18

### Added

- Added support for untyped nodes. (<https://github.com/microsoft/kiota-abstractions-dotnet/issues/175>)

## [1.7.12] - 2024-03-08

### Changed

- Upgrade `Std.UriTemplate` NuGet package to version 0.54.
- Add a unit test for emoji in URI template parameters.

## [1.7.11] - 2024-02-26

### Changed

- Updated IParseNode enum methods `DynamicallyAccessedMembersAttribute` to `PublicFields`.
- Fixed AOT compiler warnings from ILC.

## [1.7.10] - 2024-02-26

### Changed

- Added `net6.0` and `net8.0` as target frameworks.

## [1.7.9] - 2024-02-05

### Changed

- Added `DynamicallyAccessedMembers` annotation to `RequestInformation.Configure`.
- Fixes `IsTrimmable` property on the project.

## [1.7.8] - 2024-02-02

### Changed

- Updated `DynamicallyAccessedMembers` annotations for the `WriteCollectionOfEnumValues` method.

## [1.7.7] - 2024-02-01

### Changed

- Fixed AOT trimming warnings the URI template parameters resolution. [microsoft/kiota#4065](https://github.com/microsoft/kiota/issues/4065).

## [1.7.6] - 2024-01-24

### Changed

- Improve AllowedHost validator to throw an error if `https://` or `http://` prefix is present in a allowed host value.(<https://github.com/microsoft/kiota-abstractions-dotnet/issues/165>)

## [1.7.5] - 2024-01-11

### Changed

- Fixes missing query parameters when the parameter values are empty strings.(<https://github.com/microsoft/kiota-abstractions-dotnet/issues/172>)

## [1.7.4] - 2024-01-09

### Changed

- Fixed Method not found error due to conflicting dependencies by updating Std.UriTemplate dependency.
- Fixed unicode characters decoding in URI (<https://github.com/std-uritemplate/std-uritemplate/issues/114>).

## [1.7.3] - 2023-11-30

### Changed

- Fixed an issue where arrays of non-string types passed into the query parameter were not being converted to strings leading to Invalid cast exceptions. [microsoft/kiota#3354](https://github.com/microsoft/kiota/issues/3354)

## [1.7.2] - 2023-11-14

### Added

- Added support for dotnet 8.

## [1.7.1] - 2023-11-13

### Changed

- Fixed an issue where path and query parameters of enum type would not be expanded properly. [microsoft/kiota#3693](https://github.com/microsoft/kiota/issues/3693)

## [1.7.0] - 2023-11-07

### Added

- Added methods in request information to reduce the amount of code being generated.

## [1.6.1] - 2023-11-02

### Changed

- Fixes sanitization of Date and Time values in query and path parameters

## [1.6.0] - 2023-10-31

### Added

- Added helper methods to facilitate serialization and deserialization of models. [microsoft/kiota#3406](https://github.com/microsoft/kiota/issues/3406)

## [1.5.0] - 2023-10-19

### Added

- Added dotnet trimming support.

## [1.4.0] - 2023-10-12

### Added

- Added a method to set the request body content type in request information on binary payloads.

## [1.3.5] - 2023-10-05

### Changed

- Uses headers try add when setting the content type.

## [1.3.4] - 2023-10-04

### Changed

- Added a TryAdd method on the RequestHeaders Dictionary

## [1.3.3] - 2023-09-25

### Changed

- Removed the code that changed the first character of the query parameter name to lower case
- Added sanitization of guid values in query parameters

## [1.3.2] - 2023-09-21

### Changed

- Switched from `Tavis.UriTemplates` to `Std.UriTemplate` for URI template parsing.

## [1.3.1] - 2023-08-08

### Fixed

- Fixed a bug where excess duplicate subscriptions would be created on the same property in the backing store causing performance issues in some scenarios. Related to <https://github.com/microsoftgraph/msgraph-sdk-dotnet/issues/1994>

## [1.3.0] - 2023-08-01

### Added

- Added support for multipart form data request body serialization.

## [1.2.1] - 2023-07-03

### Fixed

- Fixed a bug that caused the uri parameters not to be applied when the Uri template had a different casing than the parameter name that was used to set it.

## [1.2.0] - 2023-06-28

### Added

- Added an interface to mark composed type wrappers and facilitate serialization.

## [1.1.4] - 2023-06-22

### Fixed

- Use concurrent dictionary for In memory backing store registry to avoid race conditions.

## [1.1.3] - 2023-06-13

### Fixed

- Fixed a bug that would allow multiple "Content-Type", "Content-Length", and "Content-Location" header values.

## [1.1.2] - 2023-05-17

### Changed

- Fixes a bug in the InMemoryBackingStore that would not leave out properties in nested IBackedModel properties.

## [1.1.1] - 2023-04-06

### Added

- Adds the Response Headers to the ApiException class

## [1.1.0] - 2023-03-22

### Added

- Added a base request builder and a request configuration class to reduce the amount of code being generated.

## [1.0.1] - 2023-03-10

### Changed

- Update minimum version of [`System.Diagnostics.DiagnosticSource`](https://www.nuget.org/packages/System.Diagnostics.DiagnosticSource) to `6.0.0`.

## [1.0.0] - 2023-02-27

### Added

- GA release

### Changed

## [1.0.0-rc.7] - 2023-02-03

### Added

- Added a status code field to the API exception class.

## [1.0.0-rc.6] - 2023-01-27

### Changed

- Relaxed nullability tolerance when merging objects for composed types.

## [1.0.0-rc.5] - 2023-01-26

### Changed

- Use concurrent dictionary for serialization registry to avoid race conditions.

### Changed

## [1.0.0-rc.4] - 2023-01-17

### Changed

- Adds support for nullable reference types

## [1.0.0-rc.3] - 2023-01-09

### Changed

- Adds a method to convert abstract requests to native requests in the request adapter interface.

## [1.0.0-rc.2] - 2023-01-05

### Changed

- Release candidate 2
- Prevents sending requests with empty query parameter values

## [1.0.0-rc.1] - 2022-12-15

### Changed

- Release candidate 1

## [1.0.0-preview.19] - 2022-12-13

### Changed

- Added support for multi-valued request headers

## [1.0.0-preview.18] - 2022-11-22

### Changed

- Bumps Tavis.UriTemplates to strongly name binary version

## [1.0.0-preview.17] - 2022-11-11

### Changed

- Fixes a bug in the InMemoryBackingstore that would not detect changes in nested collections of complex types that had backing stores

## [1.0.0-preview.16] - 2022-10-28

### Changed

- Fixed a bug where request bodies that are collections of single items would not serialize properly

## [1.0.0-preview.15] - 2022-10-18

### Added

- Adds an API key authentication provider.

## [1.0.0-preview.14] - 2022-10-17

### Changed

- Changes the ResponeHandler parameter in IRequestAdapter to be a RequestOption

## [1.0.0-preview.13] - 2022-10-05

### Changed

- Fixes a bug in the InMemoryBackingstore that would not detect changes in nested complex types and collections

## [1.0.0-preview.12] - 2022-09-19

### Added

- Added tracing support for request information content type.

## [1.0.0-preview.11] - 2022-09-06

### Added

- Added support for composed types serialization.

## [1.0.0-preview.10] - 2022-08-11

### Changed

- DateTime instances added to the url paths to default to ISO 8601
- Adds explicit error message if the url template expects URI when accessing the URI from RequestInformation

## [1.0.0-preview.9] - 2022-06-13

### Changed

- Fixes a bug where the backing store would fail to be set in clients running .Net framework.

## [1.0.0-preview.8] - 2022-05-11

### Added

- Breaking: added an additional parameter to authentication methods to carry contextual information.

## [1.0.0-preview.7] - 2022-05-11

### Added

- Adds a method to support scalar request bodies

## [1.0.0-preview.6] - 2022-04-22

### Added

- Adds support for api surface revamp for query parameters

## [1.0.0-preview.5] - 2022-04-12

### Changed

- Breaking: Changes target runtime to netstandard2.0

## [1.0.0-preview.4] - 2022-04-06

### Added

- Adds the ability to get the query parameter name from attribute.

## [1.0.0-preview.3] - 2022-04-04

### Changed

- Breaking: simplifies the field deserializers.

## [1.0.0-preview.2] - 2022-03-29

### Added

- Added support for vendor specific serialization in registries

## [1.0.0-preview.1] - 2022-03-18

### Added

- Initial Nuget release
