using System.Collections.Generic;

namespace AffiliateCrawlers.Models.PageModels
{
    public class LstSizeImage
    {
        public string colorid { get; set; }
        public string image { get; set; }
        public string colorvi { get; set; }
        public string hexcode { get; set; }
        public bool selected { get; set; }
    }

    public class SablancaModel
    {
        public int COLLECTIONID { get; set; }
        public string ITEMID { get; set; }
        public string ITEMNAME { get; set; }
        public string NAMEID { get; set; }
        public int MAINGROUPID { get; set; }
        public int SALEPRICE { get; set; }
        public int RETAILPRICE { get; set; }
        public int DISCOUNTPERCENT { get; set; }
        public bool ISSAMEPRICE { get; set; }
        public bool ISNEW { get; set; }
        public string MATERIAL { get; set; }
        public bool ISLOCKEDORDER { get; set; }
        public string IMAGE { get; set; }
        public string HEXCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public int PRODUCTBLOGID { get; set; }
        public string COLORDEFAULT { get; set; }
        public bool ONLYSELLONLINE { get; set; }
        public string LISTIMAGE { get; set; }
        public string HEELHEIGHT { get; set; }
        public string MATERIALTEXT { get; set; }
        public string STYLETEXT { get; set; }
        public string SUBGROUPMAP { get; set; }
        public string SUBGROUPNAME { get; set; }
        public string COMPARTMENT { get; set; }
        public string DIMENSION { get; set; }
        public string MAINGROUPLINK { get; set; }
        public List<string> LISTIMAGES { get; set; }
        public string STRAPTYPETEXT { get; set; }
        public object COLORDEFAULTTEXT { get; set; }
        public object PATTERN { get; set; }
        public List<LstSizeImage> lstSizeImage { get; set; }
    }
}