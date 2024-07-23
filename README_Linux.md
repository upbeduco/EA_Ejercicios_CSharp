# Linux setup


## Installation of the .NET SDK

```bash

# ====================
# Register MS package Repository

# If running Ubuntu
declare repo_version=$(if command -v lsb_release &> /dev/null; then lsb_release -r -s; else grep -oP '(?<=^VERSION_ID=).+' /etc/os-release | tr -d '"'; fi)
# If running Mint
repo_version="22.04"

# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/$repo_version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
sudo dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb

# Update packages
sudo apt update

# ====================
# Installation

# Install both SDK and runtime
sudo apt-get install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0

# ====================
# Check the installation
dotnet --info
dotnet --list-sdks
dotnet --list-runtimes
```

## Plug-in for VSCode
[C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)  



## References

[Register the Microsoft Package Repository](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#register-the-microsoft-package-repository)  
[Install .NET in Linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website)  
[]()  
[]()  
