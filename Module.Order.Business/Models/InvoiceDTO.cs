using System;
using System.Collections.Generic;
using System.IO;
using KristaShop.Common.Extensions;

namespace Module.Order.Business.Models {
    
    public enum InvoiceCurrency {
        USD = 0,
        RUB
    }
    
    public class InvoiceDTO {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string CityName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerFullName { get; set; }

        public string InvoiceClientTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public string InvoiceNum { get; set; }
        public InvoiceCurrency Currency { get; set; }
        public double PrePay { get; set; }
        public double TotalPay { get; set; }
        public double ExchangeRate { get; set; }
        public bool WasPayed { get; set; }
        public bool IsPrePay { get; set; }

        private string _basePath = "";
        public void setBaseDirectory(string path) {
            _basePath = path.Trim().TrimEnd('/').TrimEnd('\\') + "/";
        }

        public bool HasAttachedFile {
            get {
                return File.Exists(_basePath + AttachedFileName + ".pdf") || File.Exists(_basePath + AttachedFileName + ".xlsx") || File.Exists(_basePath + AttachedFileName + ".xls");
            } 
        }

        public string AttachedFileName {
            get {
                return $"{UserId}_{InvoiceNum}_{CreateDate.ToString("yyyy-MM-dd")}";
            }
        }

        public string AttachedFullFileName {
            get {
                if (File.Exists(_basePath + AttachedFileName + ".pdf")) {
                    _ext = ".pdf";
                    return _basePath + AttachedFileName + ".pdf";
                } else if (File.Exists(_basePath + AttachedFileName + ".xlsx")) {
                    _ext = ".xlsx";
                    return _basePath + AttachedFileName + ".xlsx";
                } else if (File.Exists(_basePath + AttachedFileName + ".xls")) {
                    _ext = ".xls";
                    return _basePath + AttachedFileName + ".xls";
                } else {
                    _ext = ".pdf";
                    return null;
                }
            }
        }

        private string _ext = ".pdf";
        public string AttachedBriefFileName {
            get {
                return AttachedFileName + _ext;
            }
        }

        public string CurrencySign {
            get {
                if (Currency == InvoiceCurrency.USD) {
                    return "$";
                } else {
                    return "р";
                }
            }
        }

        public string PrePayFormated {
            get {
                if (Currency == InvoiceCurrency.USD) {
                    return $"{Math.Abs(PrePay).ToTwoDecimalPlaces()} $";
                } else {
                    return $"{Math.Abs(PrePay * ExchangeRate).ToTwoDecimalPlaces()} р";
                }
            }
        }

        public string TotalPayFormated {
            get {
                if (Currency == InvoiceCurrency.USD) {
                    return $"{TotalPay.ToTwoDecimalPlaces()} $";
                } else {
                    return $"{(TotalPay * ExchangeRate).ToTwoDecimalPlaces()} р";
                }
            }
        }

        public List<InvoiceLineDTO> Lines { get; set; }

        public InvoiceDTO() {
            Lines = new List<InvoiceLineDTO>();
        }
    }
}
