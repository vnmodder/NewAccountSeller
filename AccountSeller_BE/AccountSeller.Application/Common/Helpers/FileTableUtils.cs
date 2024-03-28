
using AccountSeller.Infrastructure.Databases.AccountSellerDB;

namespace AccountSeller.Application.Common.Helpers
{
    public static class FileTableUtils
    {
        /// <summary>
        /// Insert  data into FileData_table
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="screenId">Id of screen that export a file (SN-20-00 etc...)</param>
        /// <param name="fileName">Name of output file</param>
        /// <param name="directory">Directory contains the file</param>
        /// <param name="noBillingDate">TyouhyouNaiyoDate</param>
        /// <param name="propertyCode">Property code of output data</param>
        /// <param name="tenantCode">Property code of output data</param>
        /// <param name="userId">Id of who inserted</param>
        /// <param name="outputTime">Time of export action</param>
        /// <param name="additionalInfoCode">A item in ExportTypeConstants or StatusTypeConstants or ReportListModeConstants</param>
        /// <param name="subAdditionalInfoCode">A item in RemovalExportTypeConstants, this params is required when additionalInfoCode is TransitionalRemovalTypeConstants</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The insertion to FileData_table task</returns>
        public static async Task InsertFileTableAsync(
            this AccountSellerDbContext dbContext,
            string screenId,
            string fileName,
            string directory,
            DateTime? noBillingDate,
            string propertyCode,
            string? tenantCode,
            string userId,
            DateTime outputTime,
            int? additionalInfoCode = null,
            int? subAdditionalInfoCode = null,
            CancellationToken cancellationToken = default)
        {
            //var fileType = GetOutputFileType(screenId, additionalInfoCode, subAdditionalInfoCode);

            //var fileDataRecord = new FileDataTable
            //{
            //    ChaFileName = fileName,
            //    ChaBukkenCode = propertyCode,
            //    ChaTenantCode = tenantCode,
            //    ChaKeiyakuType = fileType,
            //    ChaFileDirectory = $"{directory}{fileName}",
            //    DteFileDate = outputTime,
            //    DteTyouhyouNaiyoDate = noBillingDate,
            //    ChaInsertUserId = userId,
            //    DteInsertDate = outputTime,
            //    ChaDeleteUserId = null
            //};

            //dbContext.FileDataTables.Add(fileDataRecord);

            //await dbContext.SaveChangesAsync(cancellationToken);
        }

        //private static string GetOutputFileType(string screenId, int? additionalInformationCode = null, int? subAdditionalInformationCode = null)
        //{
        //    return screenId switch
        //    {
        //        ScreenIdConstants.PURCHASE_ORDER => ScreenOutputFileConstants.PURCHASE_ORDER,
        //        ScreenIdConstants.PROPERTY_LEDGER => ScreenOutputFileConstants.PROPERTY_LEDGER,
        //        ScreenIdConstants.TENANT_LEDGER => GetOutputFileTypeForTenantLedger(additionalInformationCode!.Value),
        //        ScreenIdConstants.RECRUIMENT => ScreenOutputFileConstants.RECRUITMENT,
        //        _ => ScreenOutputFileConstants.OTHERS,
        //    };
        //}

    }
}
