using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DaZhongManagementSystem.Entities.TableEntity;
using DaZhongManagementSystem.Entities.UserDefinedEntity;
using DaZhongManagementSystem.Infrastructure.QRCodeManagement;

namespace DaZhongManagementSystem.Areas.QRCodeManagement.Controllers.ScanData.BusinessLogic
{
    public class ScanDataLogic
    {
        private ScanDataServer _ScanData;
        public ScanDataLogic()
        {
            _ScanData = new ScanDataServer();
        }
        public Business_ScanData_Information Save(Guid vguid)
        {
            return _ScanData.Save(vguid);
        }
    }
}