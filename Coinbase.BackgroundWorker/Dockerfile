FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

WORKDIR /app

COPY ./Coinbase.BackgroundWorker/Coinbase.BackgroundWorker.csproj ./Coinbase.BackgroundWorker/Coinbase.BackgroundWorker.csproj
COPY ./Coinbase.BackgroundTasks/Coinbase.BackgroundTasks.csproj ./Coinbase.BackgroundTasks/Coinbase.BackgroundTasks.csproj
COPY ./Coinbase.Constants/Coinbase.Constants.csproj ./Coinbase.Constants/Coinbase.Constants.csproj
COPY ./Coinbase.Data/Coinbase.Data.csproj ./Coinbase.Data/Coinbase.Data.csproj
COPY ./Coinbase.Dto/Coinbase.Dto.csproj ./Coinbase.Dto/Coinbase.Dto.csproj
COPY ./Coinbase.Integration/Coinbase.Integration.csproj ./Coinbase.Integration/Coinbase.Integration.csproj

COPY ./NuGet.Config ./NuGet.Config

RUN dotnet restore ./Coinbase.BackgroundWorker/Coinbase.BackgroundWorker.csproj --configfile NuGet.Config -p:HideWarningsAndErrors=true -p:EmitAssetsLogMessages=false

COPY /Coinbase.BackgroundWorker/ ./Coinbase.BackgroundWorker/
COPY /Coinbase.BackgroundTasks/ ./Coinbase.BackgroundTasks/
COPY /Coinbase.Constants/ ./Coinbase.Constants/
COPY /Coinbase.Data/ ./Coinbase.Data/
COPY /Coinbase.Dto/ ./Coinbase.Dto/
COPY /Coinbase.Integration/ ./Coinbase.Integration/

RUN dotnet publish ./Coinbase.BackgroundWorker/Coinbase.BackgroundWorker.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Coinbase.BackgroundWorker.dll"]
