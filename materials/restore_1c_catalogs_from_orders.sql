-- Данный скрипт обновляет остатки моделей предзаказа в 1c_catalog
-- из количества моделей в заказах

-- он нужен в случае если сделали выгрузку из 1с на сайт предварительно не обновив остатки в 1с
-- и выгрузка сбросила все остатки в начальные значения, которые были до совершения заказов

UPDATE `1c_catalog` AS cat 
JOIN `1c_models` as models ON cat.model = models.id
JOIN (
	SELECT details.nomenclature_id, details.model_id, details.size_value,
	details.color_id, details.storehouse_id, SUM(details.amount) AS amount
	FROM for1c_orders AS orders
	LEFT JOIN for1c_order_details AS details ON orders.id = details.order_id
	WHERE orders.create_date > 'enter date here' AND details.storehouse_id < 1
	GROUP BY details.nomenclature_id, details.model_id, details.size_value,
	details.color_id, details.storehouse_id
) AS det ON cat.model = det.model_id AND cat.color  = det.color_id AND (models.line = det.size_value OR cat.razmer = det.size_value)
SET cat.kolichestvo = (cat.kolichestvo - det.amount);