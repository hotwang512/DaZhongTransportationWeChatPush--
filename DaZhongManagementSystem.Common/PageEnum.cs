﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaZhongManagementSystem.Common
{
    public enum PageEnum
    {

        //登录界面
        登录界面 = 0,

        //基础数据（组织结构）
        组织结构列表界面 = 1,

        新增部门界面 = 2,

        编辑部门界面 = 3,

        部门详情界面 = 4,

        //人员信息
        人员信息列表界面 = 5,

        人员信息详情页面 = 6,

        //习题管理
        习题管理列表界面 = 7,

        新增习题界面 = 8,

        编辑习题界面 = 9,

        习题详情界面 = 10,

        已审核习题列表界面 = 11,

        阅卷界面 = 36,

        //微信推送
        草稿列表界面 = 12,

        新增草稿界面 = 13,

        编辑草稿界面 = 14,

        草稿详情界面 = 15,

        已提交列表界面 = 16,

        已提交详情界面 = 17,

        已审核列表界面 = 18,

        已审核详情界面 = 19,

        已推送列表界面 = 20,

        已推送详情界面 = 21,

        //报表管理
        习题成绩报表界面 = 22,

        推送统计报表界面 = 23,

        //操作日志
        日志管理列表界面 = 24,

        日志详情界面 = 25,

        //系统管理
        用户管理列表界面 = 26,

        新增用户界面 = 27,

        编辑用户界面 = 28,

        用户详情界面 = 29,

        角色管理列表界面 = 30,

        新增角色界面 = 31,

        编辑角色界面 = 32,

        角色详情界面 = 33,

        手机端界面 = 34,

        配置文件详情界面 = 35,

        //推送历史
        推送历史界面 = 37,

        推送历史详情界面 = 45,

        //习题库管理
        习题库管理草稿习题界面 = 38,

        习题库管理正式习题界面 = 39,
        //知识库管理
        知识库管理草稿界面 = 40,

        知识库管理草稿新增界面 = 41,

        知识库管理草稿编辑界面 = 42,

        知识库管理正式列表界面 = 43,

        扫码历史 = 44,

        营收支付详情界面 = 46,
        // 其他支付=47

        支付历史界面 = 47,

        支付报表界面 = 48,

        协议操作历史界面 = 49,

        协议操作历史详细界面 = 50,

        红包领取历史界面=51
    }
}
