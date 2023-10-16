using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App2.Model
{
    public static class PermissionService
    {
        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
    where T : Permissions.BasePermission, new()
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            if (status != PermissionStatus.Granted)
            {
                // Informuj użytkownika, że nie ma wymaganych uprawnień.
            }
            return status;
        }



        public static async Task CheckAndRequestMultiplePermissionsAsync(params Permissions.BasePermission[] permissions)
        {
            foreach (var permission in permissions)
            {
                var status = await permission.CheckStatusAsync();
                if (status != PermissionStatus.Granted)
                {
                    status = await permission.RequestAsync();
                    if (status != PermissionStatus.Granted)
                    {
                        // Informuj użytkownika, że nie ma wymaganych uprawnień.
                        return;
                    }
                }
            }
        }

    }
}
