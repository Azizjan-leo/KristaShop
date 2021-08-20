$(function () {
    openModalForAnchor();

    function openModalForAnchor() {
        const anchor = window.location.hash.substr(1);
        const modalWindow = $(`#modal-${anchor}`);
        if (modalWindow.length > 0) {
            modalWindow.modal("show");
        }
    }
})