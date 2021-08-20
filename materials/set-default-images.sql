UPDATE video_gallery 
SET preview_path = '/img/default/banner.jpg'
WHERE preview_path LIKE '/galleryphotos/%';

UPDATE gallery_video
SET preview_path = '/img/default/gallery.jpg'
WHERE preview_path LIKE '/galleryphotos/%';

UPDATE blog_items
SET image_path = '/img/default/gallery.jpg'
WHERE image_path LIKE '/galleryphotos/%';

UPDATE gallery_items
SET image_path = '/img/default/gallery.jpg'
WHERE image_path LIKE '/galleryphotos/%';

UPDATE dict_category
SET image_path = '/img/default/category.jpg'
WHERE image_path LIKE '/galleryphotos/%';

UPDATE catalog_item_descriptor
SET main_photo = '/img/default/model.jpeg'
WHERE main_photo LIKE '/galleryphotos/%';

UPDATE model_photos_1c
SET photo_path = '/img/default/model.jpeg'
WHERE photo_path LIKE '/galleryphotos/%';

UPDATE banner_items
SET image_path = '/img/default/banner.jpg'
WHERE image_path LIKE '/galleryphotos/%';

UPDATE menu_contents
SET image_path = '/img/default/about.png'
WHERE image_path LIKE '/galleryphotos/%';

UPDATE menu_contents
SET title_icon_path = '/img/default/about-icon.svg'
WHERE title_icon_path LIKE '/galleryphotos/%';

UPDATE menu_contents 
SET body = REPLACE (body, '%/fileserver/%.%', '/img/noimage.png')
WHERE body LIKE '%/fileserver/%.% %';

UPDATE menu_contents 
SET body = REGEXP_REPLACE (body, 'src="\/fileserver.*"', 'src="/img/noimage.png"')
WHERE body LIKE '%/fileserver/%.% %';