Исходный код плагина был изменен специально для проекта Krista
В settings был добавлен:
recoupMilliseconds - целое значение миллисекунд на которое сдвигается текущее время пользователя

Добавлена строка в метод currentDate:

// shift user current datetime on specific amount of milliseconds
new_date.setMilliseconds(settings.recoupMilliseconds);

Это нужно для того, чтобы текущее время пользователя соответствовало серверному времени