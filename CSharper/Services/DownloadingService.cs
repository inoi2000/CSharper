using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class DownloadingService : IDisposable
    {
        public string OutPutDirectory => "DownloadedFiles";
        private HttpClient _httpClient => new HttpClient();

        public async Task<Stream> DownloadToMemoryAsync(Uri uri)
        {
            if (uri == null) { throw new ArgumentNullException($"\"{nameof(uri)}\" не может быть null.", nameof(uri)); }

            using var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task DownloadToFileAsync(Uri uri, string fileName, IProgress<double> progress, CancellationToken token)
        {
            if (uri == null) { throw new ArgumentNullException($"\"{nameof(uri)}\" не может быть null.", nameof(uri));}

            EnshureSaveFolderExist();

            string outputPath = $"{OutPutDirectory}\\{fileName}";
            long localFileLen = 0;

            FileStream file;
            if (File.Exists(outputPath))
            {
                localFileLen = new FileInfo(outputPath).Length;
                file = File.OpenWrite(outputPath);
                file.Seek(0, SeekOrigin.End);
            }
            else { file = File.Create(outputPath); }

            var request = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers = { Range = new RangeHeaderValue(localFileLen, null) }
            };

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            long totalBytesRead = 0;

            response.EnsureSuccessStatusCode();

            long? contenerLength = response.Content.Headers.ContentLength;

            await using var contentStream = await response.Content.ReadAsStreamAsync();

            byte[] buffer = new byte[8192 * 2];

            while (true)
            {
                if (token.IsCancellationRequested) 
                    token.ThrowIfCancellationRequested();

                int bytesRead = await contentStream.ReadAsync(buffer);

                if (bytesRead == 0) { break; }

                if (bytesRead == buffer.Length) { await file.WriteAsync(buffer, token); }
                else { await file.WriteAsync(buffer[..bytesRead], token); }

                totalBytesRead += bytesRead;
                if (contenerLength != null)
                {
                    progress.Report((double)totalBytesRead / contenerLength.Value);
                }
            }
            file.Close();
            file.Dispose();
        }

        public void EnshureSaveFolderExist()
        {
            if(!Directory.Exists(OutPutDirectory)) { Directory.CreateDirectory(OutPutDirectory); }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
