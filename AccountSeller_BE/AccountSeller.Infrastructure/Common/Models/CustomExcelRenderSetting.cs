namespace AccountSeller.Infrastructure.Common.Models
{
    public class CustomExcelRenderSetting
    {
        /// <summary>
        /// Number of column per each detail record.
        /// </summary>
        public int DetailColumnCount { get; set; }

        /// <summary>
        /// Row index to start render detail records.
        /// </summary>
        public int DetailStartRow { get; set; }

        /// <summary>
        /// Column index to start render detail records.
        /// </summary>
        public int DetailStartColumn { get; set; }

        /// <summary>
        /// Max number of worksheets to rendering.
        /// </summary>
        public int MaxPageCount { get; set; }

        /// <summary>
        /// Max detail record per each worksheet.
        /// </summary>
        public int DetailRowPerPage { get; set; }

        /// <summary>
        /// Setting to render position (column) of header records.
        /// </summary>
        public List<CustomDetailCellRangeSetting> HeaderSettings { get; set; } = new List<CustomDetailCellRangeSetting>();

        /// <summary>
        /// Setting to render position (column) of detail records.
        /// </summary>
        public List<CustomDetailCellRangeSetting> DetailSettings { get; set; } = new List<CustomDetailCellRangeSetting>();
    }
}
