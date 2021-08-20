$(document).ready(function () {
    $(".marquee").marquee({
        duration: 30000,
        startVisible: true,
        gap: 4 // px
    });

    $('.tools-flipster').flipster({
        style: 'flat',
        start: 'center',
        loop: true,
        click: true,
        scrollwheel: false,
        touch: true,
        spacing: -0.552
    });

    $('.tools-slider').lightSlider({
        autoWidth: true,
        auto: true,
        speed: 400,
        pause: 5000,
        loop: true,
        pauseOnHover: true,
        pager: false,
        controls: false,
    });
    
    $('[data-become-partner]').on("click", function(){
        const kristaIconHtml = "<img src='/common/img/svg/krista-logo-gradient.svg' width='95' height='48' alt='krista-logo' class='mr-2 pr-1'><img src='/common/img/svg/krista-pic-transparent-sm.gif' width='50' height='42' alt='krista-logo'>";
        Swal.fire({
            icon: "question",
            iconHtml: kristaIconHtml,
            title: 'Партнерство с Krista Exclusive',
            text: 'Вы оставляете заявку на партренство с Krista Exclusive',
            showCancelButton: true,
            cancelButtonText: "Отменить",
            confirmButtonText: "Подтвердить",
            buttonsStyling: false,
            customClass: {
                container: "mt-0",
                popup: "py-4 px-3 px-lg-5",
                icon: "border-0 mt-2 mb-4",
                title: "header-font h4-smaller text-uppercase text-dark text-bold mb-3",
                content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                confirmButton: "btn-basic btn-basic-big btn-main-gradient m-1",
                cancelButton: "btn-basic btn-basic-big btn-main-cancel m-1",
                actions: "my-3 pb-1"
            }
        }).then((result) => {
            if(!result.isConfirmed) return;

            $.ajax({
                type: "POST",
                url: "/Partnership/Apply"
            }).done(function (responseText) {
                if (responseText.isSuccess) {
                    Swal.fire({
                        icon: "question",
                        iconHtml: kristaIconHtml,
                        title: 'Заявка принята',
                        text: responseText.messages.join('. '),
                        showCancelButton: true,
                        cancelButtonText: "Закрыть",
                        showConfirmButton: false,
                        buttonsStyling: false,
                        customClass: {
                            container: "mt-0",
                            popup: "py-4 px-3 px-lg-5",
                            icon: "border-0 mt-2 mb-4",
                            title: "header-font h4-smaller text-uppercase text-dark text-bold mb-3",
                            content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                            cancelButton: "btn-basic btn-basic-big btn-main-gradient m-1",
                            actions: "my-3 pb-1"
                        }
                    });
                } else {
                    Swal.fire({
                        icon: "question",
                        iconHtml: kristaIconHtml,
                        title: 'Извините',
                        text: responseText.messages.join('. '),
                        showCancelButton: true,
                        cancelButtonText: "Закрыть",
                        showConfirmButton: false,
                        buttonsStyling: false,
                        customClass: {
                            container: "mt-0",
                            popup: "py-4 px-3 px-lg-5",
                            icon: "border-0 mt-2 mb-4",
                            title: "header-font h4-smaller text-uppercase text-danger text-bold mb-3",
                            content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                            cancelButton: "btn-basic btn-basic-big btn-main-gradient m-1",
                            actions: "my-3 pb-1"
                        }
                    });
                }
            }).fail(function (jqXHR) {
                Swal.fire({
                    icon: "question",
                    iconHtml: kristaIconHtml,
                    title: 'Ошибка',
                    text: 'Произошла ошибка. Пожалуйста, попробуйте позже',
                    showCancelButton: true,
                    cancelButtonText: "Закрыть",
                    showConfirmButton: false,
                    buttonsStyling: false,
                    customClass: {
                        container: "mt-0",
                        popup: "py-4 px-3 px-lg-5",
                        icon: "border-0 mt-2 mb-4",
                        title: "header-font h4-smaller text-uppercase text-danger text-bold mb-3",
                        content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                        cancelButton: "btn-basic btn-basic-big btn-main-gradient m-1",
                        actions: "my-3 pb-1"
                    }
                });
            });
        });
    });
});

//#region youtube video on background
// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/player_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

// 3. This function creates an <iframe> (and YouTube player)
// after the API code downloads.
var player;

function onYouTubePlayerAPIReady() {
    const videoId = $("#yt-wrap").find("iframe").data("videoId");
    player = new YT.Player('ytplayer', {
        width: '100%',
        height: '100%',
        videoId: videoId,
        playerVars: {
            'autoplay': 1,
            'showinfo': 0,
            'autohide': 1,
            'loop': 1,
            'controls': 0,
            'modestbranding': 1,
            'vq': 'hd1080'
        },
        events: {
            'onReady': onPlayerReady,
            'onStateChange': onPlayerStateChange
        }
    });
}

// 4. The API will call this function when the video player is ready.
function onPlayerReady(event) {
    event.target.playVideo();
    player.mute(); // comment out if you don't want the auto played video muted
    const wrapper = $("#yt-wrap");
    wrapper.find(".cover").hide();
    wrapper.addClass("stretched");
}

// 5. The API calls this function when the player's state changes.
// The function indicates that when playing a video (state=1),
// the player should play for six seconds and then stop.
function onPlayerStateChange(event) {
    if (event.data == YT.PlayerState.ENDED) {
        player.seekTo(0);
        player.playVideo();
    }
}

function stopVideo() {
    player.stopVideo();
}

//#endregion run youtube video on background