#Base image: Use an offical run time as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 

# set the working directory in the container
WORKDIR /app

#Expose the port on which your .net app runs
EXPOSE 6060

#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy the project files (.csproj) from your local machine into the container
COPY ["CloudCustomers/CloudCustomers.csproj", "CloudCustomers/"]

#download the dependencies
RUN dotnet restore "CloudCustomers/CloudCustomers.csproj"

# This will copy the remaining project files into the container.
COPY . .
WORKDIR "/src/CloudCustomers"
RUN dotnet build "CloudCustomers.csproj" -c Release -o /app/build

#Publish Stage
FROM build AS publish
RUN dotnet publish "CloudCustomers.csproj" -c Release -o /app/publish

#Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudCustomers.dll"]
