using System;
using System.Collections.Generic;
using System.Text;

public interface IPlatformCopyAssets
{
    // For Android Only (Copy the assets file to android sdcard)
    bool CopyAssets(string imageName);
}

