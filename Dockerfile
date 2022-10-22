# Set the base image as the .NET 6.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
COPY . ./
RUN dotnet publish ./nup-github-action-jira-transition/nup-github-action-jira-transition.csproj -c Release -o out --no-self-contained

# Label as GitHub action
LABEL com.github.actions.name=".NET code metric analyzer"
LABEL com.github.actions.description="Transitions jira issues "
LABEL com.github.actions.icon="sliders"
LABEL com.github.actions.color="purple"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/nup-github-action-jira-transition.dll" ]
