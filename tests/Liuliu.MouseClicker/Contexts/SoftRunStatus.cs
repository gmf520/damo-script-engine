// -----------------------------------------------------------------------
//  <copyright file="SoftRunStatus.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-15 15:32</last-date>
// -----------------------------------------------------------------------

namespace Liuliu.MouseClicker
{
    public enum SoftRunStatus
    {
        Initialized,
        Starting,
        WaitingUpdate,
        StartFail,
        Started,
        Stopping
    }
}