FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG CERT_PASS
ENV CERT_PASS=$CERT_PASS

RUN mkdir https
RUN dotnet dev-certs https -ep ./https/aspnetapp.pfx -p ${CERT_PASS}
RUN dotnet dev-certs https --trust

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

#ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
RUN dotnet restore "Conservices.Status/Conservices.Status.csproj"

COPY . .
WORKDIR "/src/Conservices.Status"
RUN dotnet build "Conservices.Status.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Conservices.Status.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --chmod=0755 --from=build /https/* /https/
COPY --from=publish /app/publish .

RUN mkdir keys
RUN chmod -R 775 keys

RUN mkdir logs
RUN chmod -R 775 logs

ENTRYPOINT ["dotnet", "Conservices.Status.dll"]
