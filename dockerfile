FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1909 as builder
COPY . /temp/
WORKDIR /temp
RUN nuget restore
RUN msbuild /p:DeployOnBuild=true /p:WebPublishMethod=FileSystem /p:DeployTarget=WebPublish /p:publishUrl=c:\output

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-1909
RUN powershell.exe Add-WindowsFeature Web-Windows-Auth
RUN powershell.exe -NoProfile -Command \
  Set-WebConfigurationProperty -filter /system.WebServer/security/authentication/AnonymousAuthentication -PSPath IIS:\ -name enabled -value true  ; \
  Set-WebConfigurationProperty -filter /system.webServer/security/authentication/windowsAuthentication -PSPath IIS:\ -name enabled -value true

# Copy application
COPY --from=builder /output/ /inetpub/wwwroot
