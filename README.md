# WinLock Server
A simple HTTP server that responds with '0' if the host PC is unlocked and '1' if it is locked.

## Setup
**This program can only run on Windows!**
In order to run, please ensure you've done the following:
- Run `netsh http add urlacl url=http://+:26969/ user=<domain>\<username>` in an elevated command prompt.
  - Ensure you replace `<domain>` with your domain (on personal computers, this is often your computer's name, found in Settings -> System -> About -> Device name.
  - `<username>` must be replaced with your username. This should be the same as the name of your user directory.
- Add port `26969` to the Windows Defender Firewall.
  - Search for 'Windows Defender Firewall' in your PC.
  - Click 'Advanced Settings'.
  - Click 'Inbound Rules'.
  - Click 'New Rule...' on the right side.
  - For 'Rule Type' select 'Port'.
  - For 'Protocol' select 'TCP'.
  - For 'Port' select 'Specific local ports' and type '26969' in the box.
  - For 'Action' select 'Allow the connection'.
  - For 'Profile' you should probably only select 'Private', but this may differ depending on your use case.
  - For 'Name', I suggest 'WinLock Server', but this can be anything you want.
  - Click 'Finish' and you can access the server on port 26969 from any device on your local network.

  The reasons as to why these steps are necessary are because of Windows being very strict with security regarding networking.  
  Not running the netsh command would require the application to always be started as admin,
  not allowing incoming traffic for the port would mean that the server can only be accessed locally and no other device can access it.
