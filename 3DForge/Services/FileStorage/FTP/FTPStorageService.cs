﻿using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;

namespace Backend3DForge.Services.FileStorage.FTP
{
    public class FTPStorageService : IFileStorage
    {
        protected readonly FTPStorageConfigurationMetadata configuration;

        public FTPStorageService(IOptions<FTPStorageConfigurationMetadata> configuration)
        {
            this.configuration = configuration.Value;
        }

        public async Task DeleteFileAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            filename = NormalizePath(filename);
            string[] pathParts = filename.Split('/');

            FtpWebRequest? request = CreateFTPConnection(filename, WebRequestMethods.Ftp.DeleteFile);
            WebResponse response = await request.GetResponseAsync();

            response.Close();

            // delete empty directories
            for (int i = pathParts.Length - 1; i > 0; i--)
            {
                string path = string.Join("/", pathParts.Take(i));
                if (await IsDirectoryEmptyAsync(path))
                {
                    await DeleteDirectoryAsync(path);
                }
            }

        }

        public async Task<Stream> DownloadFileAsync(string filename)
        {
            FtpWebRequest? request = CreateFTPConnection(filename, WebRequestMethods.Ftp.DownloadFile);

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            return response.GetResponseStream();
        }

        public async Task UploadFileAsync(string filename, Stream fileStream, int fileSize = -1)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            filename = NormalizePath(filename);

            string[] pathParts = filename.Split('/');
            string path = string.Empty;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                path = $"{path}/{pathParts[i]}";
                if (!await IsDirectoryExists(path))
                {
                    await MkDirAsync(path);
                }
            }

            FtpWebRequest request = CreateFTPConnection(filename, WebRequestMethods.Ftp.UploadFile);

            request.ContentLength = fileSize > 0 ? fileSize : fileStream.Length;

            Stream ftpStream = request.GetRequestStream();
            await fileStream.CopyToAsync(ftpStream);
            ftpStream.Close();
        }

        protected async Task<bool> IsDirectoryExists(string directoryPath)
        {
            try
            {
                FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.ListDirectoryDetails);
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    return true;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        protected async Task MkDirAsync(string directoryPath)
        {
            FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.MakeDirectory);

            var response = await request.GetResponseAsync();
            response.Close();
        }

        protected async Task<bool> IsDirectoryEmptyAsync(string directoryPath)
        {
            try
            {
                FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.ListDirectory);

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                            return false;
                    }
                }

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        protected async Task DeleteDirectoryAsync(string directoryPath)
        {
            FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.RemoveDirectory);

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            response.Close();
        }

        protected FtpWebRequest CreateFTPConnection(string path, string method)
        {
            path = NormalizePath(path);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{configuration.UriPath}/{path}");
            request.Method = method;
            request.Credentials = new NetworkCredential(configuration.Username, configuration.Password);

            return request;
        }

        protected string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.StartsWith("/"))
            {
                path = path.Substring(1)
                    .TrimEnd('/');
            }
            return path.Replace("\\", "/");
        }
    }
}