using AccountSeller.Application.Common.Services;

namespace AccountSeller.Application.Common.Interfaces
{
    public interface IFtpDirectoryService
    {
        string GetRootDirectory();

        /// <summary>
        /// Transfer a file with type <see cref="Stream"/> to FTP directory.
        /// </summary>
        /// <param name="fileStream">File instance in type <see cref="Stream"/></param>
        /// <param name="outputDirectory">Directory to save file in AWS EC2 instance.</param>
        /// <param name="outputFileName">File name to save file in AWS EC2 instance.</param>
        /// <returns>Result of transferring.</returns>
        Task<TransferResult> TransferToFtpDirectoryAsync(Stream fileStream, string outputDirectory, string outputFileName);

        /// <summary>
        /// Get a file from FTP Directory with type of <see cref="FileStream"/>.
        /// </summary>
        /// <param name="remoteFilePath">File path in FTP directory folder, must be included file name.</param>
        /// <returns></returns>
        TransferResult GetFileStream(string remoteFilePath);
    }
}
