# ProxyScanSharp
- Lightweight
- Stable
- Configurable
- Linux and Windows support
- Scans random IP's per request
- Multithreading support
- Multiple ports support
- Custom timeouts support
- Custom domain check support

# Setup
- Ubuntu Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)
- Centos Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-centos)
- Debian Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian)
- Building Linux (dotnet publish -r linux-x64)
- Building Windows (just build as release)

# Usage
- (Linux no backround) dotnet ProxyScanSharp.dll <ports> <threads> <filename> <port_timeout_ms> <proxy_timeout_seconds> <url_to_check>
- (Linux in backround) screen dotnet ProxyScanSharp.dll <ports> <threads> <filename> <port_timeout_ms> <proxy_timeout_seconds> <url_to_check>
- (Windows) ProxyScanSharp.exe <ports> <threads> <filename> <port_timeout_ms> <proxy_timeout_seconds> <url_to_check>

# Example Usage
- (Linux no backround mutiple ports) dotnet ProxyScanSharp.dll 3128,8080 1 Valid.txt 200 2 https://ipv4.icanhazip.com
- (Linux no backround one port) dotnet ProxyScanSharp.dll 3128 1 Valid.txt 200 2 https://ipv4.icanhazip.com
- (Linux in backround mutiple ports) screen dotnet ProxyScanSharp.dll 3128,8080 1 Valid.txt 200 2 https://ipv4.icanhazip.com
- (Linux in backround one port) screen dotnet ProxyScanSharp.dll 3128 1 Valid.txt 200 2 https://ipv4.icanhazip.com
- (Windows) ProxyScanSharp.exe 3128 1 Valid.txt 200 2 https://ipv4.icanhazip.com

# Future Plans
- Configurable Scan only cloud ranges
- Configurable Scan ranges
- Configurable Blacklist Check (need community feedback for api to use)