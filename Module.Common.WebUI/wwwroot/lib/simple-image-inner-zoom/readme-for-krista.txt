Исходный код плагина был изменен специально для проекта Krista для работы с плагином lightbox2
В блок кода на строке 158 были добавлены следующие строки:
    stop();
    clicked = false;
    $(document).off(mousemove, zoom.move);
    $(document).off('click.zoom');


Данный блок кода был изменен:
	$source.on('click.zoom',
						function (e) {
							if (clicked) {
								stop();
								clicked = false;
								$(document).off(mousemove, zoom.move);
								$(document).off('click.zoom');	
								return;
							} else {
								clicked = true;
								start(e);
								$(document).on(mousemove, zoom.move);
								$(document).one('click.zoom',
									function () {
										stop();
										clicked = false;
										$(document).off(mousemove, zoom.move);
									}
								);
								return false;
							}
						}
					);

В папке origin лежат не измененный исходный код файлов