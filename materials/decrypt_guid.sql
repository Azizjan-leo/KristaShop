
DELIMITER //

CREATE FUNCTION GUID_TO_BINARY(value varchar(64))
RETURNS binary(16) DETERMINISTIC
BEGIN
	DECLARE result_binary binary(16);
	SELECT UNHEX(CONCAT(
			substr(value, 7, 2), substr(value, 5, 2), substr(value, 3, 2), substr(value, 1, 2), 
			substr(value, -25, 2), substr(value, -27, 2), 
			substr(value, -20, 2), substr(value, -22, 2), 
			substr(value, -17, 4),
			substr(value, -12, 12))) INTO result_binary;
	RETURN result_binary;
END;

//

DELIMITER ;


DELIMITER //

CREATE FUNCTION BINARY_TO_GUID(value binary(16))
RETURNS varchar(36) DETERMINISTIC
BEGIN
	DECLARE tmp_str varchar(32);
	DECLARE result_str varchar(36);
	SELECT HEX(value) INTO tmp_str;
	SELECT LOWER(CONCAT(
			substr(tmp_str, 7, 2), substr(tmp_str, 5, 2), substr(tmp_str, 3, 2), substr(tmp_str, 1, 2), '-',
			substr(tmp_str, -22, 2), substr(tmp_str, -24, 2), '-',
			substr(tmp_str, 15, 2), substr(tmp_str, 13, 2),  '-',
			substr(tmp_str, 17, 4), '-',
			substr(tmp_str, -12, 12))) INTO result_str;
	RETURN result_str;
END;

//

DELIMITER ;

