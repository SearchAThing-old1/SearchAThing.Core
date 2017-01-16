# SearchAThing.Core

Common core libraries.

## Build

There are many dependencies between projects in relative path directly from other source repositories,
in order to build successfully its suggested to clone follow repository [SearchAThing](https://github.com/devel0/SearchAThing) containing all of them.

## Linux build

- install mono-complete

- edit ~/.config/NuGet/NuGet.Config

ensure a local repositoryPath is activated, this way the .csproj will search in $(HOME)/nuget-packages and will found required references

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <activePackageSource>
    <add key="Official NuGet Gallery" value="https://www.nuget.org/api/v2/" />
  </activePackageSource>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
  <config>
    <add key="repositoryPath" value="/root/nuget-packages" />
  </config>
</configuration>

```

