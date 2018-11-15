using System;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Entities.TableEntity
{
    ///<summary>
    ///
    ///</summary>
    public class Business_Payroll_Information
    {

        /// <summary>
        /// Desc:工号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string JobNumber { get; set; }

        /// <summary>
        /// Desc:发放日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? IssuingDate { get; set; }

        /// <summary>
        /// Desc:姓名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:岗位工资
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PostSalary { get; set; }

        /// <summary>
        /// Desc:考核奖
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? AssessmentOfAward { get; set; }

        /// <summary>
        /// Desc:加/值班
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? WorkOvertime { get; set; }

        /// <summary>
        /// Desc:节假日加班工资
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HolidaysOvertimePay { get; set; }

        /// <summary>
        /// Desc:休息日加班工资
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? RestdaysOvertimePay { get; set; }

        /// <summary>
        /// Desc:全勤奖
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PerfectAttendance { get; set; }

        /// <summary>
        /// Desc:嘉奖
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? Reward { get; set; }

        /// <summary>
        /// Desc:岗位津贴
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PostAllowance { get; set; }

        /// <summary>
        /// Desc:职称津贴
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? JobTitleAllowance { get; set; }

        /// <summary>
        /// Desc:夜班津贴
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? NightShiftAllowance { get; set; }

        /// <summary>
        /// Desc:班长津贴
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HighTemperatureAllowance { get; set; }
        /// <summary>
        /// Desc:班长津贴
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? MonitorAllowance { get; set; }

        /// <summary>
        /// Desc:其他1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Other1 { get; set; }

        /// <summary>
        /// Desc:其他2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Other2 { get; set; }

        /// <summary>
        /// Desc:饭帖
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? RicePost { get; set; }

        /// <summary>
        /// Desc:通讯费
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? Communications { get; set; }

        /// <summary>
        /// Desc:病假扣
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? SickLeaveBuckle { get; set; }

        /// <summary>
        /// Desc:事假扣
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PrivateAffairLeaveBuckle { get; set; }

        /// <summary>
        /// Desc:其他扣
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? OtherButtons { get; set; }

        /// <summary>
        /// Desc:代扣
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? Withholding { get; set; }

        /// <summary>
        /// Desc:公积金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ProvidentFund { get; set; }

        /// <summary>
        /// Desc:补公积
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? FillReserves { get; set; }

        /// <summary>
        /// Desc:养老金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? Pensions { get; set; }

        /// <summary>
        /// Desc:
        /// Default:补养老金
        /// Nullable:True
        /// </summary>           
        public decimal? ComplementaryPension { get; set; }

        /// <summary>
        /// Desc:医保金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HealthCare { get; set; }

        /// <summary>
        /// Desc:补医保金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ComplementaryHealthCare { get; set; }

        /// <summary>
        /// Desc:失保金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? LossOfInsurance { get; set; }

        /// <summary>
        /// Desc:补失保金
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ComplementaryLossOfInsurance { get; set; }

        /// <summary>
        /// Desc:会费
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? TheMembershipFee { get; set; }

        /// <summary>
        /// Desc:应发
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ShouldSend { get; set; }

        /// <summary>
        /// Desc:个税
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? IncomeTax { get; set; }

        /// <summary>
        /// Desc:实扣
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? RealBuckle { get; set; }

        /// <summary>
        /// Desc:实发
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ActualRelease { get; set; }

        /// <summary>
        /// Desc:身份证号码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string IDCard { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public Guid? PersonnelVGUID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public Guid? PushVGUID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CreateUser { get; set; }

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

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public Guid VGUID { get; set; }

    }
}
