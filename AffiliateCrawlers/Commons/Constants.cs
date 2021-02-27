namespace AffiliateCrawlers.Commons
{
    public static class Constants
    {
        /// <summary>
        /// Application constants
        /// </summary>
        public static class AppConstants
        {
            public const string AppName = "AffiliateCrawlers";

            public const string DataIsEmpty = "Không có dữ liệu để xuất file";
            public const string WrongFolder = "Folder chọn không hợp lên";
            public const string ExportFileSuccessful = "Xuất file thành công";
            public const string ExportFileFailure = "Xuất file không thành công";

            public const string TextFileExtension = "txt";
        }

        /// <summary>
        /// Constant for Sablance page
        /// </summary>
        public static class Sablanca
        {
            public const string HostName = "https://sablanca.vn/";
            public const string SiteName = "sablanca.vn";
            public const string FileName = "sablanca_";

            public const string MaterialText = "Chất liệu";
            public const string StrapTypeText = "Loại dây đeo";
            public const string DimensionText = "Kích thước";
            public const string CompartmentText = "Số ngăn";
            public const string StyleText = "Dòng";
            public const string Description = "Mô tả";
        }

        /// <summary>
        /// Constant for Sevenam page
        /// </summary>
        public static class Sevenam
        {
            public const string HostName = "https://sevenam.vn";
            public const string SiteName = "sevenam.vn";
            public const string FileName = "sevenam_";
        }

        /// <summary>
        /// Constant for Sexyforever page
        /// </summary>
        public static class Sexyforever
        {
            public const string HostName = "https://sexyforever.vn";
            public const string SiteName = "sexyforever.vn";
            public const string FileName = "sexyforever_";
        }
    }
}