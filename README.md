# ProxyScanSharp
- Lightweight
- Stable
- Configurable
- Linux and Windows support
- Scans random IP's per request
- Multithreading support

# Setup
- Ubuntu Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)
- Centos Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-centos)
- Debian Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian)
- Building Linux (dotnet publish -r linux-x64)
- Building Windows (just build as release)

# Usage
- (Linux no backround) dotnet ProxyScanSharp.dll <port> <threads>
- (Linux in backround) screen dotnet ProxyScanSharp.dll <port> <threads>
- (Windows) ProxyScanSharp.exe <port> <threads>

# Notes
- Proxies are stored in Valid.txt

# Future Plans
- Configurable file name for saved proxies
- Configurable Scan only cloud ranges
- Configurable Scan ranges
- Configurable multiple ports
- Configurable Domain Check
- Configurable Blacklist Check
- Configurable Timeouts