using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.Structs;

namespace Module.Partners.Business.DTOs {
    public class DocumentDTO<TItems> where TItems : class {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public ulong Number { get; set; }
        public string FormattedNumber => Number.ToString("D5");
        public DateTimeOffset CreateDate { get; set; }
        public string CreateDateAsString => CreateDate.ToBasicString();
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string Name { get; set; }
        public string Descriptor => $"Документ {Name} №{FormattedNumber} от {CreateDateAsString}";
        
        public virtual MovementDirection Direction { get; set; }
        public ICollection<TItems> Items { get; set; }
        public ICollection<DocumentDTO<TItems>> Children { get; set; }
        public State State { get; set; }
        public string NextActionName { get; set; }

        public ReportTotalInfo Totals { get; set; }

        public virtual bool CanHaveGroupedItems { get; set; } = false;
    }

    public class IncomeDocumentDTO<TItems>  : DocumentDTO<TItems> where TItems : class {
        public override bool CanHaveGroupedItems { get; set; } = true;
    }
    
    public class RevisionDocumentDTO<TItems> : DocumentDTO<TItems> where TItems : class {

        public bool HasDeficiencyDocument() {
            return Children.Any(x => x.GetType() == typeof(RevisionDeficiencyDocumentDTO<TItems>));
        }
        
        public RevisionDeficiencyDocumentDTO<TItems> GetDeficiencyDocument() {
            return Children.First(x => x.GetType() == typeof(RevisionDeficiencyDocumentDTO<TItems>)) as RevisionDeficiencyDocumentDTO<TItems>;
        }
        
        public override bool CanHaveGroupedItems { get; set; } = true;
    }

    public class RevisionDeficiencyDocumentDTO<TItems>  : DocumentDTO<TItems> where TItems : class {
        
    }

    public class RevisionExcessDocumentDTO<TItems>  : DocumentDTO<TItems> where TItems : class {
        
    }

    public class MoneyDocumentDTO<TItems>  : DocumentDTO<TItems> where TItems : class {
        public double Sum { get; set; }
    }
    
    public class PaymentDocumentDTO<TItems>  : MoneyDocumentDTO<TItems> where TItems : class {
        
    }

    public class SellingRequestDocumentDTO<TItems> : DocumentDTO<TItems> where TItems : class {
        
    }
}