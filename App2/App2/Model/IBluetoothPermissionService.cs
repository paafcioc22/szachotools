using System.Collections.Generic;
using System.Threading.Tasks;

namespace App2.Model
{
    /// <summary>
    /// We need to create an interface for DependencyService (Android-iOS-UWP)
    /// </summary>
    public interface IBluetoothPermissionService
    {
        Task<bool> CheckAndRequestBluetoothPermissionAsync();
    }
}