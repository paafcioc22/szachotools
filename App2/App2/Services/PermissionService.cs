using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;
using Xamarin.Forms.PlatformConfiguration;

namespace App2.Services
{
    public static class PermissionService
    {
        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
    where T : BasePermission, new()
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



        public static async Task CheckAndRequestMultiplePermissionsAsync(params BasePermission[] permissions)
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
