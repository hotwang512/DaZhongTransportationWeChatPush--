using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DaZhongManagementSystem.Common.LogHelper
{
    public class Log4NetWriter : ILogWriter
    {
        public void WriteLogInfo(string txt)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(txt);
        }
    }
}
