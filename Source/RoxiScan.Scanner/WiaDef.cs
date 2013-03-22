
namespace RoxiScan
{
    internal class WIA_PROPERTIES
    {
        public const uint WIA_DPS_HORIZONTAL_BED_SIZE = 3074;
        public const uint WIA_DPS_VERTICAL_BED_SIZE = 3075;

        public const uint WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES = 3086;
        public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = 3087;
        public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT = 3088;
        public const uint WIA_DPS_DOCUMENT_HANDLING_CAPACITY = 3089;

        public const uint WIA_DPS_PAGE_SIZE = 3097;

        public const uint WIA_DPS_PAGE_WIDTH = 3098;
        public const uint WIA_DPS_PAGE_HEIGHT = 3099;

        public const uint WIA_DPS_PREVIEW = 3100;

        public const uint WIA_IPS_MAX_HORIZONTAL_SIZE = 6165;
        public const uint WIA_IPS_MAX_VERTICAL_SIZE = 6166;
    }

    public class WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES
    {
        public const uint FEED = 0x01;
        public const uint FLAT = 0x02;
    }


    public class WIA_DPS_DOCUMENT_HANDLING_STATUS
    {
        public const uint FEED_READY = 0x01;
        public const uint FLAT_READY = 0x02;
        public const uint DUP_READY = 0x04;
        public const uint FLAT_COVER_UP = 0x08;
        public const uint PATH_COVER_UP = 0x10;
        public const uint PAPER_JAM = 0x20;
        public const uint FILM_TPA_READY = 0x40;
        public const uint STORAGE_READY = 0x80;
        public const uint STORAGE_FULL = 0x100;
        public const uint MULTIPLE_FEED = 0x200;
        public const uint DEVICE_ATTENTION = 0x400;
        public const uint LAMP_ERR = 0x800;
    }

    //
    // WIA_DPS_DOCUMENT_HANDLING_SELECT flags
    //
    public class WIA_DPS_DOCUMENT_HANDLING_SELECT
    {
        public const uint FEEDER = 0x001;
        public const uint FLATBED = 0x002;
        public const uint DUPLEX = 0x004;
        public const uint FRONT_FIRST = 0x008;
        public const uint BACK_FIRST = 0x010;
        public const uint FRONT_ONLY = 0x020;
        public const uint BACK_ONLY = 0x040;
        public const uint NEXT_PAGE = 0x080;
        public const uint PREFEED = 0x100;
        public const uint AUTO_ADVANCE = 0x200;
        public const uint ADVANCED_DUPLEX = 0x400;
    }

    public class WIA_IMAGE_FORMAT
    {
        public const string BMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        public const string PNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
        public const string GIF = "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}";
        public const string JPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
        public const string TIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";
    }

    public class WIA_ERROR
    {
        public const uint WIA_ERROR_GENERAL_ERROR = 0x80210001;
        public const uint WIA_ERROR_PAPER_JAM = 0x80210002;
        public const uint WIA_ERROR_PAPER_EMPTY = 0x80210003;
        public const uint WIA_ERROR_PAPER_PROBLEM = 0x80210004;
        public const uint WIA_ERROR_OFFLINE = 0x80210005;
        public const uint WIA_ERROR_BUSY = 0x80210006;
        public const uint WIA_ERROR_WARMING_UP = 0x80210007;
        public const uint WIA_ERROR_USER_INTERVENTION = 0x80210008;
        public const uint WIA_ERROR_ITEM_DELETED = 0x80210009;
        public const uint WIA_ERROR_DEVICE_COMMUNICATION = 0x8021000A;
        public const uint WIA_ERROR_INVALID_COMMAND = 0x8021000B;
        public const uint WIA_ERROR_INCORRECT_HARDWARE_SETTING = 0x8021000C;
        public const uint WIA_ERROR_DEVICE_LOCKED = 0x8021000D;
        public const uint WIA_ERROR_EXCEPTION_IN_DRIVER = 0x8021000E;
        public const uint WIA_ERROR_INVALID_DRIVER_RESPONSE = 0x8021000F;
    }
}
