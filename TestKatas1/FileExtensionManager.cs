using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestKatas1.Interfaces;

namespace TestKatas1
{
    public class FileExtensionManager : IFileExtensionManager
    {
        public bool IsValid(string fileName)
        {
            if (fileName == "")
            {
                throw new Exception("filename has to be provided");
            }
            return fileName.ToLower().EndsWith(".slf");
        }
    }
}
