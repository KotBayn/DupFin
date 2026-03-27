using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupFin.Enums
{
    // Type choices
    public enum HashAlgorithmType
    {
        MD5,
        SHA256,
        SHA512
    }

    public enum ScanMode
    {
        Async,
        Parallel,
        Sync,
        Bidirectional
    }
    
}
