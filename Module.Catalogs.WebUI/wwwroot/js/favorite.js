function deleteFavorite(form) {
    var card = $(form).parents(".card");
    removeCard(card);
}

function removeCard(card) {
    var element = card;
    if (card.parent().hasClass("col")) {
        element = card.parent();
    }

    $(element).fadeOut(400, "swing", function() {
         element.remove();
         if ($(".card [name='favorite']").length < 1) {
             location.reload(true);
         }
    });
}

//paginator current page image
$(document).ready(function () {
    $(".niloclass a").html('<img src="/common/img/svg/paginator-current.svg" width="17px">');
});