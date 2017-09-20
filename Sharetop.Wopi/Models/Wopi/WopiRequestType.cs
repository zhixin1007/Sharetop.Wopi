using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharetop.Wopi.Models.Wopi
{
    /// <summary>
    /// Enumeration for the different types of WOPI Requests
    /// For more information see: https://wopi.readthedocs.org/projects/wopirest/en/latest/index.html
    /// </summary>
    public enum WopiRequestType
    {
        None,
        CheckFileInfo,
        GetFile,
        Lock,
        GetLock,
        RefreshLock,
        Unlock,
        UnlockAndRelock,
        PutFile,
        PutRelativeFile,
        RenameFile,
        PutUserInfo
    }
}