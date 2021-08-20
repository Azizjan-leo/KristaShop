function UpdateNavbarCartInfo(amountAdded) {
    const navbarCartObj = $(".jsCartNavbar");
    if (navbarCartObj.length > 0) {
        const cartEmptyWrapper = navbarCartObj.find(".jsCartEmpty");
        const cartWrapper = navbarCartObj.find(".jsCartFull");
        const cartAmountWrapper = navbarCartObj.find(".jsCartCountValue");
        const cartAmountWrapperMobile = $(".jsCartCountValueMobile");
        let cartAmount = +cartAmountWrapper.first().text();
        
        if(cartAmount + amountAdded <= 0) { 
            cartWrapper.hide();
            cartEmptyWrapper.show();
            cartAmountWrapper.text("0");
            
            cartAmountWrapperMobile.hide();
            cartAmountWrapperMobile.text("0");
        } else {
            cartWrapper.show();
            cartEmptyWrapper.hide();
            cartAmountWrapperMobile.show();
            cartAmountWrapper.text(cartAmount + amountAdded);

            cartAmountWrapperMobile.show();
            cartAmountWrapperMobile.text(cartAmount + amountAdded);
        }
    }
}