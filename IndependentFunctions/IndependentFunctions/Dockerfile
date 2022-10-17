FROM mcr.microsoft.com/dotnet/sdk:5.0 AS installer-env
ENV ASPNETCORE_ENVIRONMENT=Production

RUN dotnet_sdk_version=3.1.409 && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz && mkdir -p /usr/share/dotnet && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet && rm dotnet.tar.gz

COPY . /src/dotnet-function-app

RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish DataProviderService/Sperry.MxA.DataProvider.Functions/Sperry.MxA.DataProvider.Functions.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:3.0-dotnet-isolated5.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]