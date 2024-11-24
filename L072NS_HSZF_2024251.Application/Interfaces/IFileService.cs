using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Application.Interfaces
{
    public interface IFileService
    {

        void ImportJSONToDatabase(string filePath);
        void ExportDatabaseToJSON(string folderPath, out string generatedFileName);
    }
}
