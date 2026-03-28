## DupFin - Duplicate File Finder
       <@_________________________>_._<_________________________@>      
       ||         :-._            |###|         :-._            ||                   
      | |       _.-'  '--.        |###|       _.-'  '--.        | |     
      / |     .'      _  -`\_     |] [|     .'      _  -`\_     | \     
     /  |    / .----.`_.'----'    |   |    / .----.`_.'----'    |  \    
    /`  |    ;/     `             |   |    ;/     `             |  '\    
    |   |   /_;                   |   |   /_;                   |   |
    \   |_________________________|   |_________________________|   /  


DupFin is a fast and efficient console utility built with C# and .NET. It is designed to scan specific directories, identify identical files, and group them together to help users free up disk space. 

## Features
* **Hash-Based Comparison:** Uses cryptographic hashing (e.g., SHA-256) to ensure files are 100% identical, avoiding false positives.
* **Service-Oriented Architecture:** The core logic is decoupled from the console interface, ensuring clean and maintainable code.
* **Detailed Reporting:** Generates a structured output displaying the exact hash and the paths of all duplicated files.

## Example Output
```text
*===* DupFin Scan Results *===*
Date: 28-03-2026 20:08:23

Total duplicate groups found: 2

[Hash: 03b05abe2ffc240aea0bd3e0a21edbbb242d4f16c2857919acab20ee5e7c63923248a4f0f1670b5f3d43dfeb73da956486455f68b1263f332e0dbebc9d6e6867]
  + D:\Games\Lynda - Unity Cert Prep\01 Introduction.mp4
  + D:\Games\Courses\Lynda - Unity Cert Prep\01 Introduction.mp4
