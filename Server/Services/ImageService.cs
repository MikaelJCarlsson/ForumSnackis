using ForumSnackis.Server.Data;
using ForumSnackis.Shared;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment env;
        private readonly ApplicationDbContext dbContext;

        public ImageService(IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.env = env;
        }

        internal async Task<int> UploadFileAsync(ImageFile[] files, ClaimsPrincipal claims)
        {
            var user = dbContext.Users.Find(claims.Claims.First(x => x.Type == "sub").Value);
            var file = files.FirstOrDefault();
            if (file is null)
                return 0;
            var buf = Convert.FromBase64String(file.base64data);
            var relativePath = "\\Images\\" + Guid.NewGuid().ToString("N") + "-" + file.fileName;

            if (user.ImagePath == "https://cdn.frankerfacez.com/emoticon/447568/4")
            {
                await File.WriteAllBytesAsync(env.ContentRootPath.Replace("Server", "Client/wwwroot") + relativePath, buf);
                user.ImagePath = relativePath;
                dbContext.Update(user);
                return await dbContext.SaveChangesAsync();
            }
            else
            {
                File.Delete(env.ContentRootPath.Replace("Server", "Client/wwwroot") + user.ImagePath);
                user.ImagePath = relativePath;
                await File.WriteAllBytesAsync(env.ContentRootPath.Replace("Server", "Client/wwwroot") + relativePath, buf);
                dbContext.Update(user);
                return await dbContext.SaveChangesAsync();
            }
        }

        internal async Task<string> GetImage(string id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user is null)
                return null;
            if (user.ImagePath is null)
                return null;

            return user.ImagePath;
        }
    }
}
