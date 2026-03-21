FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Install native AOT prerequisites
RUN apt-get update && apt-get install -y \
    clang \
    zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*

# Copy solution and project files first — layer caching means
# NuGet restore only reruns when these change, not on every code change
COPY TonnUr.sln global.json ./
COPY src/TonnUr.Domain/TonnUr.Domain.csproj             src/TonnUr.Domain/
COPY src/TonnUr.Application/TonnUr.Application.csproj   src/TonnUr.Application/
COPY src/TonnUr.Infrastructure/TonnUr.Infrastructure.csproj src/TonnUr.Infrastructure/
COPY src/TonnUr.Api/TonnUr.Api.csproj                   src/TonnUr.Api/

RUN dotnet restore src/TonnUr.Api/TonnUr.Api.csproj

# Copy everything else and build
COPY src/ src/
RUN dotnet publish src/TonnUr.Api/TonnUr.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime image — much smaller than sdk image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Non-root user — good practice for k8s
RUN groupadd --system --gid 1001 appgroup && \
    useradd --system --uid 1001 --gid appgroup --no-create-home appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TonnUr.Api.dll"]