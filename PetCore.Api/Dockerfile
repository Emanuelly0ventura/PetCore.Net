FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PetCore.Api/PetCore.Api.csproj", "PetCore.Api/"]
COPY ["PetCore.Application/PetCore.Application.csproj", "PetCore.Application/"]
COPY ["PetCore.Domain/PetCore.Domain.csproj", "PetCore.Domain/"]
COPY ["PetCore.Infrastructure/PetCore.Infrastructure.csproj", "PetCore.Infrastructure/"]
RUN dotnet restore "PetCore.Api/PetCore.Api.csproj"
COPY . .
WORKDIR /src/PetCore.Api
RUN dotnet publish "PetCore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PetCore.Api.dll"]