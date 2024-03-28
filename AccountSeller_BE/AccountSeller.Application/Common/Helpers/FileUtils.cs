using Microsoft.AspNetCore.Mvc;
using AccountSeller.Infrastructure.Common.Models;
using AccountSeller.Application.Common.Services;

namespace AccountSeller.Application.Common.Helpers
{
    public static class FileUtils
    {
        /// <summary>
        /// Get file content result from <see cref="RenderingResult"/> to respond.
        /// </summary>
        /// <param name="renderingResult">Rendering result to output</param>
        /// <param name="outputFormatFileName">Name of output file</param>
        /// <param name="fileExtension">File extension of output file</param>
        /// <param name="mimeFileType">MIME string of output file</param>
        /// <param name="additionalFormatFileName">Name that will be appear after outputFormatFileName in download file name</param>
        /// <param name="cancellationToken">A signal to stop doing task when request is canceled</param>
        /// <returns>A FileContentResult to be downloaded by user</returns>
        public static async Task<FileContentResult> GetFileContentResult(
            this RenderingResult renderingResult,
            string outputFormatFileName,
            string fileExtension,
            string mimeFileType,
            string? additionalFormatFileName = null,
            CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();
            await renderingResult.Result.CopyToAsync(memoryStream, cancellationToken);

            var fileName = $"{outputFormatFileName}{additionalFormatFileName}{DateTimeHelper.Now:yyyyMMddHHmmss}{fileExtension}";

            return new FileContentResult(memoryStream.ToArray(), mimeFileType)
            {
                FileDownloadName = fileName
            };
        }

        /// <summary>
        /// Get file content result with raw file name from <see cref="TransferResult"/> instance.
        /// </summary>
        /// <param name="transferResult"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <param name="httpContentType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<FileContentResult> GetRawFileContentResultAsync(
            this TransferResult transferResult,
            string fileName,
            string fileExtension,
            string httpContentType,
            CancellationToken cancellationToken = default)
        {
            // Declare temporary memory stream to contain stream binary.
            using var memoryStream = new MemoryStream();
            await transferResult.FileStream.CopyToAsync(memoryStream, cancellationToken);
            return new FileContentResult(memoryStream.ToArray(), httpContentType)
            {
                FileDownloadName = $"{fileName}{fileExtension}"
            };
        }

    }
}
