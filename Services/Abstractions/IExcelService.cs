namespace Services
{
    using Models;
    using System.Collections.Generic;
    
    public interface IExcelService
    {
        DownloadFile Export<T>(List<T> tList);
    }
}
