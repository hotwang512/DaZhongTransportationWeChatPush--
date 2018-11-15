using System;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    ///<summary>
    ///
    ///</summary>
    public class Business_ProtocolOperations_Information
    {
        /// <summary>
        /// Desc:推送主键
        /// Default:
        /// Nullable:True
        /// </summary>           
        public Guid? WeChatPushVGUID { get; set; }

        /// <summary>
        /// Desc:人员主键
        /// Default:
        /// Nullable:True
        /// </summary>           
        public Guid? PersonnelVGUID { get; set; }

        /// <summary>
        /// Desc:操作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? OperationTime { get; set; }

        /// <summary>
        /// Desc:操作结果
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Result { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public Guid VGUID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CreatedUser { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? ChangeDate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ChangeUser { get; set; }

    }
}
