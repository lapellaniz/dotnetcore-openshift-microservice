FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /build
COPY src/SampleHosting/SampleHosting.csproj src/SampleHosting/
RUN dotnet restore src/SampleHosting/SampleHosting.csproj
COPY . .
WORKDIR /build/src/SampleHosting
RUN dotnet build SampleHosting.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SampleHosting.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SampleHosting.dll"]
