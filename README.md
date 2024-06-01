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
