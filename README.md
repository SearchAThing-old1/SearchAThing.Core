# SearchAThing.Core

Common core libraries.

## Build

There are many dependencies between projects in relative path directly from other source repositories,
in order to build successfully its suggested to clone follow repository [SearchAThing](https://github.com/devel0/SearchAThing) containing all of them.

## Linux build

- install mono-complete

- add ref assemblies

```
sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys A7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
apt-get update
apt-get install  referenceassemblies-pcl
```

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

