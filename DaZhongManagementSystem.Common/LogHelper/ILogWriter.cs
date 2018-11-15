using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Common.LogHelper
{
    public interface ILogWriter
    {
        void WriteLogInfo(string txt);
    }
}
