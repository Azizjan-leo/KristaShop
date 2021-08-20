/* Modal windows handlers*/

/* Youtube modal */
$('*[data-toggle="video-modal"]').on("click", openVideoModal);

function openVideoModal(event) {
    var dialog = $($(this).attr("data-target"));
    var videoContainer = dialog.find(".modal-body").find(".video-wrapper");
    videoContainer.attr("src", $(this).attr("data-source"));

    dialog.on("hide.bs.modal", function () { stopVideos(); });
    dialog.modal("toggle");
}

var stopVideos = function () {
    var videos = document.querySelectorAll('iframe, video');
    Array.prototype.forEach.call(videos, function (video) {
        if (video.tagName.toLowerCase() === 'video') {
            video.pause();
        } else {
            var src = video.src;
            video.src = src;
        }
    });
};