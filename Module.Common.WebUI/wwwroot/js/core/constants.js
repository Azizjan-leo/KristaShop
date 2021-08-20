const Constants = {
    catalogs: {
        inStockLines: 1,
        inStockParts: 2,
        preorder: 3,
        rfInStockLines: 4,
        rfInStockParts: 5,
        saleLines: 6,
        saleParts: 7
    },
    extraCharges: {
        Rf: 1
    },
    DocumentStates: {
        None: -1,
        Created: 0,
        ApproveAwait: 1,
        Approved: 2,
        Completed: 3,
        NotPaid: 4,
        PaymentAwait: 5,
        Paid: 6
    }
}