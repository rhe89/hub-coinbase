FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

WORKDIR /app

COPY ./Coinbase.Web.Api/Coinbase.Web.Api.csproj ./Coinbase.Web.Api/Coinbase.Web.Api.csproj
COPY ./Coinbase.BackgroundTasks/Coinbase.BackgroundTasks.csproj ./Coinbase.BackgroundTasks/Coinbase.BackgroundTasks.csproj
COPY ./Coinbase.Constants/Coinbase.Constants.csproj ./Coinbase.Constants/Coinbase.Constants.csproj
COPY ./Coinbase.Data/Coinbase.Data.csproj ./Coinbase.Data/Coinbase.Data.csproj
COPY ./Coinbase.Dto/Coinbase.Dto.csproj ./Coinbase.Dto/Coinbase.Dto.csproj
COPY ./Coinbase.Integration/Coinbase.Integration.csproj ./Coinbase.Integration/Coinbase.Integration.csproj
COPY ./Coinbase.Providers/Coinbase.Providers.csproj ./Coinbase.Providers/Coinbase.Providers.csproj

COPY ./NuGet.Config ./NuGet.Config

RUN dotnet restore ./Coinbase.Web.Api/Coinbase.Web.Api.csproj --configfile NuGet.Config -p:HideWarningsAndErrors=true -p:EmitAssetsLogMessages=false

COPY /Coinbase.Web.Api/ ./Coinbase.Web.Api/
COPY /Coinbase.BackgroundTasks/ ./Coinbase.BackgroundTasks/
COPY /Coinbase.Constants/ ./Coinbase.Constants/
COPY /Coinbase.Data/ ./Coinbase.Data/
COPY /Coinbase.Dto/ ./Coinbase.Dto/
COPY /Coinbase.Integration/ ./Coinbase.Integration/
COPY /Coinbase.Providers/ ./Coinbase.Providers/

RUN dotnet publish ./Coinbase.Web.Api/Coinbase.Web.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Coinbase.Web.Api.dll"]
