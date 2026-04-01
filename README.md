## DupFin - Duplicate File Finder
       <@_________________________>_._<_________________________@>      
       ||         :-._            |###|         :-._            ||                   
      | |       _.-'  '--.        |###|       _.-'  '--.        | |     
      / |     .'      _  -`\_     |] [|     .'      _  -`\_     | \     
     /  |    / .----.`_.'----'    |   |    / .----.`_.'----'    |  \    
    /`  |    ;/     `             |   |    ;/     `             |  '\    
    |   |   /_;                   |   |   /_;                   |   |
    \   |_________________________|   |_________________________|   /  


# DupFin (Duplicate Finder)

DupFin is a high-performance, multi-threaded console utility (with an upcoming WinForms GUI) designed to scan file systems and find duplicate files efficiently. 

Unlike naive duplicate scanners that hash every single file and choke system memory, DupFin is engineered to minimize Disk I/O operations and maximize CPU utilization without causing Task Starvation.

## Key Features & Under the Hood

* **Smart O(1) Pre-filtering:** The engine first groups files by their exact byte size using `Directory.EnumerateFiles`. Files with a unique size are immediately discarded from the queue, saving up to 90% of unnecessary hashing and disk reads.
* **Parallel Asynchronous Hashing:** Uses `Parallel.ForEachAsync` constrained by `Environment.ProcessorCount`. This perfectly balances CPU-bound mathematical operations (hashing) and I/O-bound disk reads, preventing Disk Thrashing on HDDs/SSDs.
* **Thread-Safe Architecture:** Completely lock-free asynchronous operations utilizing `ConcurrentDictionary` and `ConcurrentBag` to prevent thread deadlocks and bottlenecks during simultaneous file processing.
* **Fault-Tolerant:** Gracefully handles `UnauthorizedAccessException` for locked/system files without crashing the scan pipeline, using `FileShare.Read` for files opened by other processes.

## Tech Stack
* **Language:** C#
* **Framework:** .NET 8
* **Hashing Algorithms:** MD5, SHA-256, SHA-512

## How it Works
1.  **Traversal:** Recursively scans the target directory (Memory-safe enumeration).
2.  **Optimization:** Filters out files that do not share the exact same size in bytes.
3.  **Hashing:** Applies the selected cryptographic hash (Default: SHA-256) only to potential duplicates.
4.  **Reporting:** Outputs a clean, structured summary of duplicate groups and saves the results to a `.txt` file on the Desktop.

## Usage (Console)
Run the application and follow the interactive prompts:
1. Enter the target directory path (or press Enter for the current directory).
2. Choose the preferred hash algorithm.
3. Review the results in the console and opt to save them to a formatted text file.
