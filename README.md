# Dullahan

## About
Dullahan is the client application for UHF RFID antennas as a part of the ETC system. It allows these antennas to send their scans to the ETC Backend.

## Development Team: Team ERC Hardware Division
#### Michael Sun

## Core File Descriptions
**RFID.cs** : implementation of RFID antenna scan logic including parsing unique scans and assigning user RFID tags to their items in a scan package, updating item availability status information, and determining if a scan is of a check-in or a check-out.

**WebCalls.cs**: implementation of web calls to the ETC backend. Also limits the rate of unique scans being sent by Dullahan within a specified period of time. 

**Program.cs**: program state logic for Dullahan. 

## External DLL files used
**Basic.dll** and **UHFReader18CSharp.dll**: two open source libraries provided by CNCEST for communicating with their antennas. 

## How to Install and Run
Download files from **bin > Release** and upload them to a single folder on a Raspberry Pi.
Install WineHQ on the Raspberry Pi: **sudo apt-get install wine**
Install Xvfb on the Raspberry Pi: **sudo apt-get install xvfb**

# Setting Up an Antenna with Dullahan
1. Connect the RFID antenna to a power supply. 
2. Connect the antenna client hosting Raspberry Pi to a power supply, and connect the USB cable of the RFID antenna to this Raspberry Pi.
3. On your computer, start an instance of the web application backend with “npm run start”. 
4. Use “ssh (username of Pi)@(IP address)” to connect to the Raspberry Pi and use “cd (directory path)” to navigate to the folder where you copied the client application files. 
5. Now use the command “Display=:0.0 wine Dullahan.exe” to start the client application headlessly. 
6. Use the first listed command, “scan (IP address of your backend host computer)”. Your touchless equipment scanning system is now ready to go!
6. If the antenna is disconnected from the Raspberry Pi or loses power at any point, close the application and restart it. 

