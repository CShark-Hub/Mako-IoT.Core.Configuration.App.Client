# Mako-IoT.Core.Configuration.App.Client
Sample web client application for configuration API. This project uses [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor).

### Mixed content
Since MAKO IoT configuration API is served via HTTP by default, Mixed Content must be allowed in browser for the client to be able to connect. If HTTPS is used by the web server on your device, this is not an issue.

## How to manually sync fork
- Clone repository and navigate into folder
- From command line execute bellow commands
- **git remote add upstream https://github.com/CShark-Hub/Mako-IoT.Base.git**
- **git fetch upstream**
- **git rebase upstream/main**
- If there are any conflicts, resolve them
  - After run **git rebase --continue**
  - Check for conflicts again
- **git push -f origin main**
